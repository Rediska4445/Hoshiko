namespace Hoshiko.Context
{
    using Hoshiko.Models.Entity;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class HoshikoDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MediaContentEntity> MediaContents { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<SeriesEntity> Series { get; set; }
        public DbSet<EpisodeEntity> Episodes { get; set; }
        public DbSet<MusicEntity> Music { get; set; }
        public DbSet<UserFavoriteGenreEntity> UserFavoriteGenres { get; set; }

        public HoshikoDbContext() : base("HoshikoDB")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<UserEntity>().HasKey(u => u.Id);
            modelBuilder.Entity<UserEntity>()
                        .Property(u => u.Username)
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode()
                        .IsVariableLength();
            modelBuilder.Entity<UserEntity>()
                        .HasIndex(u => u.Username)
                        .IsUnique();
            modelBuilder.Entity<UserEntity>()
                        .Property(u => u.PasswordHash)
                        .IsRequired()
                        .HasMaxLength(256);

            modelBuilder.Entity<MediaContentEntity>().ToTable("MediaContent");
            modelBuilder.Entity<MediaContentEntity>().HasKey(mc => mc.Id);
            modelBuilder.Entity<MediaContentEntity>()
                        .Property(mc => mc.Name)
                        .IsRequired()
                        .HasMaxLength(20);

            modelBuilder.Entity<GenreEntity>().ToTable("Genres");
            modelBuilder.Entity<GenreEntity>().HasKey(g => g.Id);
            modelBuilder.Entity<GenreEntity>()
                        .Property(g => g.Name)
                        .IsRequired()
                        .HasMaxLength(100);
            modelBuilder.Entity<GenreEntity>()
                        .Property(g => g.MediaContentId)
                        .IsRequired();
            modelBuilder.Entity<GenreEntity>()
                        .HasRequired(g => g.MediaContent)
                        .WithMany()
                        .HasForeignKey(g => g.MediaContentId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<GenreEntity>()
                        .HasIndex(g => new { g.Name, g.MediaContentId })
                        .IsUnique();

            modelBuilder.Entity<MovieEntity>().ToTable("Movies");
            modelBuilder.Entity<MovieEntity>().HasKey(m => m.Id);
            modelBuilder.Entity<MovieEntity>()
                        .Property(m => m.Title)
                        .IsRequired()
                        .HasMaxLength(300);
            modelBuilder.Entity<MovieEntity>()
                        .Property(m => m.SourcePath)
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnName("FilePath");
            modelBuilder.Entity<MovieEntity>()
                        .Property(m => m.UploadDate)
                        .IsRequired();
            modelBuilder.Entity<MovieEntity>()
                        .Property(m => m.UploadedByUserId)
                        .IsRequired();
            modelBuilder.Entity<MovieEntity>()
                        .Property(m => m.GenreId)
                        .IsRequired();

            modelBuilder.Entity<MovieEntity>()
                        .HasRequired(m => m.UploadedByUser)
                        .WithMany()
                        .HasForeignKey(m => m.UploadedByUserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MovieEntity>()
                        .HasRequired(m => m.Genre)
                        .WithMany()
                        .HasForeignKey(m => m.GenreId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<SeriesEntity>().ToTable("Series");
            modelBuilder.Entity<SeriesEntity>().HasKey(s => s.Id);
            modelBuilder.Entity<SeriesEntity>()
                        .Property(s => s.Title)
                        .IsRequired()
                        .HasMaxLength(300);
            modelBuilder.Entity<SeriesEntity>()
                        .Property(s => s.SourcePath)
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnName("FilePath");
            modelBuilder.Entity<SeriesEntity>()
                        .Property(s => s.UploadDate)
                        .IsRequired();
            modelBuilder.Entity<SeriesEntity>()
                        .Property(s => s.UploadedByUserId)
                        .IsRequired();
            modelBuilder.Entity<SeriesEntity>()
                        .Property(s => s.GenreId)
                        .IsRequired();

            modelBuilder.Entity<SeriesEntity>()
                        .HasRequired(s => s.UploadedByUser)
                        .WithMany()
                        .HasForeignKey(s => s.UploadedByUserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<SeriesEntity>()
                        .HasRequired(s => s.Genre)
                        .WithMany()
                        .HasForeignKey(s => s.GenreId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<SeriesEntity>()
                        .HasMany(s => s.Episodes)
                        .WithRequired(e => e.Series)
                        .HasForeignKey(e => e.SeriesId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<EpisodeEntity>().ToTable("Episodes");
            modelBuilder.Entity<EpisodeEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.SeriesId)
                        .IsRequired();
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.Title)
                        .HasMaxLength(300);
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.EpisodeNumber)
                        .IsRequired();
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.FilePath)
                        .IsRequired()
                        .HasMaxLength(1000);
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.UploadDate)
                        .IsRequired();
            modelBuilder.Entity<EpisodeEntity>()
                        .Property(e => e.UploadedByUserId)
                        .IsRequired();

            modelBuilder.Entity<EpisodeEntity>()
                        .HasRequired(e => e.UploadedByUser)
                        .WithMany()
                        .HasForeignKey(e => e.UploadedByUserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MusicEntity>().ToTable("Music");
            modelBuilder.Entity<MusicEntity>().HasKey(m => m.Id);
            modelBuilder.Entity<MusicEntity>()
                        .Property(m => m.Title)
                        .IsRequired()
                        .HasMaxLength(300);
            modelBuilder.Entity<MusicEntity>()
                        .Property(m => m.SourcePath)
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnName("FilePath");
            modelBuilder.Entity<MusicEntity>()
                        .Property(m => m.UploadDate)
                        .IsRequired();
            modelBuilder.Entity<MusicEntity>()
                        .Property(m => m.UploadedByUserId)
                        .IsRequired();
            modelBuilder.Entity<MusicEntity>()
                        .Property(m => m.GenreId)
                        .IsRequired();

            modelBuilder.Entity<MusicEntity>()
                        .HasRequired(m => m.UploadedByUser)
                        .WithMany()
                        .HasForeignKey(m => m.UploadedByUserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MusicEntity>()
                        .HasRequired(m => m.Genre)
                        .WithMany()
                        .HasForeignKey(m => m.GenreId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserFavoriteGenreEntity>().ToTable("UserFavoriteGenres");
            modelBuilder.Entity<UserFavoriteGenreEntity>()
                        .HasKey(ufg => new { ufg.UserId, ufg.GenreId });

            modelBuilder.Entity<UserFavoriteGenreEntity>()
                        .HasRequired(ufg => ufg.User)
                        .WithMany()
                        .HasForeignKey(ufg => ufg.UserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserFavoriteGenreEntity>()
                        .HasRequired(ufg => ufg.Genre)
                        .WithMany()
                        .HasForeignKey(ufg => ufg.GenreId)
                        .WillCascadeOnDelete(false);
        }
    }
}
