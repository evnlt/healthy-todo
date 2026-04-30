namespace HealthyTodo.DAL.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}