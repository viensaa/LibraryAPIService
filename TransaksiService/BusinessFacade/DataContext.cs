using Microsoft.EntityFrameworkCore;
using TransaksiService.DomainObject;
using TransaksiService.Model;

namespace TransaksiService.BusinessFacade
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Mahasiswa> Mahasiswas { get; set; } 
        //public DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Transaction Configuration
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.TransactionDetails)
                .WithOne(td => td.Transaction)
                .HasForeignKey(td => td.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Mahasiswa)
                .WithMany(m => m.transactions)
                .HasForeignKey(t => t.MahasiswaId);

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.Staff)
            //    .WithMany(s => s.Transactions)
            //    .HasForeignKey(t => t.StaffId);

            //transactionDetail Configuration
            modelBuilder.Entity<TransactionDetail>()
                .HasKey(td => td.TransactionDetailId);




        }

    }
}
