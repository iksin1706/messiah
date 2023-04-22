using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    public bool canFinish;
    public List<Dialogue> dialogues;
    public GameObject NpcCamera;
    

    public void EnableDialogue(int index)
    {
        dialogues[index].isEnabled = true;
    }
    public void DisableDialogue(int index)
    {
        dialogues[index].isEnabled = false;
    }
    public void FinishConversation()
    {
        GameObject.Find("Jesus").GetComponent<ConversationMeneger>().FinishConversation();
    }

}
