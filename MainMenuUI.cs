using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public Texture2D cursorSprite;

    public void OpenSettingsPanel()
    {
        Debug.Log("asd");
        settingsPanel.GetComponent<OptionsPanel>().OpenPanel();
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }


    void Start()
    {
        Cursor.SetCursor(cursorSprite, new Vector2(0, 0), CursorMode.Auto);
        settingsPanel.GetComponent<OptionsPanel>().LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
