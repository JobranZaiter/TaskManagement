using TaskManagement.Models;
using TaskManagement.Models.Enum;
using TaskManagement.Services.Interface;

namespace TaskManagement.Data
{
    public class AppDbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IPasswordManager passwordManager)
        {
            if (!context.Projects.Any())
            {
                // Create 3 users
                passwordManager.CreatePasswordHasher("test123", out string hash1, out string salt1);
                passwordManager.CreatePasswordHasher("test123", out string hash2, out string salt2);
                passwordManager.CreatePasswordHasher("test123", out string hash3, out string salt3);

                var owner = new User { Name = "Owner User", Email = "owner@example.com", PasswordHash = hash1, PasswordSalt = salt1 };
                var assignee1 = new User { Name = "Assignee One", Email = "assignee1@example.com", PasswordHash = hash2, PasswordSalt = salt2 };
                var assignee2 = new User { Name = "Assignee Two", Email = "assignee2@example.com", PasswordHash = hash3, PasswordSalt = salt3 };

                var project = new Project { Title = "Seeded Project", Description = "Project for testing", Owner = owner };

                var pa1 = new ProjectAssignee { Project = project, User = assignee1, Role = "Developer" };
                var pa2 = new ProjectAssignee { Project = project, User = assignee2, Role = "Tester" };

                var tasks = new List<AppTask>();

                for (int i = 1; i <= 2; i++)
                {
                    var task = new AppTask
                    {
                        Title = $"Task {i}",
                        Description = $"Description for Task {i}",
                        DueDate = DateTime.UtcNow.AddDays(i * 5),
                        Project = project,
                        Assignee = pa1,
                        isCompleted = false
                    };

                    // Subtasks
                    var sub1 = new SubTask { Task = task, Description = $"Subtask {i}.1", AssignedTo = pa1, Status = "In Progress" };
                    var sub2 = new SubTask { Task = task, Description = $"Subtask {i}.2", AssignedTo = pa2, Status = "Not Started" };

                    // Attachment
                    var attachment = new Attachement
                    {
                        Task = task,
                        Poster = pa1,
                        Name = $"Attachment_{i}.png",
                        FilePath = $"/files/attachment_{i}.png",
                        Description = $"Attachment for Task {i}"
                    };

                    // Log
                    var log = new Log
                    {
                        Task = task,
                        ProjectAssignee = pa1,
                        ActionType = "Created",
                        Description = $"Task {i} created",
                        Timestamp = DateTime.UtcNow
                    };

                    // Permissions
                    context.TaskPermissions.Add(new TaskPermission { Task = task, ProjectAssignee = pa1, Permission = PermissionType.Read });
                    context.TaskPermissions.Add(new TaskPermission { Task = task, ProjectAssignee = pa1, Permission = PermissionType.Write });
                    context.TaskPermissions.Add(new TaskPermission { Task = task, ProjectAssignee = pa1, Permission = PermissionType.Delete });
                    // Assignee 2 gets no permissions

                    context.SubTasks.AddRange(sub1, sub2);
                    context.Attachements.Add(attachment);
                    context.Logs.Add(log);
                    context.Tasks.Add(task);
                }

                context.Users.AddRange(owner, assignee1, assignee2);
                context.Projects.Add(project);
                context.ProjectAssignees.AddRange(pa1, pa2);

                await context.SaveChangesAsync();
            }
        }

    }
}
    
