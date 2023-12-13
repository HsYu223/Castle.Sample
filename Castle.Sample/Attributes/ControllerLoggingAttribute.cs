using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Castle.Sample.Attributes;

/// <summary>
/// Controller Log紀錄標籤
/// </summary>
/// <seealso cref="System.Attribute" />
public class ControllerLoggingAttribute : ActionFilterAttribute
{
    private readonly string _logName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerLoggingAttribute"/> class.
    /// </summary>
    /// <param name="logName">Name of the log.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public ControllerLoggingAttribute(string logName)
    {
        this._logName = logName ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// 執行Action 之前
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var url =
            $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}";
        var parameter = new object();

        if (context.ActionArguments?.Any() ?? false)
        {
            parameter = context.ActionArguments?.FirstOrDefault().Value;
        }

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerLoggingAttribute>>();
        logger.LogInformation($"[{this._logName}] [{context.HttpContext.TraceIdentifier}] Url:{url}");
        logger.LogInformation(
            $"[{this._logName}] [{context.HttpContext.TraceIdentifier}] Query 參數:{JsonSerializer.Serialize(parameter)}");
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// 執行Action 之後
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var repData = new object();
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            repData = ((ObjectResult)context.Result).Value;
        }

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerLoggingAttribute>>();
        logger.LogInformation(
            $"[{this._logName}] [{context.HttpContext.TraceIdentifier}] Return 參數:{JsonSerializer.Serialize(repData)}");
        base.OnActionExecuted(context);
    }
}