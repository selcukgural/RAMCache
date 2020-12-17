RAMCache - a simple cache in-memory for .Net
========================================


Features
--------
RAMCache is a [NuGet library](https://www.nuget.org/packages/RAMCache) that you can add in to your .Net or .Net Core project.

Examples
--------
###### Web Api example usage:

``` csharp
services.AddRAMCache(e => e.MaximumItemCount = 5000);
```


```csharp
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddRAMCache(e => e.MaximumItemCount = 5000);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
```

Add RAMCache to services and you can use it by injecting

```csharp
private readonly IRAMCache _ramCache;
public WeatherForecastController(IRAMCache ramCache)
{
    _ramCache = ramCache;
}
```

###### For other application types example usage:

```csharp
public static readonly RAMCache RamCache = new RAMCache(new RAMCacheServiceOptions{MaximumItemCount = 5000});
```