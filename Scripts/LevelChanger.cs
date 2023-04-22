using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator anim;
    private int level;

    public void Update()
    {
    }

    public void FadeToLevel(int index)
    {
        level = index;
        anim.Play("FadeIn");

    }
    public void OnFadeComplete()
    {
        //if(level!=3)
        SceneManager.LoadSceneAsync(level);
       // else
       // {
       //     SceneManager.LoadScene("house", LoadSceneMode.Additive);
       // }
    }

}
