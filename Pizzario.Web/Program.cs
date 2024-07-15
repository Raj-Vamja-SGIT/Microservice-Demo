using Pizzario.Web.Service;
using Pizzario.Web.Service.IService;
using Pizzario.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient("PizzarioApi")
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        });
}
else
{
    builder.Services.AddHttpClient();
}
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IProductService, ProductService>();

StaticDetails.CouponApiBase = builder.Configuration["ServiceUrls:CouponApi"];
StaticDetails.ProductApiBase = builder.Configuration["ServiceUrls:ProductApi"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
