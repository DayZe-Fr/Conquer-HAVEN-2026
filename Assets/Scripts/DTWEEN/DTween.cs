using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public static class DTweenLib
{
    private static DTween_Driver TheDriver;
    private static  bool IsInitialized = false;
    private static List<Tween> Tasks = new List<Tween>();

    public static void NewTween(Vector3 Target, float Duration, Func<Vector3> getter, Action<Vector3> setter)
    {
        if (!IsInitialized)
        {
            Init();
        }
        Tween<Vector3> NewItem = new Tween<Vector3>();
        NewItem.setValue = setter;
        NewItem.getValue = getter;

        NewItem.IncrementFunction = Vector3.Lerp;
        NewItem.Duration = Duration;
        NewItem.Start = NewItem.getValue();
        NewItem.End = Target;

        Tasks.Add(NewItem);
    }

     public static void NewTween(float Target, float Duration, Func<float> getter, Action<float> setter)
    {
        if (!IsInitialized)
        {
            Init();
        }
        Tween<float> NewItem = new Tween<float>();
        NewItem.setValue = setter;
        NewItem.getValue = getter;

        NewItem.IncrementFunction = Mathf.Lerp;
        NewItem.Duration = Duration;
        NewItem.Start = NewItem.getValue();
        NewItem.End = Target;

        Tasks.Add(NewItem);
    }


    static void Init()
    {
        IsInitialized = true;
        GameObject Driver = new GameObject("DTweenDriver");
        Driver.AddComponent<DTween_Driver>();
        TheDriver = Driver.GetComponent<DTween_Driver>();
    }

    public static void RunTween(float DeltaTime)
    {
        if (!IsInitialized) Init();
        for (int i = Tasks.Count - 1; i >= 0; i--)
        {
            if (Tasks[i].progress >= Tasks[i].Duration)
            {
                Tasks[i].progress = Tasks[i].Duration;
                Tasks[i].UpdateTween(0);
                Tasks.RemoveAt(i); // memory leak?
                continue;
            }
            Tasks[i].UpdateTween(DeltaTime);
        }
    }
}

public abstract class Tween
{
    public float Duration;
    public float progress;
    public abstract void UpdateTween(float DeltaTime);
}


public class Tween<T> : Tween
{
    public T Item;
    public T Start;
    public T End;
    public Func<T> getValue;
    public Action<T> setValue;
    public Func<T, T, float, T> IncrementFunction;
    public override void UpdateTween(float DeltaTime)
    {
        float ProgressValue = Mathf.Clamp01(this.progress/this.Duration);
        var Value = this.IncrementFunction(this.Start, this.End, ProgressValue);
        setValue(Value);
        this.progress += DeltaTime;
    }
}