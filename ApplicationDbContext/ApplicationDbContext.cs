using AOTableModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableDbContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base (option)
        {
            
        }
        public DbSet<AOTable> Tables { get; set; }
        public DbSet<AOColumn> AOColumns { get; set; }
        public DbSet<Form> Forms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Table keys and constraints
            modelBuilder.Entity<AOTable>()
                .Property(table => table.Id).IsRequired();
            modelBuilder.Entity<AOTable>()
                .Property(table => table.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<AOTable>()
                .Property(table=>table.Type).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<AOTable>()
                .Property(table=> table.Description).HasMaxLength(255);
            modelBuilder.Entity<AOTable>()
                .Property(table=>table.Comment).HasMaxLength(2408);

            modelBuilder.Entity<AOTable>().HasKey(table => table.Id);
            #endregion

        }
    }
}
