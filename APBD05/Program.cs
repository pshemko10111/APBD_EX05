using APBD05.Context;
using APBD05.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services (formerly in Startup.ConfigureServices)
builder.Services.AddDbContext<MasterContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=localhost;Initial Catalog=master;User Id=sa;Password=Testowe123;Encrypt=False"));
});
builder.Services.AddScoped<IDBService, DBService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Optional, for Minimal APIs
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "APBD05", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline (formerly in Startup.Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "APBD05 v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
