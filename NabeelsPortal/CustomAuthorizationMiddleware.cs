namespace NabeelsPortal
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomAuthorizationMiddleware> _logger;

        public CustomAuthorizationMiddleware(RequestDelegate next, ILogger<CustomAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Proceed with the next middleware
            await _next(context);

            // Check for 401 Unauthorized status code
            if (context.Response.StatusCode == 401)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\": \"First log in before you can perform this action.\"}");
            }
            // Check for 403 Forbidden status code
            else if (context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\": \"You do not have permission to perform this action.\"}");
            }
        }
    }

}

