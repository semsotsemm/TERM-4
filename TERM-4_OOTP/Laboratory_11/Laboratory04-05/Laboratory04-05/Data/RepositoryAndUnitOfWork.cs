using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratory04_05.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> Query();

        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BitDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(BitDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<TEntity>();
        }

        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            return DbSet.FindAsync(id);
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return DbSet.ToListAsync();
        }

        public virtual IQueryable<TEntity> Query()
        {
            return DbSet;
        }

        public virtual Task AddAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            DbSet.Add(entity);
            return Task.CompletedTask;
        }

        public virtual void Remove(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            DbSet.Remove(entity);
        }
    }

    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<List<ProductEntity>> GetFilteredAsync(
            string nameContains = null,
            string country = null,
            int? minPrice = null,
            int? maxPrice = null,
            string color = null,
            bool sortByPriceAsc = true);

        Task<List<ProductEntity>> SearchAsync(
            string name = null,
            string color = null,
            string country = null,
            string size = null);
    }

    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        public ProductRepository(BitDbContext context) : base(context)
        {
        }

        public Task<List<ProductEntity>> GetFilteredAsync(
            string nameContains = null,
            string country = null,
            int? minPrice = null,
            int? maxPrice = null,
            string color = null,
            bool sortByPriceAsc = true)
        {
            IQueryable<ProductEntity> query = DbSet;

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

            return query.ToListAsync();
        }

        public Task<List<ProductEntity>> SearchAsync(
            string name = null,
            string color = null,
            string country = null,
            string size = null)
        {
            IQueryable<ProductEntity> query = DbSet;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(color))
                query = query.Where(p => p.Color == color);

            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(p => p.ProductionCountry == country);

            if (!string.IsNullOrWhiteSpace(size))
                query = query.Where(p => p.Size == size);

            return query.ToListAsync();
        }
    }

    public interface ICustomerRepository : IRepository<CustomerEntity>
    {
        Task<List<CustomerEntity>> GetAllWithBasketAsync();
    }

    public class CustomerRepository : Repository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(BitDbContext context) : base(context)
        {
        }

        public Task<List<CustomerEntity>> GetAllWithBasketAsync()
        {
            return DbSet
                .Include(c => c.BasketItems.Select(b => b.Product))
                .ToListAsync();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        IRepository<BasketItemEntity> Baskets { get; }
        IRepository<AdminEntity> Admins { get; }

        int Commit();
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BitDbContext _context;

        private IProductRepository _products;
        private ICustomerRepository _customers;
        private IRepository<BasketItemEntity> _baskets;
        private IRepository<AdminEntity> _admins;

        private bool _disposed;

        public UnitOfWork(BitDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IProductRepository Products =>
            _products ?? (_products = new ProductRepository(_context));

        public ICustomerRepository Customers =>
            _customers ?? (_customers = new CustomerRepository(_context));

        public IRepository<BasketItemEntity> Baskets =>
            _baskets ?? (_baskets = new Repository<BasketItemEntity>(_context));

        public IRepository<AdminEntity> Admins =>
            _admins ?? (_admins = new Repository<AdminEntity>(_context));

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

