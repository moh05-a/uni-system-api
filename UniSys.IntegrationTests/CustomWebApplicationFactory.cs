using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniSys.Data;
using UniSys.Models;

namespace UniSys.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public string DbName { get; } = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptors = services
                    .Where(d => d.ServiceType.FullName != null &&
                                d.ServiceType.FullName.Contains("DbContext") ||
                                d.ServiceType == typeof(DbContextOptions) ||
                                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))
                    .ToList();

                foreach (var d in descriptors)
                    services.Remove(d);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(DbName));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            });
        }

        private void SeedTestData(ApplicationDbContext db)
        {
            db.Tutors.AddRange(
                new Tutor { Id = 1, Name = "Dr. Smith",   Department = "Computer Science" },
                new Tutor { Id = 2, Name = "Dr. Johnson", Department = "Mathematics" }
            );
            db.Subjects.AddRange(
                new Subject { Id = 1, Name = "Algorithms", CreditHours = 3 },
                new Subject { Id = 2, Name = "Calculus",   CreditHours = 4 }
            );
            db.Students.AddRange(
                new Student { Id = 1, Name = "Alice",   Major = "CS" },
                new Student { Id = 2, Name = "Bob",     Major = "Math" },
                new Student { Id = 3, Name = "Charlie", Major = "CS" }
            );
            db.SaveChanges();
        }
    }
}