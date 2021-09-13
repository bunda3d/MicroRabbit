using System.Collections.Generic;

using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepo;
		private readonly IEventBus _bus;

		public AccountService(IAccountRepository accountRepo, IEventBus bus)
		{
			_accountRepo = accountRepo;
			_bus = bus;
		}

		public IEnumerable<Account> GetAccounts()
		{
			return _accountRepo.GetAccounts();
		}

		public void Transfer(AccountTransfer accountTransfer)
		{
			//Create new transfer
			var createTransferCommand = new CreateTransferCommand(
				accountTransfer.AccountSource,
				accountTransfer.AccountDestination,
				accountTransfer.TransferAmount
			);
			//Send transfer msg thru the bus
			_bus.SendCommand(createTransferCommand);
		}
	}
}