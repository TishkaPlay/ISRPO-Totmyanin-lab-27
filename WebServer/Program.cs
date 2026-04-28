//Создание билдера
var builder = WebApplication.CreateBuilder(args);

//Сборка приложения
var app = builder.Build();

//Middleware проверка ключа
app.Use(async (context, next) =>
{
    //Получаем параметр "key" из строки запроса
    var key = context.Request.Query["key"];

    if (key != "secret")
    {
        //Ключ отсутствует или неверный → 401 Unauthorized
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync(
            "Похоже, на этом сайте есть проблема\n" +
            "Код ошибки: 401 Unauthorized\n" +
            "Проверьте, правильно ли вы ввели адрес веб-сайта."
        );
        return; //прерываем цепочку middleware
    }

    //Ключ верный → передаём управление дальше
    await next(context);
});

//Логирование запросов
app.Use(async (context, next) => {
   Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path}");
   await next(context);
   Console.WriteLine($"[LOG] Ответ отправлен: {context.Response.StatusCode}"); 
});

//Добавляем заголовок в ответ
app.Use(async (context, next) => {
    context.Response.Headers.Append("X-Powered-By", "ASP.NET Core lab27");
    await next(context);
});

//Регистрация маршрута
app.MapGet("/", () => "Добро пожаловать на сервер!");

app.MapGet("/about", () => "Это мой первый ASP.NET Core сервер");

app.MapGet("/time", () => $"Время на сервере: {DateTime.Now}");

app.MapGet("/hello/{name}", (string name) => $"Привет, {name}!");

app.MapGet("/sum/{a}/{b}", (int a, int b) => $"{a} + {b} = {a + b}");

app.MapGet("/student", () => new {
   Name = "тихон Тотьмянин",
   Group = "ИСП-232",
   Year = 3,
   IsActive = true 
});

app.MapGet("/subjects", () => new[] {
    "РПМ",
    "РМП",
    "ИСРПО",
    "СП",
});

app.MapGet("/product/{id}", (int id) => new Product(
    Id: id,
    Name: $"Товар #{id}",
    Price: id * 99.99m,
    InStock: id % 2 == 0
));
//Запуск
app.Run();

record Product(int Id, string Name, decimal Price, bool InStock);