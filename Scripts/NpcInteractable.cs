using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcInteractable : MonoBehaviour
{
    public bool canInteractFromAnySide;
    public float stoppingDistance;
    public float waitTime;
    public UnityEvent onInteract;
    public UnityEvent afterWait;
    public Vector3 offSet;

    public IEnumerator StartWaiting()
    {
        yield return new WaitForSeconds(waitTime);
        afterWait.Invoke();
        Debug.Log("pod czekaniu");
    }


}
