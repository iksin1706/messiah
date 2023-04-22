using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public GameObject sentencePanel;
    public List<GameObject> objectsToEnable;
    public List<GameObject> objectsToDisable;
    public UnityEvent onFinish;
    public GameObject canvas;


    public void SaySomething(string text)
    {
        sentencePanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(text));
    }
    public void GoToMainMenu()
    {
       canvas.SetActive(true);

        int children = canvas.transform.childCount;
        for (int i = 0; i < children; ++i)
            canvas.transform.GetChild(i).gameObject.SetActive(false);

        canvas.transform.Find("LevelChanger").gameObject.SetActive(true);

        canvas.transform.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel(0);
    }

    IEnumerator TypeSentence(string text)
    {
        var sentenceText = sentencePanel.GetComponent<TextMeshProUGUI>();
        sentenceText.text = "";
        foreach (var letter in text.ToCharArray())
        {
            sentenceText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void PlaySong(string song)
    {
        GameObject.FindObjectOfType<AudioPlayer>().PlayOnly(song);
    }

    public void StopSong(string song)
    {

    }

    public void FinishCutscne()
    {
        onFinish.Invoke();
        foreach (var item in objectsToEnable)
        {
            item.SetActive(true);
        }
        foreach (var item in objectsToDisable)
        {
            item.SetActive(false);
        }
    }




}
