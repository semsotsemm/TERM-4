public partial class Program
{
    private static void Main(string[] args)
    {
        // Создаем объект builder, который соберет все насйтроки воедино проверяем конф. файл и порты args например для задания порта в ручную
        var builder = WebApplication.CreateBuilder(args);

        // Используем логирование http запросов
        builder.Services.AddHttpLogging(logging =>
        {
            // Здесь можно настроить, ЧТО именно логировать
            logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
        });

        // Закрываем процесс настройки, получаем готовый объект приложения app 
        var app = builder.Build();

        // Регистрируем маршрут для http запросов — главная страница сайта (/), если кто-то перейдет на / выполнить лямба функцию, которая выведет Hello world.
        app.MapGet("/", () => "Мое первое ASPA");

        // Поднимаем Kestrel, слушаем входящие запросы
        app.Run();
    }
}