using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace Laboratory04_05
{
    public class DbManager
    {
        static private readonly string default_system_conn = @"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=5329965";
        static private readonly string db_name = "bitdatabase";
        static private readonly string default_db_conn = @"Host=localhost;Port=5432;Database=bitdatabase;Username=postgres;Password=5329965";

        static private readonly string conn_system_db_string;
        static private readonly string conn_db_string;

        static DbManager()
        {
            try
            {
                var sys = ConfigurationManager.ConnectionStrings["PostgresSystem"];
                conn_system_db_string = sys != null && !string.IsNullOrWhiteSpace(sys.ConnectionString)
                    ? sys.ConnectionString
                    : default_system_conn;

                var db = ConfigurationManager.ConnectionStrings["BitDatabaseRaw"];
                conn_db_string = db != null && !string.IsNullOrWhiteSpace(db.ConnectionString)
                    ? db.ConnectionString
                    : default_db_conn;
            }
            catch
            {
                conn_system_db_string = default_system_conn;
                conn_db_string = default_db_conn;
            }
        }




        public class User
        {
            public string username { get; set; }
            public string email { get; set; }
            public string phone_number { get; set; }
            public string password { get; set; }
            public List<string> basket { get; set; } = new List<string>();
        }
        public enum ColorPalette
        {
            Black,
            White,
            Red,
            Blue
        }
        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ShortDescription { get; set; }
            public decimal Rating { get; set; }
            public short Quantity { get; set; }
            public string Size { get; set; }
            public ColorPalette Color { get; set; }
            public string ProductionCountry { get; set; }
            public short Discount { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
            public byte[] Image { get; set; }
        }
        public static class Session
        {
            public static DbManager.User CurrentUser { get; set; }
        }




        private string HashingData(string data)
        {
            string available_symb = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            string hash = "";
            int hash_length = 64;
            long current_val = 0;

            for (int i = 0; i < data.Length; i++)
            {
                current_val += (i + 1) * data[i] * 73;
            }

            for (int i = 0; i < hash_length; i++)
            {
                current_val = (current_val * 27 + i);
                hash += available_symb[(int)(Math.Abs(current_val) % available_symb.Length)];
            }
            return hash;
        }

        public bool IsDatabaseExists()
        {
            using (var conn = new NpgsqlConnection(conn_system_db_string))
            {
                conn.Open();

                var checkCommand = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{db_name}'", conn);
                var exists = checkCommand.ExecuteScalar();
                return exists != null;

            }
        }

        private bool IsUserExists(string username)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = "SELECT EXISTS(SELECT 1 FROM customers WHERE username = @username)";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public void CreateDatabase()
        {
            if (!IsDatabaseExists())
            {
                using (var conn = new NpgsqlConnection(conn_system_db_string))
                {
                    conn.Open();
                    string sql = $"CREATE Database {db_name}";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();

                string sql = @"CREATE Table IF NOT EXISTS customers(
                                id SERIAL PRIMARY KEY,
                                username VARCHAR(30) NOT NULL,
                                email VARCHAR(50) NOT NULL,
                                phone_number VARCHAR(20) NOT NULL,
                                password VARCHAR(70) NOT NULL);

                               CREATE TABLE IF NOT EXISTS products (
                                id SERIAL PRIMARY KEY,
                                product_name VARCHAR(20) NOT NULL,
                                product_color VARCHAR(20) NOT NULL,
                                production_country VARCHAR(20) NOT NULL,
                                product_size VARCHAR(20) NOT NULL,
                                count INTEGER NOT NULL CHECK (count >= 0),
                                description VARCHAR(200) NOT NULL,
                                short_description VARCHAR(100) NOT NULL,
                                rating NUMERIC(2, 1) NOT NULL CHECK (rating <= 5),
                                price INTEGER NOT NULL CHECK (price >= 0),
                                discount SMALLINT NOT NULL CHECK (discount >= 0),
                                product_image BYTEA);
                               
                               CREATE Table IF NOT EXISTS baskets (
                                basket_id SERIAL primary key,               
                                quantity INTEGER NOT NULL,
                                customer_id INTEGER REFERENCES customers(id) ON DELETE CASCADE,
                                product_id INTEGER REFERENCES products(id) ON DELETE CASCADE);

                               CREATE Table IF NOT EXISTS admins(
                                id SERIAL PRIMARY KEY,
                                admin_name VARCHAR(30) NOT NULL,
                                password VARCHAR(70) NOT NULL);";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<Product> getProductsList()
        {
            List<Product> products = new List<Product>();
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();

                string sql = "SELECT product_name, product_color, production_country, product_size, count, description, short_description, rating, price, discount, product_image FROM products";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Name = reader.GetString(0),
                                Color = (ColorPalette)Enum.Parse(typeof(ColorPalette), reader.GetString(1), true),
                                ProductionCountry = reader.GetString(2),
                                Size = reader.GetString(3),
                                Quantity = (short)reader.GetInt32(4),
                                Description = reader.GetString(5),
                                ShortDescription = reader.GetString(6),
                                Rating = reader.GetDecimal(7),
                                Price = reader.GetInt32(8),
                                Discount = reader.GetInt16(9),
                                Image = reader.IsDBNull(10) ? null : (byte[])reader[10],
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public bool addNewProduct(Product new_product)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"INSERT INTO products(product_name, price, product_color, production_country, product_size, count, description, short_description, rating, discount, product_image)
                                            Values(@name,@price, @color, @country, @size, @quantity, @descr, @short_descr, @rat, @disc, @image)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", new_product.Name);
                    cmd.Parameters.AddWithValue("price", new_product.Price);
                    cmd.Parameters.AddWithValue("color", new_product.Color);
                    cmd.Parameters.AddWithValue("country", new_product.ProductionCountry);
                    cmd.Parameters.AddWithValue("size", new_product.Size);
                    cmd.Parameters.AddWithValue("size", new_product.Size);
                    cmd.Parameters.AddWithValue("quantity", new_product.Quantity);
                    cmd.Parameters.AddWithValue("descr", new_product.Description);
                    cmd.Parameters.AddWithValue("short_descr", new_product.ShortDescription);
                    cmd.Parameters.AddWithValue("disc", new_product.Discount);
                    cmd.Parameters.AddWithValue("rat", new_product.Rating);
                    cmd.Parameters.AddWithValue("image", new_product.Image);

                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public string RegistrateNewUser(User new_user)
        {
            string registrate_state = "Ok";

            string phone_pattern = @"^(\+375|80)(44|33|25|29)\d{7}$";
            string email_pattern = @"^[a-zA-Z0-9._]{5,40}@(gmail\.com|yandex\.ru|mail\.ru)$";
            string username_pattern = @"^[a-zA-Zа-яА-Я0-9ё._]{5,30}$";
            string user_password = @"^[a-zA-Z0-9а-яА-ЯёЁ._!@#$%^&*]{8,30}$";

            if (!Regex.IsMatch(new_user.phone_number, phone_pattern))
            {
                return "Неверный формат номера телефона";
            }

            if (!Regex.IsMatch(new_user.email, email_pattern))
            {
                return "Неверный формат электронной почты";
            }

            if (!Regex.IsMatch(new_user.username, username_pattern))
            {
                return "Проверьте имя пользователя";
            }
            if (!Regex.IsMatch(new_user.password, user_password))
            {
                return "Пароль слишком легкий";
            }
            if (IsUserExists(new_user.username))
            {
                return "Пользователь с таким именем уже зарегестрирован в системе, пожалуйста, выберите другое имя пользователя";
            }

            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"INSERT INTO customers(username, email, phone_number, password)
                                            Values(@user_username, @user_email, @user_phone, @user_password)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("user_username", new_user.username);
                    cmd.Parameters.AddWithValue("user_email", new_user.email);
                    cmd.Parameters.AddWithValue("user_phone", new_user.phone_number);
                    cmd.Parameters.AddWithValue("user_password", HashingData(new_user.password));

                    cmd.ExecuteNonQuery();
                }
            }

            return registrate_state;
        }

        public bool TryLogin(User user_data)
        {
            if (!IsUserExists(user_data.username))
            {
                return false;
            }
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();

                string sql = @"SELECT password FROM customers WHERE username = @username";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", user_data.username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbPassword = reader.GetString(0);
                            return dbPassword == HashingData(user_data.password);
                        }
                        return false;
                    }
                }
            }
        }

        private async Task<bool> IsUserExistsAsync(string username)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                await conn.OpenAsync();
                string sql = "SELECT EXISTS(SELECT 1 FROM customers WHERE username = @username)";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    return (bool)await cmd.ExecuteScalarAsync();
                }
            }
        }

        public bool TryAdminLogin(string admin_name, string password)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();

                string sql = @"SELECT password FROM admins WHERE admin_name = @adminname";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("adminname", admin_name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbPassword = reader.GetString(0);
                            return dbPassword == HashingData(password);
                        }
                        return false;
                    }
                }
            }
        }

        public void changeUser(User old_user, User new_user)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"UPDATE customers 
                               SET username = @new_username, 
                                   email = @new_email, 
                                   phone_number = @new_phone, 
                                   password = @new_password 
                               WHERE username = @user_name";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("new_username", new_user.username);
                    cmd.Parameters.AddWithValue("new_email", new_user.email);
                    cmd.Parameters.AddWithValue("new_phone", new_user.phone_number);
                    cmd.Parameters.AddWithValue("new_password", HashingData(new_user.password));
                    cmd.Parameters.AddWithValue("user_name", old_user.username);

                    cmd.ExecuteNonQuery();
                }
            }

            Session.CurrentUser = new_user;
        }

        public void UpdateUser(User updatedUser)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"UPDATE customers 
                               SET email = @new_email, 
                                   phone_number = @new_phone, 
                                   password = @new_password 
                               WHERE username = @user_name";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("new_email", updatedUser.email);
                    cmd.Parameters.AddWithValue("new_phone", updatedUser.phone_number);
                    cmd.Parameters.AddWithValue("new_password", HashingData(updatedUser.password));
                    cmd.Parameters.AddWithValue("user_name", updatedUser.username);

                    cmd.ExecuteNonQuery();
                }
            }
            Session.CurrentUser = updatedUser;
        }

        public void DeteteProductByName(string product_name)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"DELETE FROM products WHERE product_name = @product_name";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("product_name", product_name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetCustomerId(User customer)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"SELECT id FROM customers WHERE username = @name";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", customer.username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
        }

        private int GetProductId(Product product)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"SELECT id FROM products WHERE product_name = @name";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", product.Name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        return 0;
                    }
                }
            }
        }

        public void AddProductToBasket(User customer, Product product)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();

                string sql = @"INSERT INTO baskets(quantity, customer_id, product_id)
                                            Values(@product_quantity, @customer, @product)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("product_quantity", 1);
                    cmd.Parameters.AddWithValue("customer", GetCustomerId(customer));
                    cmd.Parameters.AddWithValue("product", GetProductId(product));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveProductFromBasket(User customer, Product product)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = "DELETE FROM baskets WHERE customer_id = @customer_id AND product_id = @product_id";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("customer_id", GetCustomerId(customer));
                    cmd.Parameters.AddWithValue("product_id", GetProductId(product));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EditProduct(Product old_product, Product edited_product)
        {
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"UPDATE products SET 
                        product_name = @new_name, 
                        product_color = @new_color, 
                        production_country = @new_country, 
                        product_size = @new_size, 
                        count = @new_count, 
                        description = @new_description, 
                        short_description = @new_short_description, 
                        rating = @new_rating, 
                        price = @new_price,
                        discount = @new_discount,  
                        product_image = @new_image
                      WHERE product_name = @old_name";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("new_name", edited_product.Name);
                    cmd.Parameters.AddWithValue("new_color", edited_product.Color.ToString());
                    cmd.Parameters.AddWithValue("new_country", edited_product.ProductionCountry);
                    cmd.Parameters.AddWithValue("new_size", edited_product.Size);
                    cmd.Parameters.AddWithValue("new_count", (int)edited_product.Quantity);
                    cmd.Parameters.AddWithValue("new_description", edited_product.Description);
                    cmd.Parameters.AddWithValue("new_short_description", edited_product.ShortDescription);
                    cmd.Parameters.AddWithValue("new_rating", edited_product.Rating);
                    cmd.Parameters.AddWithValue("new_price", (int)edited_product.Price); 
                    cmd.Parameters.AddWithValue("new_discount", (short)edited_product.Discount); 

                    cmd.Parameters.AddWithValue("new_image", (object)edited_product.Image ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("old_name", old_product.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Product> GetUserBasket(User customer)
        {
            List<Product> products = new List<Product>();
            using (var conn = new NpgsqlConnection(conn_db_string))
            {
                conn.Open();
                string sql = @"SELECT p.product_name, p.price, p.product_image
                       FROM baskets b
                       JOIN products p ON b.product_id = p.id
                       WHERE b.customer_id = @id";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", GetCustomerId(customer));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Name = reader.GetString(0),
                                Price = reader.GetInt32(1),
                                Image = reader.IsDBNull(2) ? null : (byte[])reader[2]
                            });
                        }
                    }
                }
            }
            return products;
        }
    }
}