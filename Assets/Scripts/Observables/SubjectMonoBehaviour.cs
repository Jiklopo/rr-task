using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectMonoBehaviour : MonoBehaviour
{
    private List<IObserver> subscribers = new List<IObserver>();

    public void Subscribe(IObserver subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void Notify(object value, NotificationType notificationType = NotificationType.Default)
    {
        foreach (var sub in subscribers)
        {
            sub.OnNotify(value, notificationType);
        }
    }
}
