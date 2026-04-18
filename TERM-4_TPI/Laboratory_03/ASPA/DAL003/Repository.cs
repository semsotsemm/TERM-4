using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DAL003
{
    public class Repository : IRepository
    {
        public static IRepository Create(string directoryName)
        {
            return new Repository(directoryName);
        }
        public static string JSONFileName { get; set; } = "Celebrities.json";

        public string BasePath { get; }

        private readonly string _jsonFilePath;

        public Repository(string directoryName)
        {
            BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName);

            _jsonFilePath = Path.Combine(BasePath, JSONFileName);
        }

        private Celebrity[] LoadData()
        {
            if (!File.Exists(_jsonFilePath))
            {
                return Array.Empty<Celebrity>();
            }

            string json = File.ReadAllText(_jsonFilePath);
            return JsonSerializer.Deserialize<Celebrity[]>(json) ?? Array.Empty<Celebrity>();
        }

        public Celebrity[] getAllCelebrities()
        {
            return LoadData();
        }

        public Celebrity? getCelebrityById(int id)
        {
            return LoadData().FirstOrDefault(c => c.Id == id);
        }

        public Celebrity[] getCelebritiesBySurname(string Surname)
        {
            return LoadData()
                .Where(c => c.Surname.Equals(Surname, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            var celebrity = getCelebrityById(id);
            return celebrity?.PhotoPath;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
       
    }
}