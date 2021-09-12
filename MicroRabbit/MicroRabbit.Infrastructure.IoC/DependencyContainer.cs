using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infrastructure.Bus;

using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infrastructure.IoC
{
	//allows binding of interfaces with 'concrete' types

	public class DependencyContainer
	{
		//use method to register the dependency container for other classes/projects/apps to use the services
		public static void RegisterServices(IServiceCollection services)
		{
			//Domain Bus
			services.AddTransient<IEventBus, RabbitMQBus>();
		}
	}
}