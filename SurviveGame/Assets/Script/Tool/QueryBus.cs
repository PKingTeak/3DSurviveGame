using System;
using System.Collections.Generic;
using UnityEngine;

public static class QueryBus<T>
{

    static readonly Dictionary<string, Func<T>> _providers = new();


    public static void Register(string topics, Func<T> provider)
    {
        if (string.IsNullOrEmpty(topics) || provider == null) return;

        _providers[topics] = provider;
    }

    public static void Unregister(string topics)
    {
        if (string.IsNullOrEmpty(topics)) return;

        _providers.Remove(topics);
        
    }



    public static bool TryRequest(string topic, out T value )
    {
        if (_providers.TryGetValue(topic, out var f) && f != null)
        {

            try { value = f(); return true; }
            catch (Exception e)
            {
                Debug.LogError($"QueryBus<{typeof(T).Name}> '{topic}' provider error");
                Debug.LogException(e);
            }
        }
        value = default;
        return false;
    }

    public static T RequstOr(T fallback, string topic) => TryRequest(topic, out var value) ? value : fallback;
   

}
