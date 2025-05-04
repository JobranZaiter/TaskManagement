using TaskManagement.Models;
using TaskManagement.Models.Enum;
using TaskManagement.Services.Interface;

namespace TaskManagement.Data
{
    public class AppDbSeeder
    {
            public static async Task SeedAsync(AppDbContext context , IPasswordManager passwordManager)
            {
                if (!context.Projects.Any())
                {
                    passwordManager.CreatePasswordHasher("test123", out string passwordHash, out string passwordSalt);
                    var user = new User
                    {
                        Name = "John Doe",
                        Email = "john@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };

                    var project = new Project
                    {
                        Title = "Sample Project",
                        Description = "Test project for seeding",
                        Owner = user
                    };

                    var assignee = new ProjectAssignee
                    {
                        User = user,
                        Project = project,
                        Role = "Manager"
                    };

                    var task = new AppTask
                    {
                        Title = "Setup Database",
                        Description = "Initialize and seed database",
                        DueDate = DateTime.UtcNow.AddDays(7),
                        Project = project,
                        Assignee = assignee,
                        isCompleted = false
                    };

                    var subTask = new SubTask
                    {
                        Task = task,
                        AssignedTo = assignee
                    };

                    var comment = new Comment
                    {
                        Task = task,
                        ProjectAssignee = assignee,
                        Text = "Initial setup complete"
                    };

                    var attachment = new Attachement
                    {
                        Task = task,
                        Poster = assignee,
                        Name = "schema.png",
                        Description = "Database schema diagram",
                        FilePath = "/files/schema.png"
                    };

                    var permission = new TaskPermission
                    {
                        Task = task,
                        ProjectAssignee = assignee,
                        Permission = PermissionType.Read
                    };

                    var log = new Log
                    {
                        Task = task,
                        ProjectAssignee = assignee,
                        ActionType = "Created",
                        Description = "Task created and assigned",
                        Timestamp = DateTime.UtcNow
                    };

                    context.Users.Add(user);
                    context.Projects.Add(project);
                    context.ProjectAssignees.Add(assignee);
                    context.Tasks.Add(task);
                    context.SubTasks.Add(subTask);
                    context.Comments.Add(comment);
                    context.Attachements.Add(attachment);
                    context.TaskPermissions.Add(permission);
                    context.Logs.Add(log);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
