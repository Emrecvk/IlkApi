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
}