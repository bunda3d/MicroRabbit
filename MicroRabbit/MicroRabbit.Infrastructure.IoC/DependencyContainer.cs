using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infrastructure.Bus;

using Microsoft.Extensions.DependencyInjection;

//similar to "Startup" class in typ. .NET Core webapp template

namespace MicroRabbit.Infrastructure.IoC
{
  //allows binding of interfaces with 'concrete' types

  public class DependencyContainer
  {
    //use to register services, apps, cross-cutting concerns
    public static void RegisterServices(IServiceCollection services)
    {
      //Domain Bus
      services.AddTransient<IEventBus, RabbitMQBus>();

      //Domain Banking Commands
      services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

      //Application Services
      services.AddTransient<IAccountService, AccountService>();

      //Data Layer
      services.AddTransient<IAccountRepository, AccountRepository>();
      services.AddTransient<BankingDbContext>();
    }
  }
}