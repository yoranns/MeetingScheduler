using MeetingScheduler.Data;
using MeetingScheduler.Data.Services;
using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Providers;
using MeetingScheduler.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



// Adicona os servi�os e contextos de BD
builder.Services.AddDbContext<MeetingSchedulerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MeetingSchedulerContext")));
builder.Services.AddScoped<IMeetingDataService, MeetingDataService>();
builder.Services.AddScoped<IMeetingValidationService, MeetingValidationService>();
builder.Services.AddScoped<IRoomValidationService, RoomValidationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura��o do JSON para ignorar ciclos de refer�ncia
        //!! Projetos mais sofisticados requerem solu��es mais robustas para esse caso !!
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // Conversor para mostrar o status da reuni�o como string
        options.JsonSerializerOptions.Converters.Add(new MeetingStatusJsonConverter());
    });
// Mais info sobre Swagger https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configura a inicializa��o do banco de dados
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
