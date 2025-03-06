using Microsoft.Extensions.Configuration;
using System.IO;

public class DatabaseConfig
{
    public static string GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return config.GetConnectionString("DefaultConnection");
    }
}
