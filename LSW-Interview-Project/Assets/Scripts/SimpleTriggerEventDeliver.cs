using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTriggerEventDeliver : MonoBehaviour
{
    public UnityEvent triggerEvents;
    public UnityEvent triggerExitEvents;

    public void InvokeEvents()
    {
        triggerEvents.Invoke();
    }
    public void InvokeExitEvents()
    {
        triggerExitEvents.Invoke();
    }
}
