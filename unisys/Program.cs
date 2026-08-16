using Microsoft.EntityFrameworkCore; // Provides Entity Framework Core features (DbContext, LINQ, etc.)
using UniSys.Data; // Your application's data layer (DbContext, entities)
using UniSys.Filters;
using UniSys.Middleware;
using UniSys.Services; // Business logic/services layer
using UniSys.Repositories; // Data access layer (repositories)
using UniSys.Messaging; // RabbitMQ publisher/consumer
//mw filter global ex handler
var builder = WebApplication.CreateBuilder(args); // Creates and configures the web application builder
// Under Step 3 (Register Services)
builder.Services.AddScoped<IStudentRepository, StudentRepository>(); // Registers Student repository (new instance per request)
builder.Services.AddScoped<ITutorRepository, TutorRepository>(); // Registers Tutor repository (scoped lifetime)
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>(); // Registers Subject repository

builder.Services.AddScoped<IStudentService, StudentService>(); // Registers Student service (business logic layer)
builder.Services.AddScoped<ITutorService, TutorService>(); // Registers Tutor service
builder.Services.AddScoped<ISubjectService, SubjectService>(); // Registers Subject service

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 
// Registers DbContext and configures PostgreSQL connection using connection string

builder.Services.AddMemoryCache(); // Enables in-memory caching service

builder.Services.AddSingleton<ICacheService, CacheService>(); // Registers custom cache service as singleton (one instance for app lifetime)

// RabbitMQ: bind options, register the publisher (singleton, long-lived connection),
// and run the consumer as a background service.
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddCors(options => // Configures Cross-Origin Resource Sharing (CORS)
{
    options.AddPolicy("AllowAll", policy => // Defines a policy named "AllowAll"
    {
        policy.AllowAnyOrigin() // Allows requests from any domain
              .AllowAnyMethod() // Allows GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader(); // Allows any HTTP headers
    });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingActionFilter>();
});

builder.Services.AddScoped<IStudentService, StudentService>(); // Registers StudentService again (duplicate registration)

// Adds controller support (API endpoints)
builder.Services.AddControllers();

// Adds Swagger/OpenAPI explorer (for API documentation)
builder.Services.AddEndpointsApiExplorer();

// Registers Swagger generator (UI + API docs)
builder.Services.AddSwaggerGen();

var app = builder.Build(); // Builds the application pipeline

if (app.Environment.IsDevelopment()) // Checks if running in Development mode
{
    app.UseSwagger(); // Enables Swagger JSON endpoint
    app.UseSwaggerUI(); // Enables Swagger UI page
}

app.UseCors("AllowAll"); // Applies the CORS policy globally

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();


app.UseAuthorization(); // Enables authorization middleware (checks permissions)

app.MapControllers(); // Maps controller routes to endpoints

app.Run(); // Starts the application

public partial class Program { } // Makes Program class accessible for integration testing