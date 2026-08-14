using Microsoft.EntityFrameworkCore;
using IlkApi.Modeller;

namespace IlkApi.Veri;

public class UygulamaDbContext : DbContext
{
    public UygulamaDbContext(DbContextOptions<UygulamaDbContext> secenekler)
        : base(secenekler)
    {
    }

    public DbSet<Oyun> Oyunlar => Set<Oyun>();
    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Kullanici>()
            .HasIndex(k => k.Eposta)
            .IsUnique();
    }
}