using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoindeskApi.Repositories;

public class MyCommandInterceptor : DbCommandInterceptor
{
    private readonly ILogger<MyCommandInterceptor> _logger;

    public MyCommandInterceptor(ILogger<MyCommandInterceptor> logger)
    {
        _logger = logger;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        _logger.LogInformation($"{nameof(MyCommandInterceptor)}.{nameof(ReaderExecuting)} Command: {command.CommandText}");
        // ManipulateCommand(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{nameof(MyCommandInterceptor)}.{nameof(ReaderExecutingAsync)} Command: {command.CommandText}");
        // ManipulateCommand(command);
    
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void ManipulateCommand(DbCommand command)
    {
        if (command.CommandText.StartsWith("-- Use hint: robust plan", StringComparison.Ordinal))
        {
            command.CommandText += " OPTION (ROBUST PLAN)";
        }
    }
}