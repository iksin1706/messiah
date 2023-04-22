using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class FirstMeet : MonoBehaviour
{
    [TextArea]
    public string dialogueToSay;
    public float dialogueTime;
    public FloatingText floatingText;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    bool invoked;
    // Start is called before the first frame update

    private void Update()
    {
        
    }


    void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!invoked)
        {
            invoked = true;
            onTriggerEnter.Invoke();
            floatingText.text = dialogueToSay;
            StartCoroutine(DiseactiveDialogue());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }

    IEnumerator DiseactiveDialogue()
    {
        yield return new WaitForSeconds(dialogueTime);
        floatingText.gameObject.SetActive(false);
    }
}
