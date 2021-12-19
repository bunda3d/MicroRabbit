using MicroRabbit.Domain.Core.Events;
using System;

namespace MicroRabbit.Domain.Core.Commands
{
  //Command is of type : Message
  public abstract class Command : Message
  {
    //"protected" set as only those inheriting from this abstract class can set
    public DateTime TimeStamp { get; protected set; }

    protected Command()
    {
      TimeStamp = DateTime.Now;
    }
  }
}