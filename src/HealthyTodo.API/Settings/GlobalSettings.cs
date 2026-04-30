using HealthyTodo.DAL.Settings;

namespace HealthyTodo.API.Settings;

public class GlobalSettings
{
    public DatabaseSettings Database { get; set; } = null!;
}