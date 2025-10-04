# CableShop
Магазин по продаже кабеля. Написан в рамках производственной практики
1. ### Основные возможности:
- Регистрация и авторизация пользователей
- Просмотр каталога кабеля
- Добавления товара в корзину
- Оформление заказа
- Панель администратора

2. ### Используемые технологии:
- .NET 8
- PostgreSQL 18
- Docker
3. ### Структура проекта:
- Состоит из 4 проектов
![img.png](img.png)
- Domain - сущности, интерфейсы
![img_1.png](img_1.png)
- ApplicationCore - содержит единственный класс DependencyInjection, внедрение зависимостей
![img_2.png](img_2.png)
- Infrastructure - работа с базой данных
Содержит конфигурации для создания таблиц, миграции, репозитории с логикой работы бд, сервисы тесно связанные с бд
![img_3.png](img_3.png)
- Web  - содержит контроллеры, представления, статические файлы
![img_4.png](img_4.png)
4. ### База данных
- Cables
  - CableId (uuid)
  - CableName (text)
  - Price (integer)
  - Image (text)
  - CableDescription (text)
  - CategoryId (uuid)
- CartItems
  - CartItemId (uuid)
  - CalbeId (uuid)
  - Quantity (integer)
  - TotalPrice (numeric(18,2))
  - Status (text)
  - OrderId (uuid)
  - UserId (uuid)
- Categories
  - CategoryId (uuid)
  - Title (text)
- Orders
  - OrderId (uuid)
  - UserId (uuid)
  - OrderDate (timestamp)
  - ShippingAddress (text)
- Users
  - UserId (uuid)
  - Password (text)
  - Email (text)
  - FirstName (text)
  - PhoneNumber (text)
  - Role (boolean)

# Документация классов проекта
## class DependencyInjection
Класс регистрирует сервисы и репозитории (Внедрение зависимотей)
### Entities:
Cable - сущность для кабелей    
CartItem - сущность для корзины     
Category - сущность для категорий   
Order - сущность для заказа     
User - сущность для пользователя

### DTO (Data Transfer Object):
#### User   
Login - содержит 2 поля email, password. Используется только для входа в систему    
Register - содержит основные поля для регистрации пользователя. Используется только для регистрации в системе
UpdateUserDto - Содержит Id пользователя, имя, фамилию, email, номер телефона, пароль. Не используется
#### Category
UpdateCategoryDto - содержит 2 поля id, title. Используется только для обновления названия категории

### Interfaces
Классы определяют функционал, набор методов и свойств без реализации

### DataContext
Контекст базы данных для приложения, содержит DbSet-ы всех сущностей. Конфигурирует сущности с помощью Configuration классов
### PasswordHasher 
Сервис для хеширования паролей и проверки их соответствия. Метод Generate создает хеш из пароля. Метод Verify проверяет пароль на соответствие хешу
### JwtProvider
Провайдер для генерации jwt Токенов пользователей. Метод GenerateToken принимает объект пользователя для генерации токена, возвращает строковое значение токена. Метод создает claims с id пользователя и ролью
### JwtOptions
Опции для конфигурации jwt токенов
### AuthService 
Реализует интерфейс IAuth для аутентификации и регистрации пользователей.Метод Register с параметрами Register userRegister (dto для регистрации), возвращает Task<>. Метод хеширует пароль из dto, создает новый объект User, вызывает метод для сохранения в БД   
Метод Login - параметры email, password. Возвращает сгенерированный jwt токен 
### Repositories 
Репозитории для CRUD операций в БД
### ConfigurationEntities 
Конфигурации для сущностей, используются для создания зависимостей и полей в БД

