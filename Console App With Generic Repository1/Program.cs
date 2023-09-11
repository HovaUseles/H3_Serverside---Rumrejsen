using Console_App_With_Generic_Repository1.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Setting up Depencency Injection
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddSingleton(typeof(IGenericRepository), typeof());

using IHost host = builder.Build();

using IServiceScope serviceScope = host.Services.CreateScope();
IServiceProvider provider = serviceScope.ServiceProvider;

// Getting injected services
//var userRepository = provider.GetRequiredService<IGenericRepository>();

