using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuestMeneger : MonoBehaviour
{
    public int startQuestIndex;
    public bool isOpen { get; private set; } = false;
    public Quests quests;
    public GameObject panel;
    public GameObject exclamationMarkPrefab;
    public Sprite HighPriorityMark;
    public Sprite SemiPriorityMark;
    public Sprite LowPriorityMark;
    public GameObject info;
    public GameObject questPanel;
    public GameObject questsListPanel;
    public GameObject questPrefabUI;

    private TextMeshProUGUI title;
    private TextMeshProUGUI stage;
    private Color defaultcolor;
    private Image icon;
    private List<GameObject> exclamationMarks;


    void OnLevelWasLoaded(int level)
    {

    }

    private void Awake()
    {
        quests = GameObject.Find("Quests").GetComponent<Quests>();
        panel = GameObject.Find("Canvas").transform.Find("SelectedQuest").gameObject;
        info = GameObject.Find("Canvas").transform.Find("info").gameObject;
        questPanel = GameObject.Find("Canvas").transform.Find("QuestPanel").gameObject;
        questsListPanel = GameObject.Find("Canvas").transform.Find("QuestPanel").transform.Find("Quests").gameObject;

        defaultcolor = info.GetComponent<TextMeshProUGUI>().color;
        title = panel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        stage = panel.transform.Find("Stage").GetComponent<TextMeshProUGUI>();
        icon = panel.transform.Find("Icon").GetComponent<Image>();
        exclamationMarks = new List<GameObject>();
        HideQuestPanel();

        var questToSelect = quests.quests.FirstOrDefault(e => e.status == Status.Active);



        if (questToSelect != null) SelectQuest(quests.quests.IndexOf(questToSelect));
        else panel.SetActive(false);
    }

    private void Start()
    {
        quests = GameObject.Find("Quests").GetComponent<Quests>();
        if(startQuestIndex!=-1)
        StartQuest(startQuestIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOpen) ShowQuestPanel();
        else if (Input.GetKeyDown(KeyCode.Q) && isOpen) HideQuestPanel();
        if (quests.selectedQuest == null)
        {
                panel.SetActive(false);
                info.SetActive(false);
            return;
        }
        if (quests.selectedQuest.title == "")
        {
            panel.SetActive(false);
            info.SetActive(false);
            return;
        }

        panel.SetActive(true);
    }

    public void SelectQuest(int index)
    {
        quests.selectedQuest = quests.quests[index];

        DisplaySelectedQuest();
        DisplayActiveQuests();
        CreateExclamationMarks();
        panel.SetActive(true);
        info.SetActive(true);
        Debug.Log("select");
    }

    public void StartMainQuestStage(int stage)
    {
        var activeStage = GetActiveStage(0);
        FinishStage(0, quests.quests[0].stages.IndexOf(activeStage));
        StartStage(0, stage);
    }

    public void DisplaySelectedQuest()
    {
        if (quests.selectedQuest != null)
        {
            panel.SetActive(true);

            title.text = quests.selectedQuest.title;
            stage.text = GetActiveStage(quests.quests.IndexOf(quests.selectedQuest)).title;
            switch (quests.quests[quests.quests.IndexOf(quests.selectedQuest)].priority)
            {
                case Priority.High:
                    icon.sprite = HighPriorityMark;
                    break;
                case Priority.Semi:
                    icon.sprite = SemiPriorityMark;
                    break;
                case Priority.Low:
                    icon.sprite = LowPriorityMark;
                    break;
            }
        }
        else panel.SetActive(false);
    }
    public void CreateExclamationMarks()
    {
        RemoveExclamationMarks();
        Debug.Log("Start");
        foreach (var item in GetActiveStage(quests.quests.IndexOf(quests.selectedQuest)).targets)
        {
            var gO = GameObject.Find(item);
            var exclamationMark = new GameObject();
            if (!gO.isStatic) {
                exclamationMark = Instantiate(exclamationMarkPrefab);
            exclamationMark.transform.position = gO.transform.position + Vector3.up * 2;
                exclamationMark.transform.SetParent(gO.transform);
        }
            else
            {
                exclamationMark = Instantiate(exclamationMarkPrefab);
                exclamationMark.transform.position = gO.transform.position + Vector3.up * 2;
            }
            exclamationMarks.Add(exclamationMark);
            exclamationMark.SetActive(true);
        }
    }
    public void RemoveExclamationMarks()
    {
        foreach (var item in exclamationMarks) Destroy(item);
    }
    public void StartQuest(int index)
    {
        if (quests.quests[index].status != Status.Done || quests.quests[index].status != Status.Failed)
        {
            quests.quests[index].status = Status.Active;
            SelectQuest(index);
            StartStage(index, 0);
        }
    }
    public void StartQuest(string name)
    {
        Debug.Log(name);
        var index = quests.quests.IndexOf(quests.quests.FirstOrDefault(quest => quest.title == name));

        Debug.Log(quests.quests.FirstOrDefault(quest => quest.title == name));
        if (quests.quests[index].status != Status.Done || quests.quests[index].status != Status.Failed)
        {
            quests.quests[index].status = Status.Active;
            SelectQuest(index);
            StartStage(index, 0);
        }
    }

    public void StartStage(int index, int stageIndex)
    {
        if (quests.quests[index].stages[stageIndex].status != Status.Done || quests.quests[index].stages[stageIndex].status != Status.Failed)
            quests.quests[index].stages[stageIndex].status = Status.Active;

        quests.quests[index].stages[stageIndex].onStart.Invoke();
        DisplayInfo(quests.quests[index].stages[stageIndex].title, defaultcolor);
        CreateExclamationMarks();
        DisplaySelectedQuest();
    }

    public void StartStageSecretly(int index, int stageIndex)
    {
        if (quests.quests[index].stages[stageIndex].status != Status.Done || quests.quests[index].stages[stageIndex].status != Status.Failed)
            quests.quests[index].stages[stageIndex].status = Status.Active;

        quests.quests[index].stages[stageIndex].onStart.Invoke();
        CreateExclamationMarks();
        DisplaySelectedQuest();
    }


    public void DisplayInfo(string text, Color color)
    {
        var TMP = info.GetComponent<TextMeshProUGUI>();
        TMP.enabled = true;
        TMP.text = text;
        TMP.color = color;
        info.GetComponent<Animation>().Play();
    }
    public void FinishQuestSuccessful(int index)
    {
        if (quests.quests[index].status != Status.Done || quests.quests[index].status != Status.Failed)
        {
            quests.quests[index].status = Status.Done;
            foreach (var stage in quests.quests[index].stages)
                stage.status = Status.Done;

            quests.quests[index].onFinishSuccessfully.Invoke();

            if (quests.selectedQuest == quests.quests[index]) quests.selectedQuest = null;

            DisplayInfo("Ukończono zadanie ''" + quests.quests[index].title + "''", Color.green);
            DisplayActiveQuests();

            var questToSelect = quests.quests.FirstOrDefault(e => e.status == Status.Active);

            if (questToSelect != null) SelectQuest(quests.quests.IndexOf(questToSelect));
            else panel.SetActive(false);


        }

    }
    public void FinishQuestUnsuccessful(int index)
    {
        
        RemoveExclamationMarks();

        if (quests.quests[index].status != Status.Done || quests.quests[index].status != Status.Failed)
        {
            quests.quests[index].status = Status.Failed;
            foreach (var stage in quests.quests[index].stages)
            {
                stage.status = Status.Failed;
            }
        }
        quests.quests[index].onFinishUnsuccessfully.Invoke();
        //FinishStage(quests.quests.IndexOf(quests.selectedQuest),quests.selectedQuest.stages.IndexOf(GetActiveStage(quests.quests.IndexOf(quests.selectedQuest))));
        panel.SetActive(false);
        SelectQuest(index);
        DisplayInfo("Zadanie zakończone niepowodzeniem ''" + quests.selectedQuest.title + "''", Color.red);
    }
    public void FinishStage(int index, int stageIndex)
    {
        if (quests.quests[index].stages[stageIndex].status != Status.Done || quests.quests[index].stages[stageIndex].status != Status.Failed)
            quests.quests[index].stages[stageIndex].status = Status.Done;

        quests.quests[index].stages[stageIndex].onFinish.Invoke();
        RemoveExclamationMarks();
    }
    public QuestStage GetActiveStage(int index)
    {
        return quests.quests[index].stages.FirstOrDefault(e => e.status == Status.Active);
    }
    public void NextStage(int index)
    {
        Debug.Log("lol");
        var activeStage = GetActiveStage(index);

        if (quests.quests[index].stages.IndexOf(activeStage) + 1 < quests.quests[index].stages.Count)
        {
            FinishStage(index, quests.quests[index].stages.IndexOf(activeStage));
            StartStage(index, quests.quests[index].stages.IndexOf(activeStage) + 1);
        }
        else
        {
            FinishStage(index, quests.quests[index].stages.IndexOf(activeStage));
        }
    }

    public void NextStageSecretly(int index)
    {
        Debug.Log("lol");
        var activeStage = GetActiveStage(index);

        if (quests.quests[index].stages.IndexOf(activeStage) + 1 < quests.quests[index].stages.Count)
        {
            FinishStage(index, quests.quests[index].stages.IndexOf(activeStage));
            StartStageSecretly(index, quests.quests[index].stages.IndexOf(activeStage) + 1);
        }
        else
        {
            FinishStage(index, quests.quests[index].stages.IndexOf(activeStage));
        }
    }


    public void ShowQuestPanel()
    {
        isOpen = true;
        GetComponent<Player>().canMove = false;
        questPanel.SetActive(true);
        Debug.Log("show");
    }
    public void HideQuestPanel()
    {
        isOpen = false;
        GetComponent<Player>().canMove = true;
        questPanel.SetActive(false);
        Debug.Log("hide");
    }
    public void DisplayActiveQuests()
    {
        ClearQuestPanel();
        var activeQuests = quests.quests.Where(e => e.status == Status.Active).ToList();

        questPanel.transform.Find("Status").GetComponent<TextMeshProUGUI>().text = "Aktualne";

        foreach (var item in activeQuests)
        {
            var quest = Instantiate(questPrefabUI, questsListPanel.transform);
            var text = quest.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            var questIcon = quest.transform.Find("Image").GetComponent<Image>();
            text.text = item.title;
            switch (item.priority)
            {
                case Priority.High:
                    questIcon.sprite = HighPriorityMark;
                    break;
                case Priority.Semi:
                    questIcon.sprite = SemiPriorityMark;
                    break;
                case Priority.Low:
                    questIcon.sprite = LowPriorityMark;
                    break;
            }
            if (item == quests.selectedQuest) text.color = new Color32(255, 218, 83, 255);
            quest.GetComponent<Button>().onClick.AddListener(() => { SelectQuest(quests.quests.IndexOf(item)); });
        }
    }
    public void DisplayDoneQuests()
    {
        ClearQuestPanel();
        var activeQuests = quests.quests.Where(e => e.status == Status.Done).ToList();
        questPanel.transform.Find("Status").GetComponent<TextMeshProUGUI>().text = "Wykonane";

        foreach (var item in activeQuests)
        {
            var quest = Instantiate(questPrefabUI, questsListPanel.transform);
            var text = quest.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            var questIcon = quest.transform.Find("Image").GetComponent<Image>();
            text.text = item.title;
            switch (item.priority)
            {
                case Priority.High:
                    questIcon.sprite = HighPriorityMark;
                    break;
                case Priority.Semi:
                    questIcon.sprite = SemiPriorityMark;
                    break;
                case Priority.Low:
                    questIcon.sprite = LowPriorityMark;
                    break;

            }
            if (item == quests.selectedQuest) text.color = new Color32(255, 218, 83, 255);

        }
    }
    public void DisplayFailedQuests()
    {
        ClearQuestPanel();
        var activeQuests = quests.quests.Where(e => e.status == Status.Failed).ToList();
        questPanel.transform.Find("Status").GetComponent<TextMeshProUGUI>().text = "Anulowane";
        foreach (var item in activeQuests)
        {
            var quest = Instantiate(questPrefabUI, questsListPanel.transform);
            var text = quest.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            var questIcon = quest.transform.Find("Image").GetComponent<Image>();
            text.text = item.title;
            switch (item.priority)
            {
                case Priority.High:
                    questIcon.sprite = HighPriorityMark;
                    break;
                case Priority.Semi:
                    questIcon.sprite = SemiPriorityMark;
                    break;
                case Priority.Low:
                    questIcon.sprite = LowPriorityMark;
                    break;

            }
            if (item == quests.selectedQuest) text.color = new Color32(255, 218, 83, 255);
        }
    }
    public void ClearQuestPanel()
    {
        for (int i = 0; i < questsListPanel.transform.childCount; i++)
        {
            Destroy(questsListPanel.transform.GetChild(i).gameObject);
        }
    }
}
