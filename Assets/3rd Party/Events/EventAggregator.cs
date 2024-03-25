using System;
using System.Collections.Generic;
using nosenfield.Logging;

public class EventAggregator
{
    private static readonly Dictionary<Type, List<Action<object>>> subscribers = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<TEvent>(Action<object> handler, int priority = 0)
    {
        Type eventType = typeof(TEvent);
        if (!subscribers.ContainsKey(eventType))
        {
            subscribers[eventType] = new List<Action<object>>();
        }
        subscribers[eventType].Insert(priority, handler);
    }

    public static void Unsubscribe<TEvent>(Action<object> handler)
    {
        Type eventType = typeof(TEvent);
        DefaultLogger.Instance.Log(LogLevel.DEBUG, $"Unsubscribe from {eventType}");
        if (!subscribers.ContainsKey(eventType))
        {
            throw new Exception($"EventAggregator does not have any subscribers for event {eventType.ToString()}");
        }

        try
        {
            subscribers[eventType].Remove(handler);
        }
        catch
        {
            throw new Exception($"EventAggregator does not contain the specified listener for event {eventType.ToString()}");
        }
    }

    public static void Publish<TEvent>(TEvent ev)
    {
        Type eventType = typeof(TEvent);
        if (subscribers.ContainsKey(eventType))
        {
            foreach (var handler in subscribers[eventType])
            {
                handler(ev);
            }
        }
    }
}