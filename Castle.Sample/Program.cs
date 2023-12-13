using Castle.DynamicProxy;
using Castle.Sample.Infrastructure;
using Castle.Sample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new ProxyGenerator());
builder.Services.AddScoped<IInterceptor, LogInterceptor>();
builder.Services.AddTransient<SampleService>();

builder.Services.AddService<ISampleService, SampleService>(ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Dependency Injection Extensions
/// </summary>
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddService<TService, TImplementation>(this IServiceCollection services,
        ServiceLifetime lifetime)
    {
        return services.AddService(typeof(TService), typeof(TImplementation), lifetime);
    }

    public static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implType,
        ServiceLifetime lifetime)
    {
        services.Add(new ServiceDescriptor(implType, implType, lifetime));

        Func<IServiceProvider, object> factory = (provider) =>
        {
            var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
            var target = provider.GetService(implType);

            var interceptors = provider.GetServices<IInterceptor>().ToArray();

            var proxy = proxyGenerator.CreateInterfaceProxyWithTarget(serviceType, target, interceptors);

            return proxy;
        };
        var serviceDescriptor = new ServiceDescriptor(serviceType, factory, lifetime);
        services.Add(serviceDescriptor);

        return services;
    }
}