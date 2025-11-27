using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using BusinessApp.Services;
using BusinessApp.Utilities;

var builder = WebApplication.CreateBuilder(args);

// read tessdata path from configuration (appsettings.json)
// add a default fallback "tessdata" if not set
var tessDataPath = builder.Configuration.GetValue<string>("Tesseract:TessDataPath") ?? "tessdata";

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register services and provide tessDataPath where required
builder.Services.AddSingleton<TesseractWrapper>(sp => new TesseractWrapper(tessDataPath));
builder.Services.AddSingleton<OcrService>(sp => new OcrService(tessDataPath));

// ExcelService and PaymentComparer as scoped (or singleton if you prefer)
builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<PaymentComparer>();

// Optional: configure form/file upload limits if you expect large files
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();