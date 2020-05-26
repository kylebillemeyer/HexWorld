using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Threading;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float timeout;
    public float Timeout
    {
        get { return timeout; }
        set { timeout = value; }
    }

    [SerializeField]
    private bool paused;
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    [SerializeField]
    private bool resets;
    public bool Resets
    {
        get { return resets; }
        set { resets = value; }
    }

    public bool Done { get; set; }

    [SerializeField]
    private UnityEvent onTimeout;
    public UnityEvent OnTimeout 
    { 
        get { return onTimeout; }
        set { onTimeout = value; }
    }

    private float remaining;

    private void Awake()
    {
        if (onTimeout == null)
            onTimeout = new UnityEvent();
    }

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Done)
        {
            if (!Paused)
            {
                remaining -= Time.deltaTime;
            }

            if (remaining <= 0)
            {
                OnTimeout.Invoke();

                if (Resets)
                    Reset();
                else
                    Done = true;
            }
        }
    }

    public void Reset()
    {
        remaining = Timeout;
    }
}
