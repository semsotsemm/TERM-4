using Laboratory04_05.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratory04_05.Managers
{
    public class EfShopService
    {
        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            using (var ctx = new BitDbContext())
            {
                ctx.Products.Add(product);
                await ctx.SaveChangesAsync();
                return product;
            }
        }

        public async Task<ProductEntity> GetProductByIdAsync(int id)
        {
            using (var ctx = new BitDbContext())
            {
                return await ctx.Products.FindAsync(id);
            }
        }

        public async Task<List<ProductEntity>> GetAllProductsAsync()
        {
            using (var ctx = new BitDbContext())
            {
                return await ctx.Products.ToListAsync();
            }
        }

        public async Task UpdateProductAsync(ProductEntity product)
        {
            using (var ctx = new BitDbContext())
            {
                ctx.Entry(product).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            using (var ctx = new BitDbContext())
            {
                var entity = await ctx.Products.FindAsync(id);
                if (entity != null)
                {
                    ctx.Products.Remove(entity);
                    await ctx.SaveChangesAsync();
                }
            }
        }


        public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
        {
            using (var ctx = new BitDbContext())
            {
                ctx.Customers.Add(customer);
                await ctx.SaveChangesAsync();
                return customer;
            }
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            using (var ctx = new BitDbContext())
            {
                return await ctx.Customers
                    .Include(c => c.BasketItems.Select(b => b.Product))
                    .ToListAsync();
            }
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            using (var ctx = new BitDbContext())
            {
                ctx.Entry(customer).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using (var ctx = new BitDbContext())
            {
                var entity = await ctx.Customers.FindAsync(id);
                if (entity != null)
                {
                    ctx.Customers.Remove(entity);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public async Task<List<ProductEntity>> GetProductsFilteredAsync(
            string nameContains = null,
            string country = null,
            int? minPrice = null,
            int? maxPrice = null,
            string color = null,
            bool sortByPriceAsc = true)
        {
            using (var ctx = new BitDbContext())
            {
                IQueryable<ProductEntity> query = ctx.Products;

                if (!string.IsNullOrWhiteSpace(nameContains))
                {
                    query = query.Where(p => p.Name.Contains(nameContains));
                }

                if (!string.IsNullOrWhiteSpace(country))
                {
                    query = query.Where(p => p.ProductionCountry == country);
                }

                if (!string.IsNullOrWhiteSpace(color))
                {
                    query = query.Where(p => p.Color == color);
                }

                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                query = sortByPriceAsc
                    ? query.OrderBy(p => p.Price).ThenByDescending(p => p.Rating)
                    : query.OrderByDescending(p => p.Price).ThenByDescending(p => p.Rating);

                return await query.ToListAsync();
            }
        }

        public async Task<List<ProductEntity>> SearchProductsAsync(
            string name = null,
            string color = null,
            string country = null,
            string size = null)
        {
            using (var ctx = new BitDbContext())
            {
                IQueryable<ProductEntity> query = ctx.Products;

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(p => p.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(color))
                    query = query.Where(p => p.Color == color);

                if (!string.IsNullOrWhiteSpace(country))
                    query = query.Where(p => p.ProductionCountry == country);

                if (!string.IsNullOrWhiteSpace(size))
                    query = query.Where(p => p.Size == size);

                return await query.ToListAsync();
            }
        }

        public async Task<bool> AddProductToBasketTransactionalAsync(int customerId, int productId, int quantity)
        {
            using (var ctx = new BitDbContext())
            using (var tx = ctx.Database.BeginTransaction())
            {
                try
                {
                    var product = await ctx.Products.SingleOrDefaultAsync(p => p.Id == productId);
                    if (product == null || product.Quantity < quantity)
                        return false;

                    product.Quantity -= quantity;

                    var basketItem = new BasketItemEntity
                    {
                        CustomerId = customerId,
                        ProductId = productId,
                        Quantity = quantity
                    };

                    ctx.Baskets.Add(basketItem);

                    await ctx.SaveChangesAsync();
                    tx.Commit();
                    return true;
                }
                catch (Exception)
                {
                    tx.Rollback();
                    return false;
                }
            }
        }
    }
}

