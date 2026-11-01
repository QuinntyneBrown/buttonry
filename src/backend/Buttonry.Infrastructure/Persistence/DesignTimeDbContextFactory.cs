using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Buttonry.Infrastructure.Persistence;

/// <summary>Lets dotnet-ef build the model to add migrations. It never opens a connection.</summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ButtonryDbContext>
{
    public ButtonryDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<ButtonryDbContext>().UseSqlServer("Server=design-time;Database=Buttonry").Options);
}
