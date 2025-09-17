# Онлайн магазин по продаже кабеля
<img width="1920" height="962" alt="image" src="https://github.com/user-attachments/assets/a07725f8-cfe2-420b-81fb-82069f8d3959" />

# Установка 
1. Склонируйте репозиторий `git clone https://github.com/amorqw/ShopPractice.git`
2. Создайте докер контейнер с помощью команды ` docker run --name Shop -e POSTGRES_USER=user -e POSTGRES_PASSWORD=user -p 5432:5432 -d postgres:latest`
3. Перейдите в директорию приложения `cd ShopPractice/web`
4. Установите зависимости и запустите приложение `dotnet restore` `dotnet run`
   
