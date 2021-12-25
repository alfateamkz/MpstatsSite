using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramMpBot.Database
{
    public class DatabaseConnection : DbContext
    {
        public DbSet<Violator> Violators { get; set; }
        public DatabaseConnection()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=tgbotdb;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\12.0V_shared;Database=tgbotdb;Trusted_Connection=True;");           
        }
    }

    public class Violator
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public bool IsWatched { get; set; }

    }
}
