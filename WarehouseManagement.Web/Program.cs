using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application;
using WarehouseManagement.Infrastructure;
using WarehouseManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope()) // �������� ��, ���� ��� �� ��������� �� ������ �����������
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("-���������� �������� ��...");
        db.Database.Migrate();
        Console.WriteLine("-�������� �� ��������� �������.");
    }
    catch(Exception ex)
    {
        Console.WriteLine($"-�������� �� ��������� � ��������: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
