using Microsoft.EntityFrameworkCore;
using Buttonry.Application.Shop;
using Buttonry.Infrastructure.Persistence;

namespace Buttonry.Migrator;

/// <summary>Applies the EF Core migrations of Buttonry.Infrastructure to Buttonry database.</summary>
public static class MigratorApp
{
    public const string ConnectionStringVariable = "ConnectionStrings__Buttonry";

    public const string FounderVariable = "Shop__FounderMemberId";

    public const string ProvisionFounderFlag = "--provision-founder";

    /// <summary>
    /// Applies pending migrations when a connection string is given. Without one it lists the
    /// migrations in this build, connects to nothing and fails, so a misconfigured deploy stops.
    /// </summary>
    public static int Run(TextWriter output, string? connectionString)
    {
        var options = new DbContextOptionsBuilder<ButtonryDbContext>()
            .UseSqlServer(string.IsNullOrWhiteSpace(connectionString) ? "Server=unset" : connectionString)
            .Options;
        using var db = new ButtonryDbContext(options);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            output.WriteLine($"Set {ConnectionStringVariable} to apply these migrations:");
            foreach (var migration in db.Database.GetMigrations())
            {
                output.WriteLine($"  {migration}");
            }

            return 2;
        }

        ApplyMigrations(db, output);
        return 0;
    }

    /// <summary>Applies every pending migration. Buttonry.Cli reset calls it too, so there is one migration path (ADR platform/0010).</summary>
    public static void ApplyMigrations(DbContext db, TextWriter output)
    {
        db.Database.Migrate();
        output.WriteLine("Migrations applied.");
    }

    /// <summary>
    /// The trusted operator's step that binds the one founder account (ADR platform/0018). Normal
    /// migration and deployment never call it. Exit 0: bound or already bound; 3: the configuration is
    /// missing or is not one member ID; 4: the account is unknown or unverified, or another founder is stored.
    /// </summary>
    public static int ProvisionFounder(TextWriter output, ButtonryDbContext db, string? configuredFounderId)
    {
        if (!Guid.TryParse(configuredFounderId, out var founderId) || founderId == Guid.Empty)
        {
            output.WriteLine($"Set {FounderVariable} to one member ID (a GUID).");
            return 3;
        }

        var outcome = new FounderProvisioner(db).ProvisionAsync(founderId, CancellationToken.None).GetAwaiter().GetResult();
        output.WriteLine(outcome switch
        {
            ProvisionOutcome.Bound => "Founder bound.",
            ProvisionOutcome.AlreadyBound => "Founder already bound; nothing changed.",
            ProvisionOutcome.ConflictsWithStoredBinding => "A different founder is already bound; refused.",
            ProvisionOutcome.UnknownMember => "No such account; refused.",
            _ => "The account must be open and have a verified email; refused.",
        });
        return outcome is ProvisionOutcome.Bound or ProvisionOutcome.AlreadyBound ? 0 : 4;
    }

    /// <summary>Opens the database and runs <see cref="ProvisionFounder(TextWriter, ButtonryDbContext, string?)"/>.</summary>
    public static int RunProvisionFounder(TextWriter output, string? connectionString, string? configuredFounderId)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            output.WriteLine($"Set {ConnectionStringVariable} to provision the founder.");
            return 2;
        }

        var options = new DbContextOptionsBuilder<ButtonryDbContext>().UseSqlServer(connectionString).Options;
        using var db = new ButtonryDbContext(options);
        return ProvisionFounder(output, db, configuredFounderId);
    }
}
