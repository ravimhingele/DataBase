using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace BigDataApplication1
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<MovieTitle> MovieTitles { get; set; }

        public ApplicationContext(bool flag)
        {
            if (flag)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            else
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
            optionsBuilder.EnableSensitiveDataLogging();
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.ID);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Rating);

            modelBuilder.Entity<MovieTitle>()
                .HasKey(mt => new { mt.MovieId, mt.Ordering, mt.Title });

            modelBuilder.Entity<MovieTitle>()
                .HasOne(mt => mt.Movie)
                .WithMany(m => m.Titles)
                .HasForeignKey(mt => mt.MovieId) // Указываем свойства внешнего ключа для ссылки на соответствующие свойства первичного ключа Movie
                .HasPrincipalKey(m => m.ID);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors) // Определяем связь между Movie и Actor
                .WithMany() // Нет связи обратно от Actor к Movie
                .UsingEntity(join => join.ToTable("MovieActor"));
                

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Tags)
                .WithMany()
                .UsingEntity(join => join.ToTable("MovieTag"));
        }
    }
}

