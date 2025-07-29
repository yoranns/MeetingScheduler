using MeetingScheduler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.Data
{
    public class MeetingSchedulerContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=meetingScheduler.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.HasMany(e => e.Meetings)
                      .WithOne(e => e.Room)
                      .HasForeignKey(e => e.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.StartTime);
                entity.Property(e => e.EndTime);
                entity.Property(e => e.Organizer).IsRequired();
                entity.Property(e => e.Status)
                      .HasConversion(
                          v => v.ToString(),
                          v => (MeetingStatus)Enum.Parse(typeof(MeetingStatus), v));
            });
        }
    }
}
