using Microsoft.EntityFrameworkCore;
using PowerPoint2.Infrastructure;
using PowerPoint2.Infrastructure.Services;
using PowerPoint2.Presentation.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureSerilog(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.ApplyMigrations();
app.MapHub<PresentationHub>("/presentationHub");

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error");
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();
app.MapControllers();


app.Run();
