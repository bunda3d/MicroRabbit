using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Banking.Domain.Events
{
  public class TransferCreatedEvent : Event
  {
    public int SourceAccount { get; private set; }
    public int DestinationAccount { get; private set; }
    public decimal TransferAmount { get; private set; }

    public TransferCreatedEvent(int from, int to, decimal amount)
    {
      SourceAccount = from;
      DestinationAccount = to;
      TransferAmount = amount;
    }
  }
}