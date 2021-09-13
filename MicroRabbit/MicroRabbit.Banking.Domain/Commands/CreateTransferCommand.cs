using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.Commands
{
	public class CreateTransferCommand : TransferCommand
	{
		public CreateTransferCommand(int sourceAcct, int destinationAcct, decimal amount)
		{
			SourceAcct = sourceAcct;
			DestinationAcct = destinationAcct;
			Amount = amount;
		}
	}
}