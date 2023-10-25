using Microsoft.EntityFrameworkCore;
using Swallow.Models;

namespace Swallow.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public DbSet<PlatformSettings> PlatformSettings { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ItineraryDay> ItineraryDays { get; set; }
        public DbSet<ItineraryAttraction> ItineraryAttractions { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionCategory> AttractionCategories { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<TripTransport> TripTransports { get; set; }
        public DbSet<TransportMode> TransportModes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlatformSettings>().HasData(
                new PlatformSettings
                {
                    SettingsId = 1,
                    MentenanceMode = false,
                    NextCurrencyUpdate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<User>()
                .HasMany(e => e.Trips)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserPlans)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<UserPlan>()
                .HasOne(e => e.Plan)
                .WithMany(e => e.UserPlans)
                .HasForeignKey(e => e.PlanId)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Cities)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.CountryId)
                .IsRequired();

            modelBuilder.Entity<Trip>()
                .HasOne(e => e.City)
                .WithMany(e => e.Trips)
                .HasForeignKey(e => e.TripId)
                .IsRequired();

            modelBuilder.Entity<City>()
                .HasMany(e => e.Attractions)
                .WithOne(e => e.City)
                .HasForeignKey(e => e.CityId)
                .IsRequired();

            modelBuilder.Entity<Attraction>()
                .HasMany(e => e.Schedules)
                .WithOne(e => e.Attraction)
                .HasForeignKey(e => e.AttractionId)
                .IsRequired();

            modelBuilder.Entity<Schedule>()
                .HasOne(e => e.Weekday)
                .WithMany(e => e.Schedules)
                .HasForeignKey(e => e.WeekdayId)
                .IsRequired();

            modelBuilder.Entity<Trip>()
                .HasMany(e => e.ItineraryDays)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripId)
                .IsRequired();

            modelBuilder.Entity<ItineraryDay>()
                .HasMany(e => e.ItineraryAttractions)
                .WithOne(e => e.ItineraryDay)
                .HasForeignKey(e => e.ItineraryDayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ItineraryAttraction>()
                .HasOne(e => e.Attraction)
                .WithMany(e => e.ItineraryAttractions)
                .HasForeignKey(e => e.AttractionId)
                .IsRequired();

            modelBuilder.Entity<Attraction>()
                .HasOne(e => e.Currency)
                .WithMany(e => e.Attractions)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired(false);

            modelBuilder.Entity<Attraction>()
                .HasMany(e => e.AttractionCategories)
                .WithMany(e => e.Attractions)
                .UsingEntity("AttractionsToCategories");

            modelBuilder.Entity<ItineraryAttraction>()
                .HasOne(e => e.Currency)
                .WithMany(e => e.ItineraryAttractions)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired(false);

            modelBuilder.Entity<Trip>()
                .HasMany(e => e.TripTransports)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripId)
                .IsRequired();

            modelBuilder.Entity<Trip>()
                .HasMany(e => e.Expenses)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripId)
                .IsRequired();

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Currency)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired(false);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ExpenseCategory)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .IsRequired();

            modelBuilder.Entity<TripTransport>()
                .HasOne(e => e.TransportMode)
                .WithMany(e => e.TripTransports)
                .HasForeignKey(e => e.TransportModeId)
                .IsRequired();

            modelBuilder.Entity<TripTransport>()
                .HasOne(e => e.Currency)
                .WithMany(e => e.TripTransports)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.CurrencyRates)
                .WithOne(e => e.Currency)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Currencies)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.CountryId)
                .IsRequired(false);
        }
    }
}
