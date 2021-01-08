using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeManagementApp.Models;

namespace TimeManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
            //Database.EnsureCreated();
        }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventContent> EventContents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.UserId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(e => e.Event)
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(e => e.EventId);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(p => p.User)
                .WithMany(p => p.EventParticipants)
                .HasForeignKey(p => p.UserId);
        }
    }
}
