using BikeScanner.DAL.Configurations;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.DAL
{
    public class BikeScannerContext : DbContext
    {
        public BikeScannerContext(DbContextOptions<BikeScannerContext> options)
            : base(options)
        { }

        public DbSet<Content> Contents { get; set; }
        public DbSet<NotificationQueue> NotificationsQueue { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<JobExecutionInfo> JobExecutionInfo { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DevMessage> DevMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContentsConfiguration());
            modelBuilder.ApplyConfiguration(new DevMessagesConfiguration());
            modelBuilder.ApplyConfiguration(new JobsExecInfoConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationQueueConfiguration());
            modelBuilder.ApplyConfiguration(new SubsciptionsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}

