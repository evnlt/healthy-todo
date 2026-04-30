using HealthyTodo.BLL.Abstraction.Services;
using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.BLL.Services;
using HealthyTodo.BLL.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyTodo.BLL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
	{
		services.AddTransient<ITodoListService, TodoListService>();
		
		services.AddTransient<ITodoListValidator, TodoListValidator>();

		return services;
	}
}