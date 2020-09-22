using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace SCM2020___Server
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
            //Indicate the database
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ControlDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Add identity
            //Which will be created tables on database for users control
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            ConsumptionProduct.ValueChanged += ConsumptionProduct_ValueChanged;

            services.AddControllersWithViews();

            //Add Authentication to use protected access
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                 ClockSkew = TimeSpan.Zero
             });
        }

        private void ConsumptionProduct_ValueChanged(ConsumptionProduct ConsumptionProduct)
        {
            //Armazena no banco de dados enquanto n�o tiver enviado para todos os funcion�rios.
            //Ap�s ter enviado a todos, pode apagar
            int SKU = ConsumptionProduct.Id;
            string message = string.Empty;
            //.Where(Roles == "SCM")
            if (ConsumptionProduct.Stock < ConsumptionProduct.MininumStock)
            {
                //Envia aos clientes com a role SCM alertando material com pouco estoque
                message = $"Produto {ConsumptionProduct.Id} {ConsumptionProduct.Description} est� com estoque deficiente.";
            }

            if (ConsumptionProduct.Stock > ConsumptionProduct.MaximumStock)
            {
                //Envia aos clientes com a role SCM alertando material com muito estoque
                message = $"Produto {ConsumptionProduct.Id} {ConsumptionProduct.Description} est� com estoque excedente.";
            }
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                    await context.HttpContext.Response.WriteAsync("Access Denied. Please, sign in.");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
