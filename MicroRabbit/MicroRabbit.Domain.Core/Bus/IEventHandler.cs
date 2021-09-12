using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
	//handle any type of generic event (in <TEvent>)
	//interface implements IEventHandler where incoming Event is of TEvent type
	public interface IEventHandler<in TEvent> : IEventHandler
		where TEvent : Event
	{
		//can handle events of TEvents type
		Task Handle(TEvent @event);
	}

	public interface IEventHandler
	{
	}
}