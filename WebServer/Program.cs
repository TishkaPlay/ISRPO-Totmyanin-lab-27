//Создание билдера
var builder = WebApplication.CreateBuilder(args);

//Сборка приложения
var app = builder.Build();

//Регистрация маршрута
app.MapGet("/", () => "Привет от ИСП-232! Автор: Тихон");

//Запуск
app.Run();