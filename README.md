# Middleware

	- Built-in middlewares
		- ASP.NET Core ships with built-in middlewares
		- Refer here for middleware sequence - https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0

	- Custom middlewares
		- We can create our own custom middlewares

	- 3 main methods to execute middleware
		- Use
		- Run
		- Map
    - Use
	    - Purpose: Used to add middleware components to the pipeline.
		    - Usage: Sequentially adds middleware components to the pipeline.
		    - Example:
        
                app.UseMiddleware1();
                app.UseMiddleware2();
                app.UseMiddlewareN();
        
            - Behavior: Calls the next middleware in the pipeline by invoking the next delegate.
    - Map:
    - Purpose: Defines a branch of the pipeline based on the request path.
        - Usage: Allows you to create separate middleware pipelines for different URL paths.
        - Example:

            app.Map("/path1", branch =>
            {
                // Middleware for /path1
            });

            app.Map("/path2", branch =>
            {
                // Middleware for /path2
            });
            
            - Behavior: Branches the pipeline based on the specified path. The branch that matches the request path will be executed.
    - Run:

    - Purpose: Used to terminate the pipeline for a specific branch based on a condition.
    - Usage: Typically used for short-circuiting the pipeline when no further middleware processing is required in that branch.
    - Example:

        app.Run(context =>
        {
            // Your logic to terminate the request
        });
        
    - Behavior: Does not call the next middleware in the pipeline. It's a way to terminate the request processing for a specific branch.
    - In summary, 
        - Use is for adding middleware components
        - Map is for branching the pipeline based on request paths
        - Run is for terminating the pipeline for a specific branch. 
    
    - Depending on your application's requirements, you may use a combination of these methods to construct a middleware pipeline that suits your needs.
	
    - Routes
		- /short-circuit
		- /use-when
		- /map
		- /map-when
        - /ok
        - /internal-server-error