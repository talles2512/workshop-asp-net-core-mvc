using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

namespace SalesWebMvc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMvcContext>(options => //Configurando a conexão com o banco. o Método UseMySql() tem como argumento a "string de conexão"
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                        builder.MigrationsAssembly("SalesWebMvc")));
            services.AddScoped<SeedingService>(); //Adicionandos os serviços às configurações da solução
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            var enUS = new CultureInfo("en-US"); //english - United States
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUS), //Localização padrão da aplicação
                SupportedCultures = new List<CultureInfo> { enUS }, //Lista de Culturas suportadas
                SupportedUICultures = new List<CultureInfo> { enUS }
            };

            app.UseRequestLocalization(localizationOptions); //Aplicação recebe a configuração de localização

            if (env.IsDevelopment()) //Caso seja desenvolvedor
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed(); //Iniciando a população dos registros de teste no Banco de Dados
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"); //Definindo a estrutura do site e a View Index como sendo a pagina Home
            });
        }
    }
}
