using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public bool isEnabled;
    public bool isImportant;
    public bool single;
    public string startSentence;
    public List<Sentence> sentences;
    public List<int> dialoguesToEnable;
    public UnityEvent onEnd;
    [HideInInspector]
    public int currnetSentence;
}

public enum AnimationTalkingOption
{
    Idle=0,
    Talking1,
    Talking2,
    Talking3,
    Talking4,
    Talking5,
    Yelling,
    Yes,
    Sad,
    Thankful
}

[System.Serializable]
public class Sentence
{
    public bool isPlayerSentence;
    public AnimationTalkingOption animation;
    [TextArea]
    public string text;
    public int duration {
        get {
            return text.Length / 10;
        }
    }
}
