namespace MedUnify.HealthPulseAPI.Infrastructure.Middlewares
{
    public class RequestIdMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Set the TraceIdentifier in the NLog scoped context for logging
            using (NLog.MappedDiagnosticsLogicalContext.SetScoped("TraceIdentifier", context.TraceIdentifier))
            {
                // Adding x-request-id header to the response
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("x-request-id", context.TraceIdentifier);
                    return Task.CompletedTask;
                });

                // Call the next middleware in the pipeline
                await _next(context);
            }
        }
    }
}