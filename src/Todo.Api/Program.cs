using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Todo.Api.Middlewares;
using Todo.Api.Services;
using Todo.Application.Commands.Requests;
using Todo.Domain.Commands;
using Todo.Domain.Commands.Handlers;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Contracts.Services;
using Todo.Domain.Events.Handlers;
using Todo.Domain.Events.Notifications;
using Todo.Domain.Settings;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRequestHandler<CreateTodoItemRequest, CommandResponse>, TodoItemCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteTodoItemRequest, CommandResponse>, TodoItemCommandHandler>();
builder.Services.AddScoped<IRequestHandler<MarkAsDoneTodoItemRequest, CommandResponse>, TodoItemCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateTodoItemRequest, CommandResponse>, TodoItemCommandHandler>();

builder.Services.AddScoped<INotificationHandler<CreatedTodoItemNotification>, TodoItemEventHandler>();
builder.Services.AddScoped<INotificationHandler<DeletedTodoItemNotification>, TodoItemEventHandler>();
builder.Services.AddScoped<INotificationHandler<MarkedAsDoneTodoItemNotification>, TodoItemEventHandler>();
builder.Services.AddScoped<INotificationHandler<UpdatedTodoItemNotification>, TodoItemEventHandler>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dataContext.Database.IsRelational() && dataContext.Database.GetPendingMigrations().Any())
        dataContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpMetrics();
app.UseMetricServer();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.Run();
