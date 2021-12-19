using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Bus
{
  public interface IEventBus
  {
    //Generic (<T>) to send any type of obj, but
    //T has to be of Mediatr type "Command" (T:Command)
    Task SendCommand<T>(T command) where T : Command;

    //Publish events that are Type T = Event
    //"@" prefix as "event" is C# reserved keyword
    void Publish<T>(T @event) where T : Event;

    //Allow for subscription to Events, incl. those with Event Handlers (<TH>)
    void Subscribe<T, TH>()
      where T : Event
      where TH : IEventHandler<T>; //handler of type T
  }
}