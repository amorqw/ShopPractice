# Документация API

## CableManage
### GET /admin/cable
Получить список всех кабелей
Response: страница в админ панели со списком всех кабелей
### GET /admin/cable/{id}
request: /admin/cable/434f0e81-6d93-4f6c-ba83-ccfe55bb8dc9      
response: Страница в админ панели с выбранным кабелем, используется для изменения полей в сущности
### GET /admin/cable/add
request: /admin/cable/add           
response: Страница для создания кабеля через админ панель. Все поля являются обязательными для заполнения, должна существовать выбранная категория для создания кабеля
### POST /admin/cable/add
request: /admin/cable/add?CableId=<uuid>&CableName=<string>&Price=<integer>&Image=<string>&CableDescription=<string>&CartItems=[object Object]&CartItems=[object Object]&CategoryId=<uuid>&Category.CategoryId=<uuid>&Category.Title=<string>&Category.Cables=[object Object]&Category.Cables=[object Object]       
response: Создание кабеля по введенным администратором данным
### POST /admin/cable/update/{id}
request: /admin/cable/update/:id?CableId=<uuid>&CableName=<string>&Price=<integer>&Image=<string>&CableDescription=<string>&CartItems=[object Object]&CartItems=[object Object]&CategoryId=<uuid>&Category.CategoryId=<uuid>&Category.Title=<string>&Category.Cables=[object Object]&Category.Cables=[object Object]        
response: Обновление полей кабеля. Перенаправляет на страницу с списком всех кабелей в админ панели
### POST /admin/cable/delete/{id}
Удаление кабеля по id
request: /admin/cable/delete/434f0e81-6d93-4f6c-ba83-ccfe55bb8dc9
response: Перенаправляет на страницу с списком всех кабелей в админ панели


## Cart
### GET /Cart
### GET /Cart/OrderSuccess
### POST /Cart/Add
### POST /Cart/Remove
### POST /Cart/Update
### POST /Cart/Checkout


## CartItemManage
### GET /admin/cartitem
### GET /admin/cartitem/{id}
### GET /admin/cartitem/add
### POST /admin/cartitem/add
### POST /admin/cartitem/update/{id}
### POST /admin/cartitem/delete/{id}
### POST /admin/cartitem/{id}/quantity
### POST /admin/cartitem/{id}/movetoorder/{orderId}


## CategoryManage
### GET /admin/category
### GET /admin/category
### GET /admin/addcategory
### POST /admin/addcategory
### POST /admin/category/update/{id}
### POST /admin/category/delete/{id}

## Home
### POST /logout
request: /logout        
reponse: перенаправление на домашнюю страницу, куки удалены, пользователь не авторизован в системе 
### GET /Menu
request: /Menu      
response: Получение страницы с каталогом всех кабелей. На странице присутствует выбор сортировки товара, фильтрация по категории
### GET /Menu/Category/{categoryId}
request: /Menu/Category/434f0e81-6d93-4f6c-ba83-ccfe55bb8dc9        
reponse: Отфильтрованная страница Menu по выбранной категории. Отображается только тот товар, который соответствует выбранной категории

## Login
### GET /login
Получение страницы для авторизации в системе 
### POST /login
request: curl --location '//login' \
--header 'Content-Type: multipart/form-data' \
--form 'email="test@gmail.com"' \
--form 'password="123123"'  
response: Домашняя страница, пользователь авторизирован в системе

## OrderManage
### GET /admin/order
request: /admin/order       
response: получение страницы админ панели со всеми заказами
### GET /admin/order/{id}
request: /admin/order/434f0e81-6d93-4f6c-ba83-ccfe55bb8dc9      
response: получение страницы с выбранным заказом

## Register
### GET /register
Получение страницы для регистрации пользователей
request: /register
response: Страница с полями для регистрации. Поля: email, password, FirstName, LastName, PhoneNumber
### POST /register
Принимает поля, вводимые пользователем при регистрации, после чего данные отправляются в сервис регистрации     
request: /register?Email=<email>&Password=<string>&FirstName=<string>&LastName=<string>&PhoneNumber=<string>    
response: Страница для входа в систему 

## UserManage
### GET /admin/manageuser
Получение страницы с списком всех пользователей 

