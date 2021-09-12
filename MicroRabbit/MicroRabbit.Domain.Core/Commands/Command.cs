using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroRabbit.Domain.Core.Events;

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