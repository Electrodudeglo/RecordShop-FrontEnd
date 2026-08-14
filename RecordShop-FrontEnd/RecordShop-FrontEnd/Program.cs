using RecordShop_FrontEnd.Components;
using RecordShop_FrontEnd.Interfaces;
using RecordShop_FrontEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSingleton<IToastService, ToastService>();

// Custom Class Injections
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RecordService>();


// Backend API
builder.Services.AddHttpClient<RecordService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5125/");
});

// Auth API
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5125/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RecordShop_FrontEnd.Client._Imports).Assembly);

app.Run();
