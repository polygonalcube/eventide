using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Message")]
public class Message : ScriptableObject
{
    // A message.

    // 

    public bool isBroadcasting = false;
    public List<string> arguments;
    /*
    public void Alert()
    {
        if (listeners.Length == 0) return;
        foreach (MessageListener listener in listeners)
        {
            listener.messageReceived = true;
        }
    }
    */
}
