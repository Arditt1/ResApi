using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services;
using ResApi.DTA.Services.Shared;
using ResApi.Extentions;
using ResApi.Models;

namespace ResApi
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
            services.AddLogging();

            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("restApp")));

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<ICategoryMenu, CategoryMenuService>();   
            services.AddScoped<IEmployee, EmployeeService>();
            services.AddScoped<IMenuItem, MenuItemService>();
            services.AddScoped<IOrderDetail, OrderDetailService>();
            services.AddScoped<IOrder, OrderService>();
            services.AddScoped<IPermission, PermissionService>();
            services.AddScoped<IRole, RoleService>();
            services.AddScoped<ITable, TableService>();
            services.AddScoped<ITableWaiter, TableWaiterService>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ResApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.Run();
        }
    }
}
