# LMS Platform (ASP.NET Core .NET 8)

Трислойно ASP.NET Core MVC приложение (LMS – Learning Management System) за управление на учебни материали за професионални гимназии. Проектът използва ASP.NET Core Identity, EF Core (Code First) и роли: **Admin / Teacher / Student**.

## ✨ Основни функционалности
- Публична част: начална страница, информация, правила, списък училища и професии
- Търсене и филтриране по училище/професия/материали
- Регистрация/вход (Identity)
- Одобрение на потребители от Admin (IsApproved)
- Управление на материали:
  - Teacher: Create/Edit/Delete
  - Student: Read-only
- Admin Area:
  - Dashboard (статистика)
  - Одобрение/отказ на потребители
  - CRUD за училища/професии (по задание)

## 🧱 Архитектура (3 слоя)
- **LmsPlatform.Web** – Presentation (MVC, Areas/Admin, ViewModels)
- **LmsPlatform.Core** – Business (Contracts + Services + DTOs)
- **LmsPlatform.Infrastructure** – Data (EF Core, Identity, DbContext, Migrations, Seeding)

## 🛠 Технологии
- .NET 8, ASP.NET Core MVC
- Entity Framework Core (Code First + Migrations)
- SQL Server
- ASP.NET Core Identity (Roles)

## 🚀 Стартиране (за проверяващ)
### 1) Настрой connection string
В `LmsPlatform.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LmsPlatformDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
