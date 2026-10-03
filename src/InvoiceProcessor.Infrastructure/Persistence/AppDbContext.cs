using InvoiceProcessor.Domain.Categories;
using InvoiceProcessor.Domain.Invoices;
using InvoiceProcessor.Domain.Users;
using InvoiceProcessor.Domain.Vendors;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}