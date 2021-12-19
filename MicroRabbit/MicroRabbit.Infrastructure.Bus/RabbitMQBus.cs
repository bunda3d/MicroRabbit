using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infrastructure.Bus
{
  //Sealed because don't want other classes extending or inheriting from
  public sealed class RabbitMQBus : IEventBus
  {
    #region Constructor

    //MediatR used to send msgs & return response data
    private readonly IMediator _mediator;

    //Dictionary for KVP list of Event Handlers
    private readonly Dictionary<string, List<Type>> _handlers;

    //List of Event Types
    private readonly List<Type> _eventTypes;

    public RabbitMQBus(IMediator mediator)
    {
      _mediator = mediator;
      _handlers = new Dictionary<string, List<Type>>();
      _eventTypes = new List<Type>();
    }

    #endregion Constructor

    public Task SendCommand<T>(T command) where T : Command
    {
      return _mediator.Send(command);
    }

    /// <summary>
    /// Publish method implementation
    /// </summary>
    /// <typeparam name="T">Type of Event</typeparam>
    /// <param name="event">Event</param>

    //method sends in a generic Event
    public void Publish<T>(T @event) where T : Event
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        var eventName = @event.GetType().Name;

        //new queue name == 'eventName' value
        channel.QueueDeclare(eventName.Trim(), false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
      }
    }

    /// <summary>
    /// Subscribe method implementation
    /// </summary>
    /// <typeparam name="T">Type of Event</typeparam>
    /// <typeparam name="TH">Type of IEventHandler</typeparam>
    /// <exception cref="ArgumentException"></exception>

    //for subscribing to Events and by Handler type
    public void Subscribe<T, TH>()
      where T : Event
      where TH : IEventHandler<T>
    {
      //T is the event passed in, TH is handler
      var eventName = typeof(T).Name;
      var handlerType = typeof(TH);

      //if List does not have this Event type, add it
      if (!_eventTypes.Contains(typeof(T)))
      {
        _eventTypes.Add(typeof(T));
      }
      //if Dictionary has no key entry for this Event type, add it, along with associating the key to New List of Type entry as the value pairing
      if (!_handlers.ContainsKey(eventName))
      {
        _handlers.Add(eventName, new List<Type>());
      }

      //basic validation:
      //if any exists where [eventName] (T) is key of kvp list in _handlers Dictionary,
      //if true, see if handler value pair (TH) already exists there.
      //if true, throw exception that value already exists for that key.
      if (_handlers[eventName].Any(x => x.GetType() == handlerType))
      {
        throw new ArgumentException(
          $"Handler Type {handlerType.Name} already is registered for " +
          $"'{eventName}'", nameof(handlerType));
      }

      //else: assign handler value and add it to this entry ([eventName]) in list of event type keys
      _handlers[eventName].Add(handlerType);

      //after subscription is straightened out, consume msgs (msg = T and T = Events)
      StartBasicConsume<T>();
    }

    /// <summary>
    /// Consume Event messages by name from 'eventName' queue asynchronously
    /// </summary>
    /// <typeparam name="T">Event</typeparam>
    private void StartBasicConsume<T>() where T : Event
    {
      var factory = new ConnectionFactory()
      {
        HostName = "localhost",
        DispatchConsumersAsync = true
      };

      var connection = factory.CreateConnection();
      var channel = connection.CreateModel();

      var eventName = typeof(T).Name;

      //which queue and how will it be used
      channel.QueueDeclare(eventName, false, false, false, null);

      var consumer = new AsyncEventingBasicConsumer(channel);
      consumer.Received += Consumer_Received;

      channel.BasicConsume(eventName, true, consumer);
    }

    /// <summary>
    /// Event fired when a delivery arrives for the consumer
    /// </summary>
    /// <param name="sender">Msg-sending method</param>
    /// <param name="e">BasicDeliverEventArgs => contains all info about msg</param>
    /// <returns></returns>
    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
      var eventName = e.RoutingKey;
      var message = Encoding.UTF8.GetString(e.Body.Span);

      try
      {
        await ProcessEvent(eventName, message).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        //TODO: add logging
      }
    }

    /// <summary>
    /// Process msgs from all subscription feeds, genericize them and route to Microservices
    /// </summary>
    /// <param name="eventName">Used to select for subscription instances eventType</param>
    /// <param name="message">Body of message delivered by Sender</param>
    /// <returns>new event object</returns>
    private async Task ProcessEvent(string eventName, string message)
    {
      //Dictionary key is "eventName"
      if (_handlers.ContainsKey(eventName))
      {
        var subscriptions = _handlers[eventName];
        foreach (var subscription in subscriptions)
        {
          //handler is new type of subscription
          var handler = Activator.CreateInstance(subscription);
          //keep looping thru until find valid subscription
          if (handler == null) continue;
          //loop thru local Dictionary of eventTypes by name
          var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
          //deserialize message, eventType obj into event
          var @event = JsonConvert.DeserializeObject(message, eventType);
          //all event handlers implement IEventHandler<ofSomeType>, so let's make it a generic type based on "eventType"
          var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
          //this routes the handlers
          //take event handler ("concreteType") and get its method, pass it the handler and event
          await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
        }
      }
    }
  }
}