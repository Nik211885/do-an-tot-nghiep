namespace Infrastructure.Configurations;

internal record DatabaseConnectionString(string Master, string[] Slaves);
