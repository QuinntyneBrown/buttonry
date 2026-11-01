using Microsoft.EntityFrameworkCore;
using Buttonry.Infrastructure.Persistence;
using Buttonry.Migrator;

namespace Buttonry.Cli;

/// <summary>Drops and rebuilds Buttonry database. A seam so tests can rebuild SQLite without the SQL Server migrations.</summary>
public interface IDatabaseSchema
{
    void Rebuild(ButtonryDbContext db, TextWriter output);
}

/// <summary>Drops the database and applies every migration through the migrator's own code (L2-072).</summary>
public sealed class MigratedSchema : IDatabaseSchema
{
    public void Rebuild(ButtonryDbContext db, TextWriter output)
    {
        output.WriteLine(db.Database.EnsureDeleted() ? "Database dropped." : "No database to drop.");
        MigratorApp.ApplyMigrations(db, output);
    }
}
