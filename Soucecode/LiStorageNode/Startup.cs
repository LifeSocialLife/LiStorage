// <summary>
// {one line to give the library's name and an idea of what it does.}
// </summary>
// <copyright file="Startup.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorageNode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using LiStorage.Services;
    using LiStorage.Services.Node;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        public void ConfigureServices(IServiceCollection services) // , IWebHostEnvironment env)
        {


            services.AddSingleton<FileOperationService>();


            #region RundataService init and data set

            var tmpRundata = new RundataService()
            {
                // Platform = PlatformEnum.None,
            };

            tmpRundata.Folders.PathRuntimes = this.Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            services.AddSingleton(tmpRundata);

            #endregion

            services.AddSingleton<RundataNodeService>();
            services.AddSingleton<CollectionPoolService>();
            services.AddSingleton<StoragePoolService>();
            services.AddSingleton<BlockStorageService>();
            services.AddSingleton<NodeHttpService>();

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
