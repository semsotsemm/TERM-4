using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static DAL_Celebrity.DAL_Celebrity_MSSQL;

namespace DAL_Celebrity
{
    public class Init
    {
        // Используем вашу строку подключения
        static string connstring = @"Data source=172.16.193.88; Initial Catalog=LES01;" +
                                    @"TrustServerCertificate=True;User Id=smw60;Password=21625;";

        public Init() { }
        public Init(string conn) { connstring = conn; }

        public static void Execute(bool delete = true, bool create = true)
        {
            using (Context context = new Context(connstring))
            {
                // Пересоздание базы данных
                if (delete) context.Database.EnsureDeleted();
                if (create) context.Database.EnsureCreated();

                // Хелпер для путей к фото
                Func<string, string> puri = (string f) => $"{f}";

                // Список данных для добавления
                var celebrities = new List<Celebrity>
                {
                    // 1. Noam Chomsky
                    new Celebrity { FullName = "Noam Chomsky", Nationality = "US", ReqPhotoPath = puri("Chomsky.jpg") },
                    // 2. Tim Berners-Lee
                    new Celebrity { FullName = "Tim Berners-Lee", Nationality = "UK", ReqPhotoPath = puri("Berners-Lee.jpg") },
                    // 3. Edgar Codd
                    new Celebrity { FullName = "Edgar Codd", Nationality = "US", ReqPhotoPath = puri("Codd.jpg") },
                    // 4. Donald Knuth
                    new Celebrity { FullName = "Donald Knuth", Nationality = "US", ReqPhotoPath = puri("Knuth.jpg") },
                    // 5. Linus Torvalds
                    new Celebrity { FullName = "Linus Torvalds", Nationality = "US", ReqPhotoPath = puri("Linus.jpg") },
                    // 6. John Neumann
                    new Celebrity { FullName = "John Neumann", Nationality = "US", ReqPhotoPath = puri("Neumann.jpg") },
                    // 7. Edsger Dijkstra
                    new Celebrity { FullName = "Edsger Dijkstra", Nationality = "NL", ReqPhotoPath = puri("Dijkstra.jpg") },
                    // 8. Ada Lovelace
                    new Celebrity { FullName = "Ada Lovelace", Nationality = "UK", ReqPhotoPath = puri("Lovelace.jpg") },
                    // 9. Charles Babbage
                    new Celebrity { FullName = "Charles Babbage", Nationality = "UK", ReqPhotoPath = puri("Babbage.jpg") },
                    // 10. Andrew Tanenbaum
                    new Celebrity { FullName = "Andrew Tanenbaum", Nationality = "NL", ReqPhotoPath = puri("Tanenbaum.jpg") }
                };

                // Добавляем всех Celebrity
                context.Celebrities.AddRange(celebrities);
                context.SaveChanges(); // Сохраняем, чтобы получить ID

                // Добавляем события (Liveevents)
                var events = new List<Liveevent>
                {
                    // Noam Chomsky
                    new Liveevent { CelebrityId = 1, Date = new DateTime(1928, 12, 7), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 1, Date = new DateTime(1955, 1, 1), Description = "Издание книги \"Логическая структура лингвистической теории\"" },

                    // Tim Berners-Lee
                    new Liveevent { CelebrityId = 2, Date = new DateTime(1955, 6, 8), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 2, Date = new DateTime(1989, 6, 8), Description = "В CERN предложил \"Гипертекстовый проект\"" },

                    // Edgar Codd
                    new Liveevent { CelebrityId = 3, Date = new DateTime(1923, 8, 23), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 3, Date = new DateTime(2003, 4, 18), Description = "Дата смерти" },

                    // Donald Knuth
                    new Liveevent { CelebrityId = 4, Date = new DateTime(1938, 1, 10), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 4, Date = new DateTime(1974, 1, 1), Description = "Премия Тьюринга" },

                    // Linus Torvalds
                    new Liveevent { CelebrityId = 5, Date = new DateTime(1969, 12, 28), Description = "Дата рождения. Финляндия." },
                    new Liveevent { CelebrityId = 5, Date = new DateTime(1991, 9, 17), Description = "Выложил исходный код OS Linux (версии 0.01)" },

                    // John Neumann
                    new Liveevent { CelebrityId = 6, Date = new DateTime(1903, 12, 28), Description = "Дата рождения. Венгрия." },
                    new Liveevent { CelebrityId = 6, Date = new DateTime(1957, 2, 8), Description = "Дата смерти" },

                    // Edsger Dijkstra
                    new Liveevent { CelebrityId = 7, Date = new DateTime(1930, 12, 28), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 7, Date = new DateTime(2002, 8, 6), Description = "Дата смерти" },

                    // Ada Lovelace
                    new Liveevent { CelebrityId = 8, Date = new DateTime(1815, 12, 10), Description = "Дата рождения" }, // Исправил опечатку года из скрина (там было 1852 на рождение)
                    new Liveevent { CelebrityId = 8, Date = new DateTime(1852, 11, 27), Description = "Дата смерти" },

                    // Charles Babbage
                    new Liveevent { CelebrityId = 9, Date = new DateTime(1791, 12, 26), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 9, Date = new DateTime(1871, 10, 18), Description = "Дата смерти" },

                    // Andrew Tanenbaum
                    new Liveevent { CelebrityId = 10, Date = new DateTime(1944, 3, 16), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 10, Date = new DateTime(1987, 1, 1), Description = "Создал OS MINIX — бесплатную Unix-подобную систему" }
                };

                context.Liveevents.AddRange(events);
                context.SaveChanges();

                Console.WriteLine("Тестовые данные успешно загружены.");
            }
        }
    }
}