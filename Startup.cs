namespace Middleware
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app)
        {
            //Typical middleware order

            //Catches exception thrown in following middlewares
            //app.UseExceptionHandler();
            
            //Adds Strict-Transport-Security header
            app.UseHsts();
            
            //Redirects HTTP requests to HTTPS
            app.UseHttpsRedirection();

            //Returns static files and short-circuits further request processing
            app.UseStaticFiles();

            //This is where routing decisions are made. Should be used in conjunction with UseEndpoints middleware
            app.UseRouting();

            //Setup CORS
            app.UseCors();

            //Authentication
            app.UseAuthentication();

            //Authorization
            app.UseAuthorization();

            //Custom middlewares go here

            //Make sure request lands to correct Controller/Action
            app.UseEndpoints(app => 
            {
                app.MapGet("/WeatherForecast", context =>
                {
                    string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                    var output = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();

                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(output);
                });
            });
        }
    }
}
