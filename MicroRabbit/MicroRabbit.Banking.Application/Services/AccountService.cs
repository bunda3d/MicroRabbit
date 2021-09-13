using System.Collections.Generic;

using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepo;

		public AccountService(IAccountRepository accountRepo)
		{
			_accountRepo = accountRepo;
		}

		public IEnumerable<Account> GetAccounts()
		{
			return _accountRepo.GetAccounts();
		}
	}
}