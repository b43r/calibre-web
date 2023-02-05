/*
 * CalibreWeb
 * 
 * Copyright (C) 2018..2021 by Simon Baer
 *
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 * 
 */

using System.Globalization;

using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using CalibreWeb.Models;
using CalibreWeb.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<LocService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc();

// German and English localization is supported, whereas German is the default
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("de"),
            };

        options.DefaultRequestCulture = new RequestCulture(culture: "de", uiCulture: "de");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    });

string db = Path.Combine(builder.Configuration["Calibre:CataloguePath"], "metadata.db");
if (!File.Exists(db))
{
    throw new Exception($"Calibre DB not found at \"{db}\".");
}

// Open Calibre DB in readonly mode so Calibre can run while web is running
var connection = $"Data Source={db};Mode=ReadOnly;";
builder.Services.AddDbContext<CalibreContext>(options => options.UseSqlite(connection));

builder.Services.AddRazorPages();

var app = builder.Build();

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
