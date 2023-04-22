using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monologue : MonoBehaviour
{
    public MonologueSentence[] sentences;
    public int currentSentence;
    public UnityEvent onFinish;


}
