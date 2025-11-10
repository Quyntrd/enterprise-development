# 🚲 BicycleRental API

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![AutoMapper](https://img.shields.io/badge/AutoMapper-12.0.1-orange)
![Swagger](https://img.shields.io/badge/Swagger-6.7.0-green)

## Структура проекта

### Domain

Содержит основные доменные сущности:

- **BicycleModel** — модель велосипеда (название, тип, цена за час)
- **Bicycle** — конкретный велосипед, связанный с моделью
- **Renter** — арендатор, содержащий персональные данные
- **Rental** — аренда велосипеда (начало, длительность, арендатор, велосипед)

Также определён универсальный интерфейс **`IRepository<TEntity, TKey>`**  
для работы с различными хранилищами данных.

---

### Application.Contracts

DTO-классы и контракты для API:

- **`EntityDto`** — используется для возврата сущностей из запросов  
- **`EntityCreateUpdateDto`** — для создания и обновления сущностей  
- **`RentalDto` / `RentalCreateUpdateDto`** — DTO для аренды с полями  
  `BicycleId`, `RenterId`, `StartAt`, `DurationHours`

Содержит интерфейсы сервисов (`IBicycleService`, `IRenterService`, `IRentalService` и т.д.),  
реализуемые в проекте **Application**.

---

### Application

Реализует бизнес-логику и сервисы, использующие контракты из `Application.Contracts`.

Сервисы:

- `BicycleService`
- `BicycleModelService`
- `RenterService`
- `RentalService`

Используется **AutoMapper** для маппинга между сущностями и DTO.  
Метод `MapWithPrice()` в `RentalService` рассчитывает стоимость аренды по длительности и модели велосипеда.

---

### Infrastructure.InMemory

Реализация in-memory репозиториев, позволяющая работать без базы данных.

Каждый репозиторий наследуется от `InMemoryRepository<TEntity>`  
и реализует `IRepository<TEntity, int>`.

Реализованы репозитории:

- `BicycleModelInMemoryRepository`
- `BicycleInMemoryRepository`
- `RenterInMemoryRepository`
- `RentalInMemoryRepository`

Используются для тестирования и отладки без внешних зависимостей.

---

### Api

Основной REST API-проект, реализующий контроллеры, Swagger и логирование.

Контроллеры наследуются от базового класса:

```csharp
CrudControllerBase<TDto, TCreateUpdateDto, TKey>
```

#### Контроллеры:

- `BicycleModelsController`
- `BicyclesController`
- `RentersController`
- `RentalsController`

Каждый контроллер предоставляет CRUD-операции:
`GET /api/{entity}`, `GET /api/{entity}/{id}`, `POST`, `PUT`, `DELETE`

#### Особенности:
- Для `Rentals` используется `TimeSpanSchemaFilter`, отображающий `DurationHours` корректно как `"hh:mm:ss"`.
- Swagger включает XML-комментарии из всех зависимых сборок.
- Все контроллеры используют `ILogger` для логирования ошибок и действий.

---

### Swagger

Фильтр **`TimeSpanSchemaFilter`**, расположенный в:

```
BicycleRental.Api/Swagger/TimeSpanSchemaFilter.cs
```

Корректирует отображение `TimeSpan` в Swagger UI:

Пример корректного тела запроса:

```json
{
  "bicycleId": 1,
  "renterId": 2,
  "startAt": "2025-11-10T12:00:00Z",
  "durationHours": "03:00:00"
}
```

---

### Program.cs

Файл `BicycleRental.Api/Program.cs` выполняет:

- Регистрацию AutoMapper (`BicycleRentalProfile`)
- Регистрацию in-memory репозиториев и сервисов
- Настройку контроллеров
- Подключение Swagger с XML-комментариями всех сборок

Swagger автоматически добавляет документацию для всех проектов решения.

---

### Пример API-запросов

#### Получить все аренды:
```
GET /api/Rentals
```

#### Создать аренду:
```json
POST /api/Rentals
{
  "bicycleId": 1,
  "renterId": 2,
  "startAt": "2025-11-10T10:00:00Z",
  "durationHours": "02:00:00"
}
```

#### Обновить аренду:
```json
PUT /api/Rentals/1
{
  "bicycleId": 1,
  "renterId": 2,
  "startAt": "2025-11-10T12:00:00Z",
  "durationHours": "04:00:00"
}
```

#### Удалить аренду:
```
DELETE /api/Rentals/1
```

---

### Используемые технологии

| Технология | Версия | Назначение |
|-------------|---------|-------------|
| **.NET** | 8.0 | Платформа выполнения |
| **C#** | 12.0 | Язык программирования |
| **AutoMapper** | 12.0.1 | Маппинг DTO ↔ Entity |
| **Swagger (Swashbuckle)** | 6.7.0 | Документация и тестирование API |
| **ILogger** | — | Логирование ошибок и действий |
| **In-Memory Repository** | — | Тестовое хранилище данных без БД |

---

### Запуск проекта

```bash
cd BicycleRental.Api
dotnet run
```

Swagger UI доступен по адресу:
```
https://localhost:5001/swagger
```
