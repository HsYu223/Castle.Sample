using Castle.DynamicProxy;
using Castle.Sample.Attributes;
using System.Reflection;
using System.Text.Json;

namespace Castle.Sample.Infrastructure
{
    /// <summary>
    /// Log攔截器
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        private readonly ILogger<LogInterceptor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInterceptor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LogInterceptor(ILogger<LogInterceptor> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.GetConcreteMethod();

            method = invocation.InvocationTarget.GetType().GetMethod(method.Name);

            var attribute = method.GetCustomAttribute<ServicesLoggingAttribute>();

            if (attribute != null)
            {
                var parameter = JsonSerializer.Serialize(invocation.Arguments);
                this._logger.LogInformation(
                    $"[{attribute.LogName}] [{this._httpContextAccessor.HttpContext?.TraceIdentifier}] Input 參數:{parameter}");

                invocation.Proceed();

                var result = JsonSerializer.Serialize(invocation.ReturnValue);
                this._logger.LogInformation(
                    $"[{attribute.LogName}] [{this._httpContextAccessor.HttpContext?.TraceIdentifier}] Output 結果:{result}");
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
