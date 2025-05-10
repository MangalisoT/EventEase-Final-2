using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventEase_Final.Data;
using EventEase_Final.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the database context with SQL Server
builder.Services.AddDbContext<EventEase_FinalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventEase_FinalContext")
                         ?? throw new InvalidOperationException("Connection string 'EventEase_FinalContext' not found.")));

// Register services for the application
builder.Services.AddControllersWithViews();

// Register the BlobService if needed for image uploading
builder.Services.AddSingleton<BlobService>();

var app = builder.Build();

// Initialize seed data 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); 
app.UseStaticFiles(); // Serve static files (CSS, JS, images, etc.)

app.UseRouting(); 

app.UseAuthorization();

// Set up the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Start the app
