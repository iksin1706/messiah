using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcTarget
{
    public bool canInteractFromAnySide;
    public float stoppingDistance;
    public float waitTime;
    public UnityEvent onInteract;
    public Vector3 offSet;
}
