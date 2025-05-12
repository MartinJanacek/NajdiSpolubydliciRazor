using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Helpers;
using NajdiSpolubydliciRazor.Helpers.Interfaces;
using NajdiSpolubydliciRazor.Services;
using NajdiSpolubydliciRazor.Services.Interfaces;

namespace NajdiSpolubydliciRazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Offer/PublishOffer");
                options.Conventions.AuthorizePage("/Offer/Delete");
                options.Conventions.AuthorizePage("/Offer/Update");
                options.Conventions.AuthorizePage("/Offer/UpdateImages");
                options.Conventions.AuthorizePage("/Demand/Delete");
                options.Conventions.AuthorizePage("/Demand/Update");
            })
            .AddMvcOptions(options =>
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        _ => "Toto pole je povinné")
                    );
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Error/";
                });
            builder.Services.AddScoped<CustomCookieAuthenticationEvents>();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ISequentialGuid, SequentialGuid>();
            builder.Services.AddScoped<IOneTimeCode, OneTimeCode>();
            builder.Services.AddScoped<IImageManipulator, ImageManipulator>();
            builder.Services.AddScoped<IImageDirectory, ImageDirectory>();
            builder.Services.AddScoped<IHasher, Hasher>();
            builder.Services.AddScoped<IAuth, Auth>();

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

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy(cookiePolicyOptions);

            app.MapRazorPages();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
