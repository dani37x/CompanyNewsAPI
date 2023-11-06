using CompanyNewsAPI.Models;
using System.Text.Json;

namespace CompanyNewsAPI.Services
{
    public class ExceptionHandlerService
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<ExceptionHandlerService> _logger;

        //public ExceptionHandlerService(RequestDelegate next, ILogger<ExceptionHandlerService> logger)
        public ExceptionHandlerService(RequestDelegate next)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                //_logger.LogError(e, e.Message);
                Error errorData = new Error { Message = e.ToString() };
                await FileService.AppendAllTextAsync(@"errors.json", "\n" + JsonSerializer.Serialize(errorData) + ",");
            }
        }
    }
}
