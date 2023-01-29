using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float RemainingSeconds { get; private set; }

    //constructor
    public Timer(float duration)
    {
        RemainingSeconds = duration;
    }

    //
    public event Action OnTimerEnd;

    public void Tick(float deltaTime)
    {
        if(RemainingSeconds == 0f) { return; }

        RemainingSeconds -= deltaTime;

        CheckForTimerEnd();
    }

    public void AddTime(float time)
    {
        RemainingSeconds += time;
    }

    private void CheckForTimerEnd()
    {
        if(RemainingSeconds > 0f) { return; }
        RemainingSeconds = 0f;


        //if it is not null.. Invoke it! (OnTimerEnd != null) --> OnTimerEnd(SomeValue);
        OnTimerEnd?.Invoke();
    }
}
