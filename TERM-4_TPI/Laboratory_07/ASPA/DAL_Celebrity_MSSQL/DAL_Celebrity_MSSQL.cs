using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DAL_Celebrity;

public class CelebritiesConfig
{
    public string PhotoFolder { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}

public class DAL_Celebrity_MSSQL
{
    public interface IRepository : DAL_Celebrity.IRepository<Celebrity, Liveevent> { }
    public class Celebrity
    {
        public Celebrity() { this.FullName = string.Empty; this.Nationality = string.Empty; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string? ReqPhotoPath { get; set; }
        public virtual bool Update(Celebrity celebrity)
        {
            return false;
        }
    }
    public class Liveevent
    {
        public Liveevent() { this.Description = string.Empty; }
        public int Id { get; set; }
        public int CelebrityId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string? ReqPhotoPath { get; set; }
        public virtual bool Update(Liveevent liveevent)
        {
            return false;
        }
    }

    public class Repository : IRepository
    {
        private readonly Context context;
        private readonly string _photoFolder;

        public Repository(IOptions<CelebritiesConfig> options)
        {
            var config = options.Value;

            var connectionString = config.ConnectionString;

            context = new Context(connectionString);
            _photoFolder = config.PhotoFolder;
        }


        public List<Celebrity> GetAllCelebrities()
        {
            return context.Celebrities.ToList();
        }

        public Celebrity? GetCelebrityById(int celebrity_id)
        {
            return context.Celebrities.Find(celebrity_id);
        }

        public bool AddCelebrity(Celebrity celebrity)
        {
            context.Celebrities.Add(celebrity);
            return context.SaveChanges() > 0;
        }

        public bool DelCelebrity(int celebrity_id)
        {
            var celebrity = context.Celebrities.Find(celebrity_id);
            if (celebrity != null)
            {
                context.Celebrities.Remove(celebrity);
                return context.SaveChanges() > 0;
            }
            return false;
        }

        public bool UpdCelebrity(int celebrity_id, Celebrity celebrity)
        {
            var existing = context.Celebrities.Find(celebrity_id);
            if (existing != null)
            {
                existing.FullName = celebrity.FullName;
                existing.Nationality = celebrity.Nationality;
                existing.ReqPhotoPath = celebrity.ReqPhotoPath;
                return context.SaveChanges() > 0;
            }
            return false;
        }

        public int GetCelebrityIdByName(string celebrity_name)
        {
            var celebrity = context.Celebrities
                .FirstOrDefault(c => c.FullName == celebrity_name);
            return celebrity?.Id ?? -1;
        }


        public List<Liveevent> GetAllLiveevents()
        {
            return context.Liveevents.ToList();
        }

        public Liveevent? GetLiveeventsById(int liveevent_id)
        {
            return context.Liveevents.Find(liveevent_id);
        }

        public bool AddLiveevent(Liveevent liveevent)
        {
            context.Liveevents.Add(liveevent);
            return context.SaveChanges() > 0;
        }

        public bool DelLiveevent(int liveevent_id)
        {
            var liveevent = context.Liveevents.Find(liveevent_id);
            if (liveevent != null)
            {
                context.Liveevents.Remove(liveevent);
                return context.SaveChanges() > 0;
            }
            return false;
        }

        public bool UpdLiveevent(int liveevent_id, Liveevent liveevent)
        {
            var existing = context.Liveevents.Find(liveevent_id);
            if (existing != null)
            {
                existing.CelebrityId = liveevent.CelebrityId;
                existing.Date = liveevent.Date;
                existing.Description = liveevent.Description;
                existing.ReqPhotoPath = liveevent.ReqPhotoPath;
                return context.SaveChanges() > 0;
            }
            return false;
        }


        public List<Liveevent> GetLifeeventsByCelebrityId(int celebrity_id)
        {
            return context.Liveevents
                .Where(e => e.CelebrityId == celebrity_id)
                .ToList();
        }

        public Celebrity? GetCelebrityByLiveeventId(int liveevent_id)
        {
            var liveevent = context.Liveevents.Find(liveevent_id);
            if (liveevent != null)
            {
                return context.Celebrities.Find(liveevent.CelebrityId);
            }
            return null;
        }


        public void Dispose()
        {
            context?.Dispose();
        }
    }

    public class Context : DbContext
    {
        public string? ConnectionString { get; private set; } = null;
        public Context(string connection_string)
        {
            this.ConnectionString = connection_string;
        }
        
        public Context() : base()
        {
     
        }

        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<Liveevent> Liveevents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder option_builder)
        {
            if (this.ConnectionString is null)
            {
                this.ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=CelebrityDB;Trusted_Connection=True;TrustServerCertificate=True;";
            }

            option_builder.UseSqlServer(this.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder model_builder)
        {
            model_builder.Entity<Celebrity>().ToTable("Celebrities").HasKey(p => p.Id);
            model_builder.Entity<Celebrity>().Property(p => p.Id).IsRequired();
            model_builder.Entity<Celebrity>().Property(p => p.FullName).IsRequired().HasMaxLength(50);
            model_builder.Entity<Celebrity>().Property(p => p.Nationality).IsRequired().HasMaxLength(10);
            model_builder.Entity<Celebrity>().Property(p => p.ReqPhotoPath).IsRequired().HasMaxLength(200);

            model_builder.Entity<Liveevent>().ToTable("Liveevents").HasKey(p => p.Id);
            model_builder.Entity<Liveevent>().ToTable("Liveevents");
            model_builder.Entity<Liveevent>().Property(p => p.Id).IsRequired();
            model_builder.Entity<Liveevent>().ToTable("Liveevents").HasOne<Celebrity>().WithMany().HasForeignKey(p => p.CelebrityId);
            model_builder.Entity<Liveevent>().Property(p => p.CelebrityId).IsRequired();
            model_builder.Entity<Liveevent>().Property(p => p.Date);
            model_builder.Entity<Liveevent>().Property(p => p.Description).HasMaxLength(256);
            model_builder.Entity<Liveevent>().Property(p => p.ReqPhotoPath).HasMaxLength(256);
            base.OnModelCreating(model_builder);
        }
    }
}