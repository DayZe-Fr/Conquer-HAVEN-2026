using System;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    public static Scheduler current {get; private set;}


    private List<ExecLaterFunction> ToExec = new List<ExecLaterFunction>();

    void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = ToExec.Count - 1; i >= 0; i--)
        {
            ExecLaterFunction functionToExecute = ToExec[i];
        
            functionToExecute.remainingTime -= Time.deltaTime;
        
            if (functionToExecute.remainingTime <= 0)
            {
                functionToExecute.ExecuteFunction?.Invoke();
                ToExec.RemoveAt(i);
            }
        }
    }

    public void ExecuteLater(Action action, float execIn)
    {
        ExecLaterFunction newfunc = new ExecLaterFunction();
        newfunc.remainingTime = execIn;
        newfunc.ExecuteFunction = action;

        ToExec.Add(newfunc);
    }

    private class ExecLaterFunction
    {
        public float remainingTime;
        public Action ExecuteFunction;
    }
}


