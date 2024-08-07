    using System.Text;
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
    using ResApi.Hubs;
    using ResApi.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

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
                services.AddScoped<IAuth, AuthService>();

                services.AddSignalR();
                services.AddControllers();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Res API",
                    });
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id="Bearer",
                                }
                            },
                            new string[]{ }
                        }
                    });


                });

                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder => builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });

                services.AddHttpContextAccessor();


                var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
                services.AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = Configuration["Jwt:Issuer"],
                                ValidAudience = Configuration["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                            };
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

                app.UseCors("CorsPolicy");

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<OrderHub>("/orderHub");
                });

                //app.Run();
            }
        }
    }
