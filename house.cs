using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class house : MonoBehaviour
{
    public List<GameObject> jordanObjects;
    public List<GameObject> houseObjects;
    Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeInside()
    {
        anim.Play("FadeInside");
    }
    public void FadeOutside()
    {
        anim.Play("FadeOutside");
    }


    public void EnterHouse()
    {
        foreach (var item in jordanObjects)
        {
            item.SetActive(false);
        }

        foreach (var item in houseObjects)
        {
            item.SetActive(true);
        }
    }

    public void LeaveHouse()
    {
        foreach (var item in jordanObjects)
        {
            item.SetActive(true);
        }

        foreach (var item in houseObjects)
        {
            item.SetActive(false);
        }
    }
}
