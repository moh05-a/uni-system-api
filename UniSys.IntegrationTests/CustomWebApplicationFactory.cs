using System;                                   // Provides basic .NET classes like Guid.
using System.Linq;                              // Allows LINQ methods such as Where() and ToList().
using Microsoft.AspNetCore.Hosting;             // Provides IWebHostBuilder for configuring the test web host.
using Microsoft.AspNetCore.Mvc.Testing;         // Provides WebApplicationFactory for creating an in-memory test server.
using Microsoft.EntityFrameworkCore;            // Provides Entity Framework Core functionality.
using Microsoft.Extensions.DependencyInjection; // Provides dependency injection services.
using UniSys.Data;                              // Imports the application's database context.
using UniSys.Models;                            // Imports the application's model classes (Student, Tutor, Subject).

namespace UniSys.IntegrationTests
{
    // Custom factory used to create a test version of the application.
    // It overrides the normal database with an in-memory database.
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Generates a unique database name every time the factory is created.
        // This ensures each test gets its own isolated database.
        public string DbName { get; } = Guid.NewGuid().ToString();

        // Called automatically before the test server starts.
        // Allows us to modify the application's configuration for testing.
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configure the services (Dependency Injection container).
            builder.ConfigureServices(services =>
            {
                // Find all services related to the application's DbContext.
                // We want to remove the real SQL Server configuration.
                var descriptors = services
                    .Where(d => d.ServiceType.FullName != null &&
                                d.ServiceType.FullName.Contains("DbContext") ||
                                d.ServiceType == typeof(DbContextOptions) ||
                                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))
                    .ToList();

                // Remove every database-related service found above.
                // This prevents the application from using the real database.
                foreach (var d in descriptors)
                    services.Remove(d);

                // Register a new in-memory database instead of SQL Server.
                // This database exists only while the tests are running.
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(DbName));

                // Build a temporary service provider so we can access services immediately.
                var sp = services.BuildServiceProvider();

                // Create a service scope because DbContext is a scoped service.
                using var scope = sp.CreateScope();

                // Retrieve the ApplicationDbContext from Dependency Injection.
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Create the in-memory database if it doesn't already exist.
                db.Database.EnsureCreated();

                // Insert sample data into the database for testing.
                SeedTestData(db);
            });
        }

        // Inserts predefined data into the in-memory database.
        // This gives every test a known starting state.
        private void SeedTestData(ApplicationDbContext db)
        {
            // Add two tutors to the Tutors table.
            db.Tutors.AddRange(
                new Tutor
                {
                    Id = 1,
                    Name = "Dr. Smith",
                    Department = "Computer Science"
                },
                new Tutor
                {
                    Id = 2,
                    Name = "Dr. Johnson",
                    Department = "Mathematics"
                }
            );

            // Add two subjects to the Subjects table.
            db.Subjects.AddRange(
                new Subject
                {
                    Id = 1,
                    Name = "Algorithms",
                    CreditHours = 3
                },
                new Subject
                {
                    Id = 2,
                    Name = "Calculus",
                    CreditHours = 4
                }
            );

            // Add three students to the Students table.
            db.Students.AddRange(
                new Student
                {
                    Id = 1,
                    Name = "Alice",
                    Major = "CS"
                },
                new Student
                {
                    Id = 2,
                    Name = "Bob",
                    Major = "Math"
                },
                new Student
                {
                    Id = 3,
                    Name = "Charlie",
                    Major = "CS"
                }
            );

            // Save all inserted records to the in-memory database.
            db.SaveChanges();
        }
    }
}