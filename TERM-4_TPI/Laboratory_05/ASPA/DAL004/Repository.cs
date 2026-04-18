using System.Text.Json;
using System.Text.Json.Serialization;

namespace DAL004
{
    public interface Repository : IDisposable
    {
        string base_path { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebryById(int id);
        Celebrity[] getCelebritiesBySurname(string surname);
        string? getPhotoPathById(int id);
        int? addCelebrity(Celebrity celebrity);
        bool delCelebityById(int id);
        int updCelebityById(int id, Celebrity celebrity);
        int saveChange();
    }

    public class Celebrity
    {
        [JsonPropertyName("Id")]
        public int celebrity_id { get; set; }

        [JsonPropertyName("Firstname")]
        public string firstname { get; set; } = string.Empty;

        [JsonPropertyName("Surname")]
        public string surname { get; set; } = string.Empty;

        [JsonPropertyName("PhotoPath")]
        public string photo_path { get; set; } = string.Empty;
    }

    public class JsonRepository : Repository
    {
        private readonly string directory_path;
        private List<Celebrity> celebrities;

        public string base_path => directory_path;
        public static string json_filename { get; set; } = "Celebrities.json";

        public JsonRepository(string directoryPath)
        {
            directory_path = directoryPath;
            string fullpath = Path.Combine(directory_path, json_filename);

            if (File.Exists(fullpath))
            {
                string json_string = File.ReadAllText(fullpath);
                celebrities = JsonSerializer.Deserialize<List<Celebrity>>(json_string) ?? new List<Celebrity>();
            }
            else
            {
                celebrities = new List<Celebrity>();
            }
        }

        public Celebrity[] getAllCelebrities() => celebrities.ToArray();

        public Celebrity? getCelebryById(int id)
        {
            return celebrities.FirstOrDefault(p => p.celebrity_id == id);
        }

        public Celebrity[] getCelebritiesBySurname(string surname)
        {
            return celebrities.Where(p => p.surname == surname).ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            return celebrities.FirstOrDefault(p => p.celebrity_id == id)?.photo_path;
        }

        public int? addCelebrity(Celebrity celebrity)
        {
            int nextId = celebrities.Count > 0 ? celebrities.Max(c => c.celebrity_id) + 1 : 1;
            celebrity.celebrity_id = nextId;
            celebrities.Add(celebrity);

            saveChange();
            return nextId;
        }

        public bool delCelebityById(int id)
        {
            var target = getCelebryById(id);
            if (target == null) return false;

            celebrities.Remove(target);
            saveChange();
            return true;
        }

        public int updCelebityById(int id, Celebrity celebrity)
        {
            var index = celebrities.FindIndex(c => c.celebrity_id == id);
            if (index == -1) return 0;

            celebrity.celebrity_id = id;
            celebrities[index] = celebrity;

            saveChange();
            return 1;
        }

        public int saveChange()
        {
            try
            {
                string fullpath = Path.Combine(directory_path, json_filename);

                int old_count = 0;
                if (File.Exists(fullpath))
                {
                    string old_json = File.ReadAllText(fullpath);
                    old_count = JsonSerializer.Deserialize<List<Celebrity>>(old_json)?.Count ?? 0;
                }

                string json_string = JsonSerializer.Serialize(celebrities, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fullpath, json_string);

                return Math.Abs(celebrities.Count - old_count);
            }
            catch
            {
                return 0;
            }
        }

        public void Dispose()
        {
            celebrities.Clear();
        }
    }
}