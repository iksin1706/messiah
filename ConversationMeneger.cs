using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ConversationMeneger : MonoBehaviour
{
    public Conversation currentConversation;
    public GameObject dialogueOptionsPanel;
    public GameObject dialoguePanel;    
    public GameObject DialogueOptionPrefab;
    public GameObject mainCamera;
    public GameObject brainCamera;
    public GameObject playerBackCamera;
    public GameObject npcBackCamera;

    private bool isDialogueStarted;
    private NPC npc;
    private Player player;
    private Dialogue currentDialogue;
    private List<Dialogue> enabledDialogues;


    void OnLevelWasLoaded(int level)
    {
        mainCamera = GameObject.Find("Main Camera");
        brainCamera = GameObject.Find("brain");
        dialogueOptionsPanel = GameObject.Find("Canvas").transform.Find("DialogueOptionsPanel").gameObject;
        dialoguePanel = GameObject.Find("Canvas").transform.Find("DialoguePanel").gameObject;

        dialogueOptionsPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        playerBackCamera = transform.Find("Camera").gameObject;

    }
    private void Awake()
    {
        OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (isDialogueStarted)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if(npcBackCamera!=null)
            npcBackCamera.GetComponent<DialogueCamera>().enabled = true;
        }else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (npcBackCamera != null)
                npcBackCamera.GetComponent<DialogueCamera>().enabled = false;
        }
        if (currentDialogue != null&&currentConversation!=null && (Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("Fire1"))) NextSentence();
    }

    public void StartConversation(Conversation conversation)
    {
        Debug.Log("started");
        currentConversation = conversation;
        if (currentConversation.NpcCamera != null)
        {
            npcBackCamera = currentConversation.NpcCamera;
            mainCamera.SetActive(false);
            npcBackCamera.SetActive(true);
        }
        npc = currentConversation.gameObject.GetComponent<NPC>();
        player = gameObject.GetComponent<Player>();
        ShowDialogueOptions();
        player.CanMove(false);
        dialogueOptionsPanel.GetComponent<Animation>().Play();
        if(npc!=null)
        npc.isTalking = true;
    }

    public void FinishConversation()
    {
        npcBackCamera.GetComponent<Camera>().enabled = false;
        playerBackCamera.GetComponent<Camera>().enabled = false;
        dialogueOptionsPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        if(npc!=null)
        npc.isTalking = false;

        playerBackCamera.SetActive(false);
        brainCamera.SetActive(true);
        npcBackCamera.SetActive(true);
        mainCamera.SetActive(true);


        if (currentConversation.NpcCamera != null)
        {
            npcBackCamera.SetActive(false);
            npcBackCamera.GetComponent<Camera>().enabled = false;
        }
        if (npc != null)
            currentConversation.gameObject.GetComponent<NPC>().StopInteracting();
        currentConversation = null;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueStarted = true;
        currentDialogue = dialogue;
        if (dialogue.sentences != null)
        {
            if (dialogue.sentences.Count > 0)
            {
                currentDialogue.currnetSentence = 0;
                StartSentence();
            }
            else FinishDialogue();
        }else FinishDialogue();
    }

    IEnumerator TypeSentece()
    {
        Sentence sentence = currentDialogue.sentences[currentDialogue.currnetSentence];
        var sentenceText = dialoguePanel.transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
        sentenceText.text = "";
        foreach (var letter in sentence.text.ToCharArray())
        {
            sentenceText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void StartSentence()
    {

        //With typing animation
        dialoguePanel.SetActive(true);
        dialogueOptionsPanel.SetActive(false);
        Sentence sentence = currentDialogue.sentences[currentDialogue.currnetSentence];
        npc.TriggerTalking(sentence.animation.ToString());
        StopAllCoroutines();
        StartCoroutine(TypeSentece());
        //Without typing animation
        //dialoguePanel.transform.Find("Sentence").GetComponent<TextMeshProUGUI>().text = sentence.text;
        // dialoguePanel.transform.Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text =
        //sentence.isPlayerSentence ? "Jesus" : npc.characterName;
        if (sentence.isPlayerSentence) { 
            npcBackCamera.SetActive(true);
            playerBackCamera.SetActive(false);
            npcBackCamera.GetComponent<Camera>().enabled = false;
            brainCamera.SetActive(true);
        }
        else
        {
            npcBackCamera.SetActive(false);
            brainCamera.SetActive(false);
            playerBackCamera.SetActive(true);
            playerBackCamera.GetComponent<Camera>().enabled = true;
            
        }
    }

    public void NextSentence()
    {
        if (currentDialogue.currnetSentence < currentDialogue.sentences.Count - 1)
        {
            currentDialogue.currnetSentence++;
            StartSentence();          
        }
        else FinishDialogue();
    }

    public void FinishDialogue()
    {
        if(currentDialogue.dialoguesToEnable!=null)
        foreach (var dialogue in currentDialogue.dialoguesToEnable)
        {
            currentConversation.EnableDialogue(dialogue);
        }

        isDialogueStarted = false;
        if (currentDialogue.single) currentDialogue.onEnd.AddListener(() => { currentConversation.DisableDialogue(currentConversation.dialogues.IndexOf(currentDialogue)); });
         currentDialogue.onEnd.Invoke();
        currentDialogue = null;
        if (currentConversation != null) ShowDialogueOptions();
        Debug.Log(enabledDialogues.Count);
        if(currentConversation!=null)
        if (currentConversation.dialogues.Where(e => e.isEnabled == true).ToList().Count <= 0) FinishConversation();
    }
    public void ClearDialogueOptions()
    {
        for (int i = 0; i < dialogueOptionsPanel.transform.childCount; i++)
        {
            Destroy(dialogueOptionsPanel.transform.GetChild(i).gameObject);
        }
    }
    public void ShowDialogueOptions()
    {
        ClearDialogueOptions();
        playerBackCamera.SetActive(false);
        dialoguePanel.SetActive(false);

        npcBackCamera.SetActive(true);
        brainCamera.SetActive(true);
        dialogueOptionsPanel.SetActive(true);
        enabledDialogues = currentConversation.dialogues.Where(e => e.isEnabled == true).ToList();

        if (currentConversation.canFinish) { 
            enabledDialogues.Add(new Dialogue { startSentence = "KONIEC", onEnd = new UnityEvent()});
            enabledDialogues.Last().onEnd.AddListener(() => { FinishConversation(); });
        }
        int i = 0;
        foreach (var dialogue in enabledDialogues)
        {
            i++;
            var dialogueOption =Instantiate(DialogueOptionPrefab, dialogueOptionsPanel.transform);
            
            dialogueOption.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = //((dialogue.startSentence != "KONIEC")?i+".":"")+
                dialogue.startSentence;
            if (dialogue.isImportant) dialogueOption.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color32(255, 219, 83,255);
            dialogueOption.GetComponent<Button>().onClick.AddListener(delegate { StartDialogue(dialogue); });
        }      
    }
}
