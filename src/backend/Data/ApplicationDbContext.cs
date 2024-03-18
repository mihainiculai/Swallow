using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Swallow.Models;

namespace Swallow.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
    {
        public DbSet<PlatformSettings> PlatformSettings { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
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
                    MaintenanceMode = false,
                    NextCurrencyUpdate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<Weekday>().HasData(
                new Weekday { WeekdayId = 1, Name = "Monday" },
                new Weekday { WeekdayId = 2, Name = "Tuesday" },
                new Weekday { WeekdayId = 3, Name = "Wednesday" },
                new Weekday { WeekdayId = 4, Name = "Thursday" },
                new Weekday { WeekdayId = 5, Name = "Friday" },
                new Weekday { WeekdayId = 6, Name = "Saturday" },
                new Weekday { WeekdayId = 7, Name = "Sunday" }
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

            modelBuilder.Entity<City>()
                .HasMany(e => e.Trips)
                .WithOne(e => e.City)
                .HasForeignKey(e => e.CityId)
                .IsRequired();

            modelBuilder.Entity<City>()
                .HasMany(e => e.Attractions)
                .WithOne(e => e.City)
                .HasForeignKey(e => e.CityId)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .HasIndex(e => e.Iso2)
                .IsUnique();

            modelBuilder.Entity<Country>()
                .HasIndex(e => e.Iso3)
                .IsUnique();

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

            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.ToTable("Claims");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");
            });
        }
    }
}
