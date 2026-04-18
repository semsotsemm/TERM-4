using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace Laboratory04_05
{
    public class UserManager
    {
        private const string FilePath = "users.json";

        public class User
        {
            public string user_name { get; set; }
            public string email { get; set; }
            public string phone_number { get; set; }
            public string password { get; set; }
            public List<string> basket { get; set; } = new List<string>();
        }

        public List<User> getAllUsers() 
        {
            List<User> users = new List<User>();
            if (File.Exists(FilePath))
            {
                string json_string = File.ReadAllText(FilePath);
                users = JsonSerializer.Deserialize<List<User>>(json_string);
            }
            return users;
        }
        public string registerNewUser(User new_user)
        {
            string register_status = "Ok";
            if (!(new_user.email.Contains("@gmail.com") || new_user.email.Contains("@mail.ru")))
            {
                register_status = "Неверный формат почты";
                return register_status;
            }
            if (!(new_user.phone_number.Contains("+")) || !(new_user.phone_number.Count() == 13))
            {
                register_status = "Неверный формат номера";
                return register_status;
            }
            List<User> users = getAllUsers();
            
            if (users.Find(p => p.user_name == new_user.user_name) != null) 
            {
                MessageBox.Show("Данный пользователь уже зарегистрирован в системе");
            }
            users.Add(new_user);
            string json_string = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json_string);
            return register_status;
        }
        public void changeUser(User old_user, User new_user)
        {
            List<User> users = getAllUsers();
            List<User> filtered_user = users.FindAll(p => p.user_name != old_user.user_name);
            filtered_user.Add(new_user);
            Session.CurrentUser = new_user;
            string json_string = JsonSerializer.Serialize(filtered_user, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json_string);
        }
        public bool TrySingUser(User user)
        {
            List<User> users = getAllUsers();
            User checking_user = users.Find(p => p.email == user.email);
            if (checking_user == null)
            {
                checking_user = users.Find(p => p.user_name == user.user_name);
            }
            if (checking_user == null) 
            {
                return false;
            }
            return checking_user.password == user.password;
        }
        public void UpdateUser(User updatedUser)
        {
            List<User> users = getAllUsers();
            int index = users.FindIndex(u => u.user_name == updatedUser.user_name);
            if (index != -1)
            {
                users[index] = updatedUser;
                string json_string = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json_string);
            }
        }
    }
    public static class Session
    {
        public static UserManager.User CurrentUser { get; set; }
    }
}