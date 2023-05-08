using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    public static MainThreadDispatcher Instance { get; private set; }

    private readonly Queue<System.Action> executeOnMainThreadQueue = new Queue<System.Action>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnqueueAction(System.Action action)
    {
        lock (executeOnMainThreadQueue)
        {
            executeOnMainThreadQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        while (executeOnMainThreadQueue.Count > 0)
        {
            System.Action action;

            lock (executeOnMainThreadQueue)
            {
                action = executeOnMainThreadQueue.Dequeue();
            }

            action?.Invoke();
        }
    }
}
