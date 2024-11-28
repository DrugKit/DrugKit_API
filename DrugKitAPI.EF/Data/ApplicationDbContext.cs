using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DrugKitAPI.EF.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DrugImg>().HasKey(d => new {d.DrugId,d.ImagePath});
            builder.Entity<DrugCategory>().HasKey(d => new { d.DrugId, d.CategoryId });
            builder.Entity<DonationImg>().HasKey(d => new { d.DonationId, d.ImagPath });
            builder.Entity<SideEffect>().HasKey(s => new { s.DrugId, s.Effect });
            builder.Entity<UserRequestedDonation>().HasKey(u => new { u.MobileUserId, u.DonationId });
            base.OnModelCreating(builder);
        }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<SideEffect> SideEffects { get; set; }
        public DbSet<BannedUser> BannedUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DrugCategory> DrugCategories { get; set; }
        public DbSet<DrugImg> DrugImgs { get; set; }
        public DbSet<MobileUser> MobileUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<UserRequestedDonation> UserRequestedDonations { get; set; }
    }
}