using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Middlewares
{
    public class ExceptionMiddleware : IExceptionFilter
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly Dictionary<Type, Func<Exception, IActionResult>> _exceptions;

        public ExceptionMiddleware(Dictionary<Type, Func<Exception, IActionResult>> exception)
        {
            _exceptions = exception;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Error while processing request");

            if (context.Exception.GetType() == typeof(DbUpdateException))
            {

                if (context.Exception.InnerException is PostgresException sqlEx && (sqlEx.SqlState == "2601" || sqlEx.SqlState == "2627" || sqlEx.SqlState == "23505"))
                {
                    context.Result = new ConflictObjectResult("Entity already exists");
                }
            }

            if (_exceptions.TryGetValue(context.Exception.GetType(), out var exception))
            {
                context.Result = exception(context.Exception);
            }
        }
    }
}