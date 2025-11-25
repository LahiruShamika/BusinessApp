using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http.Features;
using BusinessApp.Services;
using BusinessApp.Utilities;

var builder = WebApplication.CreateBuilder(args);

// MVC/Razor views
builder.Services.AddControllersWithViews();

// Increase default multipart/form-data limits for uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
    options.MemoryBufferThreshold = 1024 * 1024; // 1 MB
});

// Register app services (concrete types from Services/Utilities folders)
builder.Services.AddSingleton<ExcelService>();
builder.Services.AddSingleton<OcrService>();
builder.Services.AddSingleton<PaymentComparer>();
builder.Services.AddSingleton<TesseractWrapper>();

// Provide a shared upload folder path via DI
var uploadFolder = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
Directory.CreateDirectory(uploadFolder);
builder.Services.AddSingleton(new UploadSettings { Path = uploadFolder });

// Optional: bind appsettings if needed
// builder.Configuration.Bind("SomeSection", someOptions);

// Build app
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

// Default route (HomeController -> Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Simple DTO for upload settings injected into controllers/services
public class UploadSettings
{
    public string Path { get; set; } = string.Empty;
}