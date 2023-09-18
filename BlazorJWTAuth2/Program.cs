using Blazored.LocalStorage;

using BlazorJWTAuth2;
using BlazorJWTAuth2.Auth;
using BlazorJWTAuth2.Data;
using BlazorJWTAuth2.DataAccess;
using BlazorJWTAuth2.Model;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

//👇 추가 ========================
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthenticationCore();
// ===============================

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<UserAccountService>(); //👈 추가: Id존재유무 등을 확인하기 위한 작업 실행

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //👈 추가
app.UseAuthorization(); //👈 추가

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
