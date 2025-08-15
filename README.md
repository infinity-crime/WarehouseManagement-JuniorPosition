# WarehouseManagement (Приложение для управления складом) - тестовое задание на позицию .NET Developer (C#) Junior
## Быстрый старт для разработчика
Данный проект использует PostgreSQL. В нем реализовано автоматическое создание БД и применение миграций, чтобы все работало, требуются следующие шаги:
1. Установить PostgreSQL (https://www.postgresql.org/download/) См. интрукцию по установке на официальном сайте.
2. В файле appsettings.json изменить пароль-заглушку (Password=YOUR_PASSWORD) на свой пароль от сервера БД.
```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WarehouseDb;Username=postgres;Password=YOUR_PASSWORD"
  }
```
4. Запустить проект.
