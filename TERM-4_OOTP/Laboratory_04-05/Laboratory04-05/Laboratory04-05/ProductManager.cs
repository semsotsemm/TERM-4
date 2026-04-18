using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;
using System.Linq;

namespace Laboratory04_05
{
    public class ProductManager
    {
        private string Filename = "products.json";
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
            public string ImagePath { get; set; }
        }

        public List<Product> getProductsList()
        {
            if (File.Exists(Filename))
            {
                string json_string = File.ReadAllText(Filename);
                return JsonSerializer.Deserialize<List<Product>>(json_string) ?? new List<Product>();
            }
            else
            {
                return new List<Product>();
            }
        }
        public bool addNewProduct(Product new_product)
        {
            List<Product> products = getProductsList();
            products.Add(new_product);

            string json_string = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Filename, json_string);
            return true;
        }
        public void DeteteProductByName(string product_name)
        {
            if (File.Exists(Filename))
            {
                string json_string = File.ReadAllText(Filename);
                List<Product> all_products = JsonSerializer.Deserialize<List<Product>>(json_string) ?? new List<Product>();
                all_products.RemoveAll(p => p.Name == product_name);
                json_string = JsonSerializer.Serialize(all_products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Filename, json_string);
            }
        }
        public void EditProduct(Product old_product, Product edited_product)
        {
            if (!File.Exists(Filename))
            {
                return;
            }

            string json_string = File.ReadAllText(Filename);
            List<Product> all_products = JsonSerializer.Deserialize<List<Product>>(json_string) ?? new List<Product>();

            int index = all_products.FindIndex(p => p.Name == old_product.Name);

            if (index != -1)
            {
                all_products[index] = edited_product;

                json_string = JsonSerializer.Serialize(all_products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Filename, json_string);
            }
        }
    }
}
