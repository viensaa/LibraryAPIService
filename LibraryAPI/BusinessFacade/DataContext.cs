using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.BusinessFacade
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Buku> Bukus { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Mahasiswa> Mahasiswas { get; set; }
        public DbSet<Publisher> publishers { get; set; }
        public DbSet<Staff> staffs { get; set; }
        public DbSet<StorageLocation> storageLocations { get; set; }
    }
}
