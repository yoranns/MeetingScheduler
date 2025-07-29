using MeetingScheduler.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adicona os serviços de BD
builder.Services.AddDbContext<MeetingSchedulerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MeetingSchedulerContext")));

builder.Services.AddControllers();
// Mais info sobre Swagger https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configura a inicialização do banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MeetingSchedulerContext>();
    await dbContext.Database.MigrateAsync();
    await dbContext.SeedDataAsync(); 
}


// Inicia o Swagger se o ambiente for de Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
