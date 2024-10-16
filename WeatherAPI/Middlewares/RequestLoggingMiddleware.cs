using log4net;

namespace WeatherAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RequestLoggingMiddleware));

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next=next;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.Info($"Request: {context.Request.Method} {context.Request.Path}");

            try
            {
                await _next(context);
                _logger.Info($"Response status: {context.Response.StatusCode}");

                if (context.Response.StatusCode >= 400)
                {
                    var originalBodyStream = context.Response.Body;

                    // ToDo: Logging the Response body
                    // - Generate a standard ResponseModel response

                    _logger.Error($"Error response: {context.Response.Body}");
                }
            }
            catch (Exception ex)
            {
                // ToDo: Generate a standard ResponseModel response
                
                _logger.Error($"Exception on the current Request: {ex}");
                throw;
            }
        }
        
    }
}
