using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BicycleRental.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bicycle_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<int>(type: "int", nullable: false),
                    wheel_size_in_inches = table.Column<double>(type: "double", nullable: true),
                    max_passenger_weight_kg = table.Column<double>(type: "double", nullable: true),
                    weight_kg = table.Column<double>(type: "double", nullable: true),
                    brake_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model_year = table.Column<int>(type: "int", nullable: true),
                    price_per_hour = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bicycle_models", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "renters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    patronymic = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renters", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bicycles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    serial_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bicycles", x => x.id);
                    table.ForeignKey(
                        name: "FK_bicycles_bicycle_models_model_id",
                        column: x => x.model_id,
                        principalTable: "bicycle_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    bicycle_id = table.Column<int>(type: "int", nullable: false),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    start_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    duration_hours = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_bicycles_bicycle_id",
                        column: x => x.bicycle_id,
                        principalTable: "bicycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rentals_renters_renter_id",
                        column: x => x.renter_id,
                        principalTable: "renters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "bicycle_models",
                columns: new[] { "id", "brake_type", "max_passenger_weight_kg", "model_year", "name", "price_per_hour", "type", "weight_kg", "wheel_size_in_inches" },
                values: new object[,]
                {
                    { 1, "Disc", 100.0, 2024, "SportPro 1000", 7.50m, 4, 8.5, 28.0 },
                    { 2, "Disc", 95.0, 2023, "SpeedX Race", 9.00m, 4, 7.7999999999999998, 28.0 },
                    { 3, "Disc", 120.0, 2022, "MountainMax", 5.50m, 1, 14.0, 27.0 },
                    { 4, "V-Brake", 110.0, 2021, "CityComfort", 3.50m, 0, 12.5, 26.0 },
                    { 5, "Disc", 120.0, 2024, "EcoRide E1", 12.00m, 3, 22.0, 26.0 },
                    { 6, "Caliper", 90.0, 2020, "RoadMaster", 6.00m, 2, 8.9000000000000004, 28.0 },
                    { 7, "Disc", 115.0, 2023, "TrailRunner", 5.75m, 1, 13.0, 27.0 },
                    { 8, "V-Brake", 105.0, 2022, "UrbanLite", 3.00m, 0, 11.0, 26.0 },
                    { 9, "Caliper", 95.0, 2025, "SprintElite", 8.50m, 2, 7.5999999999999996, 28.0 },
                    { 10, "V-Brake", 120.0, 2019, "ComfortCity", 2.50m, 0, 13.5, 26.0 },
                    { 11, "Coaster", 100.0, 2015, "VintageTour", 1.50m, 0, 15.0, 26.0 },
                    { 12, "Disc", 110.0, 2018, "TestModelX", 4.00m, 1, 14.5, 27.0 }
                });

            migrationBuilder.InsertData(
                table: "renters",
                columns: new[] { "id", "first_name", "last_name", "patronymic", "phone" },
                values: new object[,]
                {
                    { 1, "Александр", "Александров", "Александрович", "+7-900-000-0001" },
                    { 2, "Пётр", "Петров", "Петрович", "+7-900-000-0002" },
                    { 3, "Сергей", "Сергеев", "Сергеевич", "+7-900-000-0003" },
                    { 4, "Ольга", "Ольгина", "Олеговна", "+7-900-000-0004" },
                    { 5, "Даниил", "Даниилов", "Даниилович", "+7-900-000-0005" },
                    { 6, "Евгений", "Онегин", null, "+7-900-000-0006" },
                    { 7, "Маркус", "Аврелиус", "Антонинус", "+7-900-000-0007" },
                    { 8, "Фёдор", "Фёдоров", "Фёдорович", "+7-900-000-0008" },
                    { 9, "Алексей", "Смирнов", null, "+7-900-000-0009" },
                    { 10, "Алексей", "Алексеев", "Алексеевич", "+7-900-000-0010" },
                    { 11, "Кирилл", "Кириллов", "Кириллович", "+7-900-000-0011" },
                    { 12, "Михаил", "Михаилов", "Михайлович", "+7-900-000-0012" }
                });

            migrationBuilder.InsertData(
                table: "bicycles",
                columns: new[] { "id", "color", "model_id", "serial_number" },
                values: new object[,]
                {
                    { 1, "Красный", 1, "SN-1001" },
                    { 2, "Чёрный", 2, "SN-1002" },
                    { 3, "Зелёный", 3, "SN-1003" },
                    { 4, "Синий", 4, "SN-1004" },
                    { 5, "Белый", 5, "SN-1005" },
                    { 6, "Красный", 6, "SN-1006" },
                    { 7, "Чёрный", 7, "SN-1007" },
                    { 8, "Серый", 8, "SN-1008" },
                    { 9, "Оранжевый", 9, "SN-1009" },
                    { 10, "Жёлтый", 10, "SN-1010" },
                    { 11, "Коричневый", 11, "SN-1011" },
                    { 12, "Бирюзовый", 12, "SN-1012" }
                });

            migrationBuilder.InsertData(
                table: "rentals",
                columns: new[] { "id", "bicycle_id", "duration_hours", "renter_id", "start_at" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 2, 30, 0, 0), 1, new DateTime(2025, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new TimeSpan(0, 1, 0, 0, 0), 1, new DateTime(2025, 1, 12, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new TimeSpan(0, 3, 0, 0, 0), 1, new DateTime(2025, 2, 3, 16, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 3, new TimeSpan(0, 1, 30, 0, 0), 2, new DateTime(2025, 2, 5, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 4, new TimeSpan(0, 4, 0, 0, 0), 3, new DateTime(2025, 2, 7, 11, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 5, new TimeSpan(0, 0, 30, 0, 0), 4, new DateTime(2025, 2, 10, 9, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 6, new TimeSpan(0, 6, 0, 0, 0), 5, new DateTime(2025, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 7, new TimeSpan(0, 2, 0, 0, 0), 6, new DateTime(2025, 3, 4, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 8, new TimeSpan(0, 12, 0, 0, 0), 7, new DateTime(2025, 3, 10, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 9, new TimeSpan(0, 8, 0, 0, 0), 8, new DateTime(2025, 3, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 2, new TimeSpan(0, 3, 30, 0, 0), 9, new DateTime(2025, 4, 1, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 3, new TimeSpan(0, 1, 0, 0, 0), 10, new DateTime(2025, 4, 3, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 5, new TimeSpan(1, 0, 0, 0, 0), 11, new DateTime(2025, 4, 10, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 9, new TimeSpan(0, 0, 45, 0, 0), 1, new DateTime(2025, 4, 12, 9, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bicycles_model_id",
                table: "bicycles",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_bicycle_id",
                table: "rentals",
                column: "bicycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_renter_id",
                table: "rentals",
                column: "renter_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "bicycles");

            migrationBuilder.DropTable(
                name: "renters");

            migrationBuilder.DropTable(
                name: "bicycle_models");
        }
    }
}
