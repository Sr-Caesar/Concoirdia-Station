using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Repositories;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.Data.Services;
using Microsoft.EntityFrameworkCore;
using ConcordiaStation.WebApp.SecurityServices.Interfaces;
using ConcordiaStation.WebApp.SecurityServices;
using ConcordiaStation.SyncApp;
using ConcordiaStation.SyncApp.Endpoints.Gateways;
using ConcordiaStation.WebApp;


  var builder = WebApplication.CreateBuilder(args);
  ServiceKey.SetKey();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IServiceToken>(new ServiceToken(ServiceKey.GetKey()));
builder.Services.AddScoped<IServiceKey, ServiceKey>();
builder.Services.AddScoped<IServiceComment, ServiceComment>();
builder.Services.AddScoped<IServicePhase, ServicePhase>();
builder.Services.AddScoped<IServiceExperiment, ServiceExperiment>();
builder.Services.AddScoped<IServiceScientist, ServiceScientist>();
builder.Services.AddScoped<IServiceCredential, ServiceCredential>();

builder.Services.AddScoped<IRepositoryExperiment, RepositoryExperiment>();
builder.Services.AddScoped<IRepositoryPhase, RepositoryPhase>();
builder.Services.AddScoped<IRepositoryScientist, RepositoryScientist>();
builder.Services.AddScoped<IRepositoryComment, RepositoryComment>();
builder.Services.AddScoped<IRepositoryCredential, RepositoryCredential>();

builder.Services.AddSingleton<SynchronizerBackground>();
builder.Services.AddHostedService(provider => provider.GetService<SynchronizerBackground>());

builder.Services.AddDbContext<ConcordiaLocalDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

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
