using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : Interactable
{
    
    private bool isInteracting;

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartInteracting()
    {
        isInteracting = true;
        player.GetComponent<Player>().StartInteracting();

    }

    public void PlayAnimation()
    {
        GetComponent<Animation>().Play();
    }
}
