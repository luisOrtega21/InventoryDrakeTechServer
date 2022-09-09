using INVENTORY.SHARED.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace INVENTORY.SERVER.Data
{
    public class InventoryDBContext : IdentityDbContext<User>
    {
        #region Fields
        private IDbContextTransaction _transaction;
        #endregion

        #region Properties

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; }
        #endregion

        #region Constructor
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options)
            : base(options)
        {
        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Quantity).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Reference).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Date).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).IsRequired().HasMaxLength(100);
            });

            CreateSeed(mb);
        }
        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var result = await SaveChangesAsync();
                _transaction.Commit();
                return result;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
        #endregion

        #region Seeds
        private void CreateSeed(ModelBuilder mb)
        {
            var products = GetProducts();

            mb.Entity<Product>().HasData(products);
        }
        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = new Guid("7151b821-5ea1-4e04-b14f-d5cf3fbcb3de"), Name = "Llanta", Description = "GMW", Price =60000, Quantity = 10, Reference = "LL", Date = new DateTime(2021,01,01) },
                new Product { Id = new Guid("ec2bed18-bafd-46e2-a0f2-69bbe9a8ca46"), Name = "Frenos", Description = "Fren", Price =20000, Quantity = 10, Reference = "Fren", Date = new DateTime(2022,01,01) },
                new Product { Id = new Guid("2518af24-6012-4dba-8a33-34d4a2b865d9"), Name = "Manubrio", Description = "Manu", Price =40000, Quantity = 10, Reference = "Man", Date = new DateTime(2022,05,01) }
            };
        }
    }
    #endregion
}