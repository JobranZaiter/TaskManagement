using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.Models.Enum;

namespace TaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAssignee> ProjectAssignees { get; set; }
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<TaskPermission> TaskPermissions { get; set; }
        public DbSet<Attachement> Attachements { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enums
            modelBuilder.Entity<TaskPermission>()
                .Property(p => p.Permission)
                .HasConversion<string>();

            // TaskPermission relationships
            modelBuilder.Entity<TaskPermission>()
                .HasOne(p => p.Task)
                .WithMany(t => t.Permissions)
                .HasForeignKey(p => p.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskPermission>()
                .HasOne(p => p.ProjectAssignee)
                .WithMany(a => a.Permissions)
                .HasForeignKey(p => p.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            // User relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProjectOwnerships)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProjectAssignments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project relationships
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectAssignees)
                .WithOne(a => a.Project)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppTask
            modelBuilder.Entity<AppTask>()
                .HasOne(t => t.Assignee)
                .WithMany(a => a.AppTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppTask>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppTask>()
                .HasMany(t => t.SubTasks)
                .WithOne(s => s.Task)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppTask>()
                .HasMany(t => t.Logs)
                .WithOne(l => l.Task)
                .HasForeignKey(l => l.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppTask>()
                .HasMany(t => t.Attachements)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(a => a.AppTasks)
                .WithOne(t => t.Assignee)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(a => a.SubTasks)
                .WithOne(t => t.AssignedTo)
                .HasForeignKey(t => t.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(a => a.Permissions)
                .WithOne(p => p.ProjectAssignee)
                .HasForeignKey(p => p.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(a => a.Attachements)
                .WithOne(p => p.Poster)
                .HasForeignKey(p => p.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.ProjectAssignee)
                .HasForeignKey(c => c.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasMany(p => p.Logs)
                .WithOne(l => l.ProjectAssignee)
                .HasForeignKey(l => l.ProjectAssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubTask>()
                .HasMany(s => s.Logs)
                .WithOne(l => l.SubTask)
                .HasForeignKey(l => l.SubTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignee>()
                .HasIndex(pa => new { pa.ProjectId, pa.UserId })
                .IsUnique();
        }
    }
}
