using MicroRabbit.Domain.Core.Commands;

namespace MicroRabbit.Banking.Domain.Commands
{
	//Base TransferCommand class
	//abstract so only those who can extend base class can set property value
	public abstract class TransferCommand : Command
	{
		public int SourceAcct { get; protected set; }
		public int DestinationAcct { get; protected set; }
		public decimal Amount { get; protected set; }
	}
}