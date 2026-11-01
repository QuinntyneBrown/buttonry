using Microsoft.EntityFrameworkCore;
using Buttonry.Application.Abstractions;
using Buttonry.Domain.Listings;
using Buttonry.Domain.Members;
using Buttonry.Domain.Notifications;
using Buttonry.Domain.Orders;
using Buttonry.Domain.Payouts;
using Buttonry.Domain.Shop;

namespace Buttonry.Infrastructure.Persistence;

/// <summary>EF Core context over Buttonry database (SQL Server). LINQ only, so all SQL is parameterised (L2-049).</summary>
public sealed class ButtonryDbContext(DbContextOptions<ButtonryDbContext> options) : DbContext(options), IButtonryDbContext
{
    public DbSet<Member> Members => Set<Member>();

    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();

    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    public DbSet<ExternalLogin> ExternalLogins => Set<ExternalLogin>();

    public DbSet<PendingExternalJoin> PendingExternalJoins => Set<PendingExternalJoin>();

    public DbSet<Listing> Listings => Set<Listing>();

    public DbSet<ListingPhoto> ListingPhotos => Set<ListingPhoto>();

    public DbSet<Checkout> Checkouts => Set<Checkout>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Refund> Refunds => Set<Refund>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<SavedAddress> SavedAddresses => Set<SavedAddress>();

    public DbSet<PaymentWebhookEvent> PaymentWebhookEvents => Set<PaymentWebhookEvent>();

    public DbSet<Payout> Payouts => Set<Payout>();

    public DbSet<SellerPayoutAccount> SellerPayoutAccounts => Set<SellerPayoutAccount>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public DbSet<OrderProblem> OrderProblems => Set<OrderProblem>();

    public DbSet<NotificationPreferences> NotificationPreferences => Set<NotificationPreferences>();

    public DbSet<ShopIdentity> ShopIdentities => Set<ShopIdentity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ButtonryDbContext).Assembly);
}
