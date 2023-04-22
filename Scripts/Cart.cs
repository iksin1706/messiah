using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Cart : MonoBehaviour
{

    public Player player;
    public List<GameObject> boxes;
    public UnityEvent onFilled;
    private int boxIndex=0;


    public void AddBox()
    {
        Debug.Log(player.GetHaveBoxInHands());
        Debug.Log(boxes.Count > boxIndex);
        if(player.GetHaveBoxInHands())
        if (boxes.Count > boxIndex)
        {
                Debug.Log(boxes[boxIndex]);
            boxes[boxIndex].SetActive(true);
            boxIndex++;
        }
        if(boxes.Count==boxIndex)
            {
                onFilled.Invoke();
                Debug.Log("Zapełnione");
            }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
