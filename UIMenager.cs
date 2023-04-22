using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenager : MonoBehaviour
{
    
    public bool isPaused { get; private set; } = false;
    public Player player;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject confirmPanel;   
    public Texture2D cursorSprite;
    public GameObject sideInfo;
    public static UIMenager instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
       // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Cursor.SetCursor(cursorSprite, new Vector2(0, 0), CursorMode.Auto);
        HideConfirmPanel();
        settingsPanel.GetComponent<OptionsPanel>().LoadSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();



        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) UnpauseGame();
    }
    public void OpenSettingsPanel()
    {
        Debug.Log("asd");
        settingsPanel.GetComponent<OptionsPanel>().OpenPanel();
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        settingsPanel.GetComponent<OptionsPanel>().LoadSettings();
    }

    public void UnpauseGame()
    {
        Debug.Log("sad");
        isPaused = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        player.canMove = true;
        confirmPanel.SetActive(false);
    }

    public void PauseGame()
    {
        if (player.canMove)
        {
            Debug.Log("asdas");
            isPaused = true;
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
            player.canMove = false;
        }
    }

    public void DisplaySideInfo(string text)
    {
        sideInfo.GetComponent<TextMeshProUGUI>().text = text;
        sideInfo.SetActive(true);
        
        sideInfo.GetComponent<Animation>().Play();
    }


    public void ShowConfirmPanel()
    {
        confirmPanel.SetActive(true);
    }

    public void HideConfirmPanel()
    {
        confirmPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}
