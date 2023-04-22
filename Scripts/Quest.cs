using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public enum Status
{
    Unactive,
    Active,
    Done,
    Failed
}

public enum Priority
{
    High,
    Semi,
    Low
}



[System.Serializable]
public class Quest
{
    public Priority priority;
    public string title;
    [TextArea]
    public string description;
    public Status status;
    public List<QuestStage> stages;
    public UnityEvent onStart;
    public UnityEvent onFinishSuccessfully;
    public UnityEvent onFinishUnsuccessfully;






}
