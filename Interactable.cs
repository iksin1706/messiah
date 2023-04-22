using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public  class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public bool instantInteract;
    public float waitTime;
    public float requiredDistance;
    public string floatingName;
    protected FloatingText nameText;
    public bool canInteractFromAnySide;
    public Vector3 offSet;
    public UnityEvent onStartInteracting;
    public UnityEvent onInteract;
    public UnityEvent onStopInteracting;
    public GameObject player;


    public virtual void Start()
    {
        nameText = transform.Find("Name").GetComponent<FloatingText>();
        if(floatingName!=null)
        nameText.text = floatingName;

        HideName();
        
    }

    void Update()
    {

    }

    public void SetInteract(bool _canInteract)
    {
        canInteract = _canInteract;
        if (canInteract) gameObject.tag = "Interactable";
        else gameObject.tag = "Untagged";
    }


    public void ShowName()
    {
        transform.Find("Name").Find("Text").GetComponent<TextMesh>().gameObject.SetActive(true);
    }
    public void HideName()
    {
        transform.Find("Name").Find("Text").GetComponent<TextMesh>().gameObject.SetActive(false);
    }
    public virtual void Interact()
    {
        onInteract.Invoke();
    }
}
