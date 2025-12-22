# 🚲 BicycleRental API
![.NET](https://img.shields.io/badge/.NET-8.0-blue) ![C#](https://img.shields.io/badge/C%23-12.0-blue) ![AutoMapper](https://img.shields.io/badge/AutoMapper-12.0.1-orange) ![Swagger](https://img.shields.io/badge/Swagger-6.7.0-green) ![EF Core](https://img.shields.io/badge/EF%20Core-8.0-blue) ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-enabled-orange) ![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=flat&logo=Blazor&logoColor=gray&label=.NET&labelColor=e9e9e9)

## Краткое описание

BicycleRental — реализация сервисной архитектуры для управления прокатом велосипедов. Проект разделён на несколько слоёв: `Domain`, `Application.Contracts`, `Application`, `Infrastructure` (EF Core), `Api`,  `Generator.RabbitMq.Host`, `Wasm`.

Проект реализует асинхронные CRUD-сервисы, модель данных для велосипедов, моделей велосипедов, арендаторов и самих аренды, а также вспомогательные механизмы для генерации тестовых данных и публикации их в RabbitMQ.

---

## Содержание репозитория

- `BicycleRental.Domain` — доменные модели, перечисления, интерфейс `IRepository<TEntity, TKey>` и `BicycleRentalDataSeed` для фикстур.
- `BicycleRental.Application.Contracts` — DTO и интерфейсы сервисов (контракты приложения).
- `BicycleRental.Application` — реализации сервисов и AutoMapper профиль `BicycleRentalProfile`.
- `BicycleRental.Infrastructure.EfCore` — реализации репозиториев через EF Core и контекст `BicycleRentalDbContext`.
- `BicycleRental.Api` — веб-API с контроллерами, Swagger и конфигурацией.
- `BicycleRental.Generator.RabbitMq.Host` — сервис генерации тестовых DTO и публикации в RabbitMQ.
- `BicycleRental.Wasm` — клиентская часть с реализацией основных операций по удалению, созданию записей.

---

## Основные сущности (Domain)

- **BicycleModel** — свойства: `Id`, `Name`, `Type`, `WheelSizeInInches`, `MaxPassengerWeightKg`, `WeightKg`, `BrakeType`, `ModelYear`, `PricePerHour`.
- **Bicycle** — свойства: `Id`, `SerialNumber`, `ModelId`, `Color`.
- **Renter** — свойства: `Id`, `FirstName`, `LastName`, `Patronymic`, `Phone`.
- **Rental** — свойства: `Id`, `BicycleId`, `RenterId`, `StartAt`, `DurationHours` (TimeSpan).
- **BicycleType** — `City`, `Mountain`, `Road`, `Electric`, `Sport`.

Интерфейс для репозиториев: `IRepository<TEntity, TKey>` предоставляет асинхронные методы CRUD: `Create`, `Update`, `Delete`, `Read`, `ReadAll`.

---

## Application.Contracts (DTO и контракты)

- `BicycleModelDto`, `BicycleModelCreateUpdateDto`
- `BicycleDto`, `BicycleCreateUpdateDto`
- `RenterDto`, `RenterCreateUpdateDto`
- `RentalDto`, `RentalCreateUpdateDto`

Контракты сервисов расширяют общий `IApplicationService<TDto, TCreateUpdateDto, TKey>` и добавляют специфичные методы:

- `IBicycleModelService` — `GetBicycles(int dtoId)`
- `IBicycleService` — `GetByModelId(int modelId)`
- `IRenterService` — `GetRentals(int dtoId)`
- `IRentalService` — `GetByBicycleId(int bicycleId)`, `GetByRenterId(int renterId)`

---

## Application (сервисы и логика)

Реализованы сервисы, использующие репозитории и AutoMapper:

- **BicycleModelService** — CRUD + `GetBicycles` (возвращает велосипеды по `ModelId`).
- **BicycleService** — при создании/обновлении проверяет существование модели; CRUD + `GetByModelId`.
- **RenterService** — CRUD + `GetRentals`.
- **RentalService** — CRUD + методы выборки по `BicycleId` и `RenterId`. В `RentalService` реализован `MapWithPrice(Rental r)` — маппинг аренды в `RentalDto` с расчётом `PricePerHour` и `TotalPrice` по текущей цене модели и длительности `DurationHours`.

AutoMapper профиль `BicycleRentalProfile` настраивает отображения между доменными моделями и DTO.

---

## Infrastructure (EF Core)

Реализованы репозитории на основе EF Core и контекста `BicycleRentalDbContext`:

- `BicycleModelEfCoreRepository`
- `BicycleEfCoreRepository`
- `RenterEfCoreRepository`
- `RentalEfCoreRepository`

Подключение к MySQL выполняется через строку подключения `BicycleRentalDatabase`.

---

## API

Контроллеры наследуются от обобщённого базового контроллера:

```csharp
CrudControllerBase<TDto, TCreateUpdateDto, TKey>
```

Контроллеры:

- `BicycleModelsController` — `GET /api/bicyclemodels/{id}/bicycles` — возвращает список велосипедов модели.
- `BicyclesController` — `GET /api/bicycles/{id}/rentals` — возвращает аренды по велосипедом.
- `RentersController` — `GET /api/renters/{id}/rentals` — возвращает аренды арендатора.
- `RentalsController` — обычные CRUD операции для аренды.

Особенности реализации контроллеров:

- Методы асинхронны, содержат логирование (`ILogger`).
- При ошибках возвращаются соответствующие статусы: `400`, `404`, `500` и т.д.
- `CreatedAtAction` используется при успешном создании ресурса (если в ответе присутствует `Id`).

---

## Swagger и TimeSpan

В проект добавлен `TimeSpanSchemaFilter`, который приводит `TimeSpan` к строковому представлению с форматом `hh:mm:ss` в Swagger UI.

Пример тела запроса для создания аренды в Swagger:

```json
{
  "bicycleId": 1,
  "renterId": 2,
  "startAt": "2025-11-10T12:00:00Z",
  "durationHours": "03:00:00"
}
```

---

## Генератор данных и RabbitMQ

`BicycleRental.Generator.RabbitMq.Host` содержит генераторы на базе Bogus для DTO и стратегии публикации в RabbitMQ:

- `BicycleGenerator`, `BicycleModelGenerator`, `RenterGenerator`, `RentalGenerator`.
- `GeneratorExtensions.WithRecord<T>` — позволяет создавать объекты DTO без вызова конструкторов (используется `RuntimeHelpers.GetUninitializedObject`).
- Стратегии публикации реализуют интерфейс `IGeneratorStrategy<T>` и вызывают `IProducerService` для отправки пакетов в очередь.

В `Program.cs` приложение настраивает RabbitMQ клиент с параметрами автоматического восстановления, heartbeat и `DispatchConsumersAsync`.

---

## Program.cs — ключевые моменты конфигурации

- Регистрация AutoMapper и профиля `BicycleRentalProfile`.
- Регистрация репозиториев и сервисов в DI-контейнере.
- Подключение EF Core к MySQL: `AddDbContext<BicycleRentalDbContext>(options => options.UseMySql(conn, ServerVersion.AutoDetect(conn)))`.
- Swagger с подключением XML-комментариев для всех сборок.
- Настройка RabbitMQ клиента и фоновый сервис `BicycleRentalRabbitMqConsumer`.
- Миграции БД выполняются в режиме разработки (при `app.Environment.IsDevelopment()` вызывается `db.Database.Migrate()`).

---


## Wasm - клиент

- Реализован на Blazor.
- Возможность создания, удаления, изменения записей.

---