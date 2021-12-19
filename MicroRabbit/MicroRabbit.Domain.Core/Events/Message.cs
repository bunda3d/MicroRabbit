using MediatR;

namespace MicroRabbit.Domain.Core.Events
{
  //Any request regarding MediatR can expect a (<bool>) confirmation return status
  public abstract class Message : IRequest<bool>
  {
    //"protected" set as only those inheriting from this abstract class can set
    public string MessageType { get; protected set; }

    protected Message()
    {
      MessageType = GetType().Name;
    }
  }
}