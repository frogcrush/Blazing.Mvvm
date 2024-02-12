using Blazing.Mvvm;
using Blazing.Mvvm.Infrastructure;
using Blazing.Mvvm.Sample.WebApp.Client.Pages;
using Blazing.Mvvm.Sample.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add Blazing.Mvvm
builder.Services.AddMvvm(options =>
{
    options.HostingModelType = BlazorHostingModelType.WebApp;

    // Auto discovery is the default behaviour.
    // However, you can specify the assemblies to scan for view models, which is useful when you want to scan only specific assemblies and also helps to reduce the startup time.
    //options.RegisterViewModelsFromAssemblyContaining<Counter>();
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
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
