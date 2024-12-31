using Azure;
using Blazor_OpenAI.Components;
using Blazor_OpenAI.Services;
using Azure.AI.OpenAI;
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Register the QnAServiceClient
builder.Services.AddSingleton<QnAServiceClient>();

var app = builder.Build();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
