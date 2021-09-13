using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
	public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
	{
		private readonly IEventBus _bus;

		public TransferCommandHandler(IEventBus bus)
		{
			_bus = bus;
		}

		public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
		{
			//publish event to RabbitMQ
			return Task.FromResult(true);
		}
	}
}