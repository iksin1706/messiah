using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestStage
{
    public Status status;
    public string title;
    [TextArea]
    public List<string> targets;
    public string description;
    public UnityEvent onStart;
    public UnityEvent onFinish;
    



}
