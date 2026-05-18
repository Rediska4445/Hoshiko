namespace Hoshiko.Context
{
    using Hoshiko.Models.Entity;
    using System.Data.Entity;

    public class HoshikoDbContext : DbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<MusicEntity> Music { get; set; }
        public DbSet<SeriesEntity> Series { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }

        public HoshikoDbContext() : base("HoshikoDB") {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Фильмы ===
            modelBuilder.Entity<MovieEntity>().ToTable("Movies");

            modelBuilder.Entity<MovieEntity>()
                        .Property(e => e.Id)
                        .HasColumnName("Id");

            modelBuilder.Entity<MovieEntity>()
                        .Property(e => e.Title)
                        .HasColumnName("Title");

            modelBuilder
                .Entity<MovieEntity>()
                .Property(e => e.SourcePath)
                .HasColumnName("FilePath");

            modelBuilder.Entity<MovieEntity>()
                        .Property(e => e.UploadDate)
                        .HasColumnName("UploadDate");

            modelBuilder.Entity<MovieEntity>()
                        .Property(e => e.UploadedByUserId)
                        .HasColumnName("UploadedByUserId");

            // === Музыка ===
            modelBuilder.Entity<MusicEntity>().ToTable("Music");

            modelBuilder.Entity<MusicEntity>()
                        .Property(e => e.Id)
                        .HasColumnName("Id");

            modelBuilder.Entity<MusicEntity>()
                        .Property(e => e.Title)
                        .HasColumnName("Title");

            modelBuilder.Entity<MusicEntity>()
                        .Property(e => e.SourcePath)
                        .HasColumnName("FilePath");

            modelBuilder.Entity<MusicEntity>()
                        .Property(e => e.UploadDate)
                        .HasColumnName("UploadDate");

            modelBuilder.Entity<MusicEntity>()
                        .Property(e => e.UploadedByUserId)
                        .HasColumnName("UploadedByUserId");

            // === Сериалы ===
            modelBuilder.Entity<SeriesEntity>().ToTable("Series");
            modelBuilder.Entity<SeriesEntity>()
                        .Property(e => e.SourcePath)
                        .HasColumnName("FilePath");

            modelBuilder.Entity<GenreEntity>().ToTable("Genres");

            // === Genres === 
            modelBuilder.Entity<GenreEntity>()
                        .Property(e => e.Id)
                        .HasColumnName("Id");

            modelBuilder.Entity<GenreEntity>()
                        .Property(e => e.Name)
                        .HasColumnName("Name");

            // === Episodes === 
            modelBuilder.Entity<SeriesEntity>()
                .HasMany(s => s.Episodes)
                .WithRequired(e => e.Series)
                .HasForeignKey(e => e.SeriesId);
        }
    }
}
