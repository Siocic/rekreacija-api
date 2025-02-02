using eRekreacija.Services.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Database.Context
{
    public class RekreacijaContext : DbContext
    {
        public RekreacijaContext(DbContextOptions<RekreacijaContext> options) : base(options) { }

        public DbSet<tbl_SportCategory> TblSportCategory { get; set; }
        public DbSet<tbl_Review> TblReview { get; set; }
        public DbSet<tbl_Holiday> TblHoliday { get; set; }
        public DbSet<tbl_Objects> TblObject { get; set; }
        public DbSet<tbl_Appointment> TblAppointment { get; set; }
        public DbSet<tbl_ObjectHoliday> TblObjectHoliday { get; set; }
        public DbSet<tbl_ObjectSportCategory>TblObjectSportCategory { get; set; }
        public DbSet<tbl_Notification> TblNotification { get; set; }
        public DbSet<tbl_Payment> TbPayment { get; set; }
        public DbSet<tbl_Favorites> TblFavorite {  get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_SportCategory>(e =>
            {
                e.ToTable("tbl_SportCategory");
                e.HasKey(e => e.id);
                e.Property(e => e.name).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<tbl_Review>(e =>
            {
                e.ToTable("tbl_Review");
                e.HasKey(e => e.id);
                e.Property(e => e.comment).IsRequired().HasMaxLength(100);
                e.Property(e => e.rating).IsRequired();
                e.HasOne(e=>e.TblObjects).WithMany(e=>e.Reviews).HasForeignKey(e=>e.object_id).OnDelete(DeleteBehavior.Cascade);    
            });
            modelBuilder.Entity<tbl_Holiday>(e =>
            {
                e.ToTable("tbl_Holiday");
                e.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_Objects>(e =>
            {
                e.ToTable("tbl_Objects");
                e.HasKey(e => e.id);
                e.Property(e => e.name).IsRequired().HasMaxLength(50);
                e.Property(e => e.address).IsRequired().HasMaxLength(50);
                e.Property(e=>e.city).IsRequired().HasMaxLength(50);
                e.Property(e=>e.description).IsRequired().HasMaxLength(200);
            });
            modelBuilder.Entity<tbl_Appointment>(e =>
            {
                e.ToTable("tbl_Appointment");
                e.HasKey(e => e.id);
                e.HasOne(o => o.TblObjects).WithMany(a => a.Appointments).HasForeignKey(o => o.object_id).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<tbl_ObjectHoliday>(e =>
            {
                e.ToTable("tbl_ObjectHoliday");
                e.HasKey(o => new {o.object_id,o.holiday_id});
                e.HasOne(o=>o.TblObjects).WithMany(o=>o.ObjectHoliday).HasForeignKey(o=>o.object_id);
                e.HasOne(o=>o.TblHoliday).WithMany(o=>o.ObjectHolidays).HasForeignKey(o=>o.holiday_id);
            });
            modelBuilder.Entity<tbl_ObjectSportCategory>(e =>
            {
                e.ToTable("tbl_ObjectSportCategory");
                e.HasKey(o => new {o.object_id,o.sport_category_id});
                e.HasOne(o => o.TblObject).WithMany(o => o.ObjectSportCategory).HasForeignKey(o => o.object_id);
                e.HasOne(o => o.TblSportcategory).WithMany(o => o.ObjectSportCategories).HasForeignKey(o => o.sport_category_id);
            });
            modelBuilder.Entity<tbl_Notification>(e =>
            {
                e.ToTable("tbl_Notification");
                e.HasKey(o => o.id);
                e.Property(o => o.name).IsRequired().HasMaxLength(50);
                e.Property(o=>o.description).IsRequired().HasMaxLength(100);
                e.HasOne(n => n.TblObject).WithMany(o=>o.Notifications).HasForeignKey(o=>o.object_id);
            });
            modelBuilder.Entity<tbl_Payment>(e =>
            {
                e.ToTable("tbl_Payment");
                e.HasKey(o => o.id);
                e.HasOne(e=>e.TblObjects).WithMany(e=>e.Payment).HasForeignKey(e=>e.object_id).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(e=>e.TblAppointment).WithOne(e=>e.TblPayment).HasForeignKey<tbl_Payment>(n=>n.appointment_id);
            });
            modelBuilder.Entity<tbl_Favorites>(e =>
            {
                e.ToTable("tbl_Favorites");
                e.HasKey(e => new {e.user_id,e.object_id});
                e.HasOne(f => f.TblObjects).WithMany().HasForeignKey(f => f.object_id).OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
