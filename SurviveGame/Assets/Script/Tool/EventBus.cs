using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public static class EventBus<T>
{
    static readonly Dictionary<string, Action<T>> _topics = new(); //여기에 이제 구독자들을 등록하고

    public static void Subscribe(string topic, Action<T> handler)
    {
        if (string.IsNullOrEmpty(topic) || handler == null)
        {
            Debug.Log("해당 이벤트가 존재하지 않습니다.");
            return;
        }
        
        if (_topics.TryGetValue(topic, out var exist))
        {
            _topics[topic] = exist + handler;//해당 이벤트에 추가로 이벤트를 붙이는것
        }
        else
        {
            _topics[topic] = handler; //첫구독
        }
                
        
     
    }

    public static void Unsubscribe(string topic, Action<T> handler)
    {
        if (_topics.TryGetValue(topic, out var exist))
        {
            exist -= handler;
            if (exist == null)
            {
                _topics.Remove(topic);
            }
            else 
            {
                _topics[topic] = exist;
            }
        }
    }




    public static void PublishAction(string topic, T payLoad)
    {
        if (string.IsNullOrEmpty(topic)) return;

        if (_topics.TryGetValue(topic, out var handler) && handler != null)
        { 
            foreach (var d in handler.GetInvocationList())
            {
                try 
                {
                    ((Action<T>)d).Invoke(payLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"EventBus<{typeof(T).Name}> '{topic}' handler error");
                    Debug.LogException(e);
                }
            }
            
        }
        
    }

    
    

}
