using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;
using Buttonry.Application.Abstractions;
using Buttonry.Application.Notifications;
using Buttonry.Application.Orders;
using Buttonry.Application.Shop;
using Buttonry.Domain.Shop;
using Buttonry.Infrastructure.Persistence;

namespace Buttonry.Api.Tests.Support;

/// <summary>
/// Hosts Buttonry.Api in memory with SQLite in-memory in place of SQL Server, a captured email
/// sender, in-memory photo storage, a fake payment provider, a fake carrier tracking service, a fake Google and a controllable clock. No real database or external system is touched (AGENTS.md).
/// </summary>
public sealed class ButtonryApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;
    private readonly string? _sharedDatabase;

    /// <summary>
    /// One in-memory database on one connection. With <paramref name="concurrent"/>, every
    /// DbContext opens its own connection to a shared-cache in-memory database instead, so
    /// parallel requests run in parallel as they would against SQL Server.
    /// </summary>
    public ButtonryApiFactory(bool concurrent = false)
    {
        _sharedDatabase = concurrent ? $"Data Source=file:buttonry-{Guid.NewGuid():N}?mode=memory&cache=shared" : null;
        _connection = new SqliteConnection(_sharedDatabase ?? "DataSource=:memory:");
        _connection.Open();
    }

    /// <summary>Starts at the real time, because the cookie container expires cookies by the real clock.</summary>
    public FakeTimeProvider Clock { get; } = new(DateTimeOffset.UtcNow);

    public CapturingEmailSender Email { get; } = new();

    public FakePhotoStorage Photos { get; } = new();

    public FakePaymentProvider Payments { get; } = new();

    public FakeCarrierTracking Carriers { get; } = new();


    public RecordingOrderCompleted Completed { get; } = new();

    public FakeGoogleIdentityProvider Google { get; } = new();

    /// <summary>The shop configuration the host reads; a test changes it as a restart with new settings would.</summary>
    public ShopOptions Shop { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Privacy:FieldEncryptionKeys", "test1:AAECAwQFBgcICQoLDA0ODxAREhMUFRYXGBkaGxwdHh8=");
        builder.UseSetting("Auth:SigningKey", "test-signing-key-that-is-at-least-32-bytes-long");
        builder.UseSetting("App:BaseUrl", "https://buttonry.test");
        builder.UseSetting("Notifications:UnsubscribeSigningKey", "test-unsubscribe-key");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ButtonryDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<ButtonryDbContext>>();
            services.AddDbContext<ButtonryDbContext>(options =>
            {
                if (_sharedDatabase is null)
                {
                    options.UseSqlite(_connection);
                }
                else
                {
                    options.UseSqlite(_sharedDatabase);
                }
            });
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(Clock);
            services.RemoveAll<IEmailSender>();
            services.AddSingleton<IEmailSender>(Email);
            services.RemoveAll<IPhotoStorage>();
            services.AddSingleton<IPhotoStorage>(Photos);
            services.RemoveAll<IPaymentProvider>();
            services.AddSingleton<IPaymentProvider>(Payments);
            services.RemoveAll<ICarrierTracking>();
            services.AddSingleton<ICarrierTracking>(Carriers);
            services.AddSingleton<INotificationHandler<OrderCompleted>>(Completed);
            services.RemoveAll<ShopOptions>();
            services.AddSingleton(Shop);
            services.RemoveAll<IGoogleIdentityProvider>();
            services.AddSingleton<IGoogleIdentityProvider>(Google);
        });
    }

    /// <summary>Creates the schema once the host is built.</summary>
    public void EnsureDatabase()
    {
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ButtonryDbContext>().Database.EnsureCreated();
    }

    /// <summary>
    /// An HTTPS client that keeps cookies, so Secure session cookies round-trip. Without
    /// <paramref name="followRedirects"/> each hop of a redirect flow can be asserted.
    /// </summary>
    public HttpClient CreateMemberClient(bool followRedirects = true)
    {
        EnsureDatabase();
        return CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = followRedirects });
    }

    /// <summary>One dispatcher pass, as the worker runs it; returns the emails it sent.</summary>
    public async Task<IReadOnlyList<EmailMessage>> DispatchEmailsAsync()
    {
        var before = Email.Sent.Count;
        using var scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ISender>().Send(new DispatchEmailsCommand());
        return Email.Sent.Skip(before).ToList();
    }

    /// <summary>The ID of the registered member with this email.</summary>
    public Guid MemberId(string email)
    {
        EnsureDatabase();
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ButtonryDbContext>();
        return db.Members.AsNoTracking().Single(m => m.NormalizedEmail == email.ToUpperInvariant()).Id;
    }

    /// <summary>Configures and stores the founder binding, as the operator's provisioning step does.</summary>
    public void BindFounder(Guid memberId)
    {
        if (Shop.FounderMemberId is not null)
        {
            return;
        }

        Shop.FounderMemberId = memberId;
        InDatabase(db =>
        {
            db.ShopIdentities.Add(ShopIdentity.Bind(memberId));
            db.SaveChanges();
        });
    }

    public void InDatabase(Action<ButtonryDbContext> action)
    {
        using var scope = Services.CreateScope();
        action(scope.ServiceProvider.GetRequiredService<ButtonryDbContext>());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
