using Final.Data;
using Final.Models;
using Final.Repository;
using Final.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

// Thêm dịch vụ IHttpContextAccessor vào container dịch vụ
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<SessionManager>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //prevent cycle unlimit
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // ignore $id, $values,...    
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // prevent camel case
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGenericRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IGenericRepository<Borrower>, BorrowerRepository>();
builder.Services.AddScoped<IGenericRepository<History>,HistoryRepository>();
builder.Services.AddScoped<IGenericRepository<BorrowItem>,BorrowItemRepository>();


builder.Services.AddScoped<IItemServices,ItemServices>();
builder.Services.AddScoped<IBorrowerServices, BorrowerServices>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
