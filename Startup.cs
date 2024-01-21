using Microsoft.AspNetCore.Builder;

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

            //Anonymous middleware
            app.Use(
                async (httpCtx, next) =>
                {
                    //Do some work with httpcontext.Request
                    await next.Invoke();
                    // Do something with httpContext.Response (Logging etc.)
                });


            //Use - Inserts middleware in pipeline. This usecase is of short-circuiting a request based on path-match
            app.Use(
                async (httpCtx, next) =>
                {
                    if (httpCtx.Request.Path.StartsWithSegments("/short-circuit"))
                    {
                        await httpCtx.Response.WriteAsync("Request terminated by short-circuit middleware.");
                    }
                    else
                    {
                        await next();
                    }
                });

            //UseWhen - More focused on conditionally executing request based on HttpContext
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/use-when"),
                branch =>
                {
                    branch.Use(async (context, next) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/use-when"))
                        {
                            await context.Response.WriteAsync("Request terminated by use-when middleware");
                        }
                        else
                        {
                            await next();
                        }
                    });
                });

            //Map - More focused on conditionally executing middleware based on Request Path. - Need to troubleshoot this
            app.Map("/map-test",
                branch =>
                {
                    branch.Use(async (context, next) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/map-test"))
                        {
                            await context.Response.WriteAsync("Request terminated by map middleware");
                        }
                        else
                        {
                            await next();
                        }
                    });
                });

            //Map more focused on Path.
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/map-when"),
                branch =>
                {
                    branch.Use(async (context, next) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/map-when"))
                        {
                            await context.Response.WriteAsync("Request terminated by map-when middleware");
                        }
                        else
                        {
                            await next();
                        }
                    });
                });

            //Enabling this will terminate all requests here if route is /ok or /internal-server-error
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Request terminates here");
            });

            app.UseEndpoints(app =>
                {
                    app.MapGet("/ok", context =>
                    {
                        return context.Response.WriteAsync($"Request received: {context.Request.Path.Value}");
                    });

                    app.MapGet("/internal-server-error", context =>
                    {
                        context.Response.StatusCode = 500;
                        return context.Response.WriteAsync("Something went wrong");
                    });
                });
        }
    }
}
