using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quests : MonoBehaviour
{
    public string startQuestTitle;
    [HideInInspector]
    public Quest selectedQuest;
    public List<Quest> quests;
    
    public static Quests instance;
    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) Destroy(gameObject);

        if (instance == null)
            instance = this;
        else
        {
          //  instance.quests = quests;
           // instance.selectedQuest = gameObject.GetComponent<Quests>().selectedQuest;
            Debug.Log(instance.selectedQuest.title);
            
            Destroy(gameObject);
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex != 1)
            DontDestroyOnLoad(transform.gameObject);

       //quests = GameObject.Find("Jesus").GetComponent<QuestMeneger>().quests;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
