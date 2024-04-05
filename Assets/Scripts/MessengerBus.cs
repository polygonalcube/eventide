using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerBus : MonoBehaviour
{
    // The global messemger bus.

    // Facilitates communication between scripts.
    
    public static MessengerBus messenger; // Allows for Singleton.

    public Message[] messages;

    void Awake()
    {
        if (messenger != null && messenger != this) Destroy(this.gameObject);
        else messenger = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(Initialize());
    }

    public void Broadcast(string msgName = "", List<string> args = null)
    {
        foreach (Message message in messages)
        {
            if ((message.name == msgName))
            {
                StartCoroutine(BroadcastCycle(message));
                if (args != null) message.arguments = args;
                return;
            }
        }
    }

    IEnumerator Initialize()
    {
        foreach (Message message in messages)
        {
            message.isBroadcasting = false;
            message.arguments = new List<string>();
            yield return new WaitForSeconds(1f/30f);
            message.isBroadcasting = false;
            message.arguments = new List<string>();
        }
    }

    IEnumerator BroadcastCycle(Message message)
    {
        message.isBroadcasting = true;
        yield return new WaitForSeconds(1f/30f);
        message.isBroadcasting = false;
    }
}
