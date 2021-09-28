using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineQuee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuee.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        internal DbSet<Setting> Settings { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<User> Users { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                
                optionsBuilder.UseSqlite("Data Source=Data.db;");

                //using (var conn = new SqliteConnection("Data Source=Term.db;"))
                //{
                //    conn.Open();
                    // var command = conn.CreateCommand();
                    // command.CommandText = "PRAGMA key=password;";
                    // command.ExecuteNonQuery();

                    //optionsBuilder.UseSqlite(conn);
                //} 

            }
            catch (Exception ex)
            {
                
                throw new Exception("OnConfiguring", ex);
            }
        }
       
    }
}
