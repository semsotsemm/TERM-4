using System;
using System.Collections.Generic;
using static DAL_Celebrity.DAL_Celebrity_MSSQL;

namespace DAL_Celebrity;

internal class Program
{
    private static void Main(string[] args)
    {
        // 1. Строка подключения
        string CS = @"Server=(localdb)\mssqllocaldb;Database=CelebrityDB;Trusted_Connection=True;TrustServerCertificate=True;";
        // 2. Инициализация БД (создание таблиц и начальных данных)
        Init init = new Init(CS);
        Init.Execute(delete: true, create: true);

        // 3. Лямбда-выражения для красивого вывода в консоль
        Func<Celebrity, string> printC = (c) => $"Id = {c.Id}, FullName = {c.FullName}, Nationality = {c.Nationality}, ReqPhotoPath = {c.ReqPhotoPath}";
        Func<Liveevent, string> printL = (l) => $"Id = {l.Id}, CelebrityId = {l.CelebrityId}, Date = {l.Date:d}, Description = {l.Description}, ReqPhotoPath = {l.ReqPhotoPath}";
        Func<string, string> puri = (string f) => $"{f}";

        // 4. Тестирование Репозитория
        using (IRepository repo = new Repository(CS))
        {
            // --- GetAllCelebrities ---
            Console.WriteLine("\n------ GetAllCelebrities() ----------");
            repo.GetAllCelebrities().ForEach(celebrity => Console.WriteLine(printC(celebrity)));

            // --- GetAllLiveevents ---
            Console.WriteLine("\n------ GetAllLiveevents() -----------");
            repo.GetAllLiveevents().ForEach(life => Console.WriteLine(printL(life)));

            // --- AddCelebrity (Albert Einstein) ---
            Console.WriteLine("\n------ AddCelebrity() ---------------");
            Celebrity c1 = new Celebrity { FullName = "Albert Einstein", Nationality = "DE", ReqPhotoPath = puri("Einstein.jpg") };
            if (repo.AddCelebrity(c1)) Console.WriteLine($"OK: AddCelebrity: {printC(c1)}");
            else Console.WriteLine($"ERROR: AddCelebrity: {printC(c1)}");

            // --- AddCelebrity (Samuel Huntington) ---
            Celebrity c2 = new Celebrity { FullName = "Samuel Huntington", Nationality = "US", ReqPhotoPath = puri("Huntington.jpg") };
            if (repo.AddCelebrity(c2)) Console.WriteLine($"OK: AddCelebrity: {printC(c2)}");
            else Console.WriteLine($"ERROR: AddCelebrity: {printC(c2)}");

            // --- DelCelebrity (Einstein) ---
            Console.WriteLine("\n------ DelCelebrity() ---------------");
            int idEinstein = repo.GetCelebrityIdByName("Albert Einstein");
            if (idEinstein > 0)
            {
                if (repo.DelCelebrity(idEinstein)) Console.WriteLine($"OK: DelCelebrity: ID {idEinstein}");
                else Console.WriteLine($"ERROR: DelCelebrity: ID {idEinstein}");
            }
            else Console.WriteLine("ERROR: GetCelebrityIdByName (Einstein)");

            // --- UpdCelebrity (Huntington) ---
            Console.WriteLine("\n------ UpdCelebrity() ---------------");
            int idHunt = repo.GetCelebrityIdByName("Samuel Huntington");
            if (idHunt > 0)
            {
                Celebrity? cToUpd = repo.GetCelebrityById(idHunt);
                if (cToUpd != null)
                {
                    cToUpd.Nationality = "USA"; // Меняем данные
                    if (repo.UpdCelebrity(idHunt, cToUpd)) Console.WriteLine($"OK: UpdCelebrity: {printC(cToUpd)}");
                    else Console.WriteLine($"ERROR: UpdCelebrity: {idHunt}");
                }
                else Console.WriteLine($"ERROR: GetCelebrityById: {idHunt}");
            }
            else Console.WriteLine("ERROR: GetCelebrityIdByName (Huntington)");

            // --- AddLiveevent ---
            Console.WriteLine("\n------ AddLiveevent() ---------------");
            int idHuntEvent = repo.GetCelebrityIdByName("Samuel Huntington");
            if (idHuntEvent > 0)
            {
                Liveevent newEvent = new Liveevent { CelebrityId = idHuntEvent, Date = DateTime.Now, Description = "Тестовое событие" };
                if (repo.AddLiveevent(newEvent)) Console.WriteLine("OK: AddLiveevent");
                else Console.WriteLine("ERROR: AddLiveevent");
            }

            // --- GetLifeeventsByCelebrityId ---
            Console.WriteLine("\n------ GetLifeeventsByCelebrityId ---");
            int idHuntEvents = repo.GetCelebrityIdByName("Samuel Huntington");
            if (idHuntEvents > 0)
            {
                repo.GetLifeeventsByCelebrityId(idHuntEvents).ForEach(l =>
                    Console.WriteLine($"OK: GetLifeeventsByCelebrityId, {idHuntEvents}, {printL(l)}"));
            }

            // --- GetCelebrityByLiveeventId ---
            Console.WriteLine("\n------ GetCelebrityByLiveeventId ---");
            int eventId = 1; // Например, самое первое событие в БД
            Celebrity? foundC = repo.GetCelebrityByLiveeventId(eventId);
            if (foundC != null) Console.WriteLine($"OK: {printC(foundC)}");
            else Console.WriteLine($"ERROR: GetCelebrityByLiveeventId, {eventId}");
        }

        Console.WriteLine("\n--------------> Done. Press any key...");
        Console.ReadKey();
    }
}