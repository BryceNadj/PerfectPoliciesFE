using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Models.QuizModels;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Set up a central configuration for the HttpClient
            services.AddHttpClient("ApiClient", c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiUrl"]);
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // create an in memory Database for storing session content
            services.AddDistributedMemoryCache();

            // Define the session parameters
            services.AddSession(opts =>
            {
                opts.IdleTimeout = TimeSpan.FromMinutes(3);
                opts.Cookie.HttpOnly = true;
                opts.Cookie.IsEssential = true;
            });

            services.AddSingleton<RouteValuesHelper>();
            services.AddHttpContextAccessor();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#if TEST
            services.AddSingleton<TestDatabase>();
            services.AddScoped<IApiRequest<Quiz>, InMemoryRequest<Quiz>>();
            services.AddScoped<IApiRequest<Question>, InMemoryRequest<Question>>();
            services.AddScoped<IApiRequest<Option>, InMemoryRequest<Option>>();
            services.AddScoped<IApiRequest<UserInfo>, InMemoryRequest<UserInfo>>();
#else
            services.AddScoped<IApiRequest<Quiz>, ApiRequest<Quiz>>();
            services.AddScoped<IApiRequest<Question>, ApiRequest<Question>>();
            services.AddScoped<IApiRequest<Option>, ApiRequest<Option>>();
            services.AddScoped<IApiRequest<UserInfo>, ApiRequest<UserInfo>>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
