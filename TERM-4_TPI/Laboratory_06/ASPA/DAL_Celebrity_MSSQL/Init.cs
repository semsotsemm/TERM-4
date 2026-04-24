using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static DAL_Celebrity.DAL_Celebrity_MSSQL;

namespace DAL_Celebrity
{
    public class Init
    {
        static string connstring = @"Data source=172.16.193.88; Initial Catalog=LES01;" +
                                    @"TrustServerCertificate=True;User Id=smw60;Password=21625;";

        public Init() { }
        public Init(string conn) { connstring = conn; }

        public static void Execute(bool delete = true, bool create = true)
        {
            using (Context context = new Context(connstring))
            {
                if (delete) context.Database.EnsureDeleted();
                if (create) context.Database.EnsureCreated();

                Func<string, string> puri = (string f) => $"{f}";

                var celebrities = new List<Celebrity>
                {
                    new Celebrity { FullName = "Noam Chomsky", Nationality = "US", ReqPhotoPath = puri("Chomsky.jpg") },
                    new Celebrity { FullName = "Tim Berners-Lee", Nationality = "UK", ReqPhotoPath = puri("Berners-Lee.jpg") },
                    new Celebrity { FullName = "Edgar Codd", Nationality = "US", ReqPhotoPath = puri("Codd.jpg") },
                    new Celebrity { FullName = "Donald Knuth", Nationality = "US", ReqPhotoPath = puri("Knuth.jpg") },
                    new Celebrity { FullName = "Linus Torvalds", Nationality = "US", ReqPhotoPath = puri("Linus.jpg") },
                    new Celebrity { FullName = "John Neumann", Nationality = "US", ReqPhotoPath = puri("Neumann.jpg") },
                    new Celebrity { FullName = "Edsger Dijkstra", Nationality = "NL", ReqPhotoPath = puri("Dijkstra.jpg") },
                    new Celebrity { FullName = "Ada Lovelace", Nationality = "UK", ReqPhotoPath = puri("Lovelace.jpg") },
                    new Celebrity { FullName = "Charles Babbage", Nationality = "UK", ReqPhotoPath = puri("Babbage.jpg") },
                    new Celebrity { FullName = "Andrew Tanenbaum", Nationality = "NL", ReqPhotoPath = puri("Tanenbaum.jpg") }
                };

                context.Celebrities.AddRange(celebrities);
                context.SaveChanges();

                var events = new List<Liveevent>
                {
                    new Liveevent { CelebrityId = 1, Date = new DateTime(1928, 12, 7), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 1, Date = new DateTime(1955, 1, 1), Description = "Издание книги \"Логическая структура лингвистической теории\"" },

                    new Liveevent { CelebrityId = 2, Date = new DateTime(1955, 6, 8), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 2, Date = new DateTime(1989, 6, 8), Description = "В CERN предложил \"Гипертекстовый проект\"" },

                    new Liveevent { CelebrityId = 3, Date = new DateTime(1923, 8, 23), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 3, Date = new DateTime(2003, 4, 18), Description = "Дата смерти" },

                    new Liveevent { CelebrityId = 4, Date = new DateTime(1938, 1, 10), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 4, Date = new DateTime(1974, 1, 1), Description = "Премия Тьюринга" },

                    new Liveevent { CelebrityId = 5, Date = new DateTime(1969, 12, 28), Description = "Дата рождения. Финляндия." },
                    new Liveevent { CelebrityId = 5, Date = new DateTime(1991, 9, 17), Description = "Выложил исходный код OS Linux (версии 0.01)" },

                    new Liveevent { CelebrityId = 6, Date = new DateTime(1903, 12, 28), Description = "Дата рождения. Венгрия." },
                    new Liveevent { CelebrityId = 6, Date = new DateTime(1957, 2, 8), Description = "Дата смерти" },

                    new Liveevent { CelebrityId = 7, Date = new DateTime(1930, 12, 28), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 7, Date = new DateTime(2002, 8, 6), Description = "Дата смерти" },

                    new Liveevent { CelebrityId = 8, Date = new DateTime(1815, 12, 10), Description = "Дата рождения" }, 
                    new Liveevent { CelebrityId = 8, Date = new DateTime(1852, 11, 27), Description = "Дата смерти" },

                    new Liveevent { CelebrityId = 9, Date = new DateTime(1791, 12, 26), Description = "Дата рождения" },
                    new Liveevent { CelebrityId = 9, Date = new DateTime(1871, 10, 18), Description = "Дата смерти" },

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