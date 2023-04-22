using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject first;
    public GameObject second;
    public Player player;


    private void Start()
    {
        player.canMove = false;
    }
    public void StartTutorial()
    {
        player.canMove = false;
        tutorialPanel.SetActive(true);
        first.SetActive(true);
    }
    public void GoToSecond()
    {
        first.SetActive(false);
        second.SetActive(true);
    }
    public void Finish()
    {
        this.gameObject.SetActive(false);
        player.canMove = true;
    }




}
