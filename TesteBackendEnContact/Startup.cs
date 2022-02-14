using FluentMigrator.Runner;
using FuncionariosAPIService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TesteBackendEnContact", Version = "v1" });
            });

            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSQLite()
                        .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(typeof(Startup).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());


            // #INFO: Configurar aqui os serviços.
            services.AddSingleton(new DatabaseConfig { ConnectionString = Configuration.GetConnectionString("DefaultConnection") });

            services.AddScoped<IContactBookRepository, ContactBookRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder/*IAppBuilder */app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TesteBackendEnContact v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
            //app.


            // Owin 
            //var config = new HttpConfiguration();

            //config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //    );
            //app.UseCors(CorsOptions.AllowAll);
            //AtivarGeracaoTokenAcesso(app);
            //app.UseWebApi(config);
        }

        //private void AtivarGeracaoTokenAcesso(IAppBuilder app)
        //{
        //    var opcoesConfiguracaoToken = new OAuthAuthorizationServerOptions()
        //    {
        //        AllowInsecureHttp = true,
        //        TokenEndpointPath = new PathString("/token"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
        //        Provider = new ProviderDeTokensDeAcesso()
        //    };
        //    app.UseOAuthAuthorizationServer(opcoesConfiguracaoToken);
        //    app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        //}
    }
}
