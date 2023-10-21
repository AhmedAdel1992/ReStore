using API.Data;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSqlConnection"));
});
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});

app.UseAuthorization();

app.MapControllers();

//Another way of creating a Database
//awel 7aga bn3ml scope 3shan e7na m7tagen el scope da 3shan n7ot el service bta3et el context gowa 7aga 3shan nst5dmha
var scope = app.Services.CreateScope();
//b3d kda bn7ot el service fe variable 
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
//w bn3ml kman variable lel logger 3shan y log ay error mmkn y7sl w bndelo el Program 3shan hwa da el mkan elly 3yzeno 
//y log mno ay errors ht7sl
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}
catch(Exception ex)
{
    logger.LogError(ex,"A problem occured during migration");
}

app.Run();
