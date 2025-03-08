using Repository;
using Repository.Interfaces;
using Repository.Interfaces.Users;
using Repository.Repositories;
using Repository.Repositories.Users;
using Service.Interfaces;
using Service.Interfaces.Users;
using Service.Services;
using Service.Services.Users;
using Service.Utility;

namespace Web
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string cannot be null!");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddScoped<DataAccess>(x => new DataAccess(_connectionString));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ServiceValidator>();
            services.AddScoped<ExceptionHandler>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IUserService, UserService>();

            services.AddControllers(); // Register controllers
        }

        public void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting(); // Ensure routing is set up
            app.UseAuthorization();

            app.MapControllers(); // Map attribute-routed controllers
        }
    }
}