using Microsoft.EntityFrameworkCore;
using PunjabCleaningServices.Data;
using PunjabCleaningServices.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpContextAccessor();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("CONNECTION STRING:");
Console.WriteLine("CONN = " + conn);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(conn));


builder.Services.AddAuthentication();
builder.Services.AddAuthorization(); // This won't complain anymore!

var app = builder.Build();

/*// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.Initialize(app);

app.Run();