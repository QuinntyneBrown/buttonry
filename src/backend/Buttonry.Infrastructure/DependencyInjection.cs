using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Buttonry.Application.Abstractions;
using Buttonry.Application.Catalog;
using Buttonry.Application.Listings;
using Buttonry.Application.Notifications;
using Buttonry.Application.Shop;
using Buttonry.Infrastructure.Accounts;
using Buttonry.Infrastructure.Carriers;
using Buttonry.Infrastructure.Email;
using Buttonry.Infrastructure.Payments;
using Buttonry.Infrastructure.Persistence;
using Buttonry.Infrastructure.Photos;
using Buttonry.Infrastructure.Privacy;

namespace Buttonry.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Registers Buttonry database and the adapters behind the application ports.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ButtonryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Buttonry")));
        services.AddScoped<IButtonryDbContext>(sp => sp.GetRequiredService<ButtonryDbContext>());
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
        services.AddSingleton<IBreachedPasswords, BundledBreachedPasswords>();
        services.AddSingleton<IEmailSender, LoggingEmailSender>();
        services.AddSingleton(ShopConfiguration.Read(configuration));
        FieldCipher.Use(FieldCipher.FromConfiguration(configuration["Privacy:FieldEncryptionKeys"]));
        services.Configure<AppOptions>(configuration.GetSection("App"));
        services.Configure<NotificationOptions>(configuration.GetSection("Notifications"));
        services.Configure<SellingOptions>(configuration.GetSection("Selling"));
        services.Configure<PhotoOptions>(configuration.GetSection("Photos"));
        services.Configure<CatalogOptions>(configuration.GetSection("Catalog"));
        services.Configure<PhotoStorageOptions>(configuration.GetSection("PhotoStorage"));
        services.AddSingleton<IImageInspector, SkiaImageInspector>();
        services.AddSingleton<IListingPhotoProcessor, SkiaPhotoProcessor>();
        services.AddSingleton<IPhotoStorage, FileSystemPhotoStorage>();
        services.AddSingleton<IPaymentProvider, UnconfiguredPaymentProvider>();
        services.AddSingleton<ICarrierTracking, UnconfiguredCarrierTracking>();
        return services;
    }
}

/// <summary>Reads <c>Shop:FounderMemberId</c>. A value that is set but not a single GUID stops startup; an unset value grants no access (ADR platform/0018).</summary>
internal static class ShopConfiguration
{
    public static ShopOptions Read(IConfiguration configuration)
    {
        var raw = configuration[$"{ShopOptions.Section}:{nameof(ShopOptions.FounderMemberId)}"];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return new ShopOptions();
        }

        return Guid.TryParse(raw, out var id) && id != Guid.Empty
            ? new ShopOptions { FounderMemberId = id }
            : throw new InvalidOperationException("Shop:FounderMemberId must be one member ID (a GUID), not a list or another value");
    }
}
