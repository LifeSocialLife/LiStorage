using LiStorage.Services;
using LiStorage.Services.Node;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiStorageNode
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // , IWebHostEnvironment env)
        {


            services.AddSingleton<FileOperationService>();


            #region RundataService init and data set

            var tmpRundata = new RundataService()
            {
                
                // Platform = PlatformEnum.None,
            };

            tmpRundata.Folders.PathRuntimes = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            
            services.AddSingleton(tmpRundata);

            #endregion

            services.AddSingleton<RundataNodeService>();
            services.AddSingleton<StoragePoolService>();


            services.AddRazorPages();

            services.AddHostedService<NodeWorker>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
