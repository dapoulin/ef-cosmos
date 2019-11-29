using System;

namespace Ef.Cosmos
{
    public interface IDbSettings
    {
        Uri Uri { get; set; }
        string PrimaryKey { get; set; }
        string DatabaseName { get; set; }
        string ContainerName { get; set; }
        bool EnableDetailedErrors { get; set; }
        bool EnableSensitiveDataLogging { get; set; }
    }
}
