using System;

namespace MicroRabbit.Domain.Core.Events
{
  //Base class entity (does not extend anything)
  public abstract class Event
  {
    public DateTime Timestamp { get; protected set; }

    protected Event()
    {
      Timestamp = DateTime.Now;
    }
  }
}