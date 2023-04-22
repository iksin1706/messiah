using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using MalbersAnimations;



public class OptionsPanel : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI volumeText;
    public Sprite normalButton;
    public Sprite pressedButton;

    Image high;
    Image medium;
    Image low;
    Image yes;
    Image no;

    public enum GraphicQuality
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    GraphicQuality quality;
    float volume;
    bool isFullScreen=true;


    private void Awake()
    {
        LoadSettings();
    }
    void AssignElements()
    {
        high = transform.Find("high").GetComponent<Image>();
        medium = transform.Find("medium").GetComponent<Image>();
        low = transform.Find("low").GetComponent<Image>();
        yes = transform.Find("yes").GetComponent<Image>();
        no = transform.Find("no").GetComponent<Image>();
        Debug.Log(no);

    }
    void Update()
    {      
        volumeText.text = (volume * 100).ToString();
        
    }
    public void ChangeVolume()
    {           
        volume=slider.value;
        volume = Mathf.Round(volume * 100f) / 100f;
        volumeText.text = (volume * 100).ToString();
        AudioListener.volume = volume;
    }

    void UnpressQualityButtons()
    {
        low.sprite = normalButton;
        medium.sprite = normalButton;
        high.sprite = normalButton;
        var lowTextRect = low.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        lowTextRect.offsetMin = new Vector2(lowTextRect.offsetMin.x, 7.75f);

        var mediumTextRect = medium.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        mediumTextRect.offsetMin = new Vector2(mediumTextRect.offsetMin.x, 7.75f);

        var highTextRect = high.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        highTextRect.offsetMin = new Vector2(highTextRect.offsetMin.x, 7.75f);
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("quality", (int)quality);
        switch (quality)
        {
            case GraphicQuality.Low:
                SetLowQuality();
                LowQualityClick();
                break;
            case GraphicQuality.Medium:
                SetMediumQuality();
                MediumQualityClick();
                break;
            case GraphicQuality.High:
                
                HighQualityClick();
                break;
        }
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("fullScreen", Convert.ToInt32(isFullScreen));
        SetFullScreen(isFullScreen);
    }

   


    public void LowQualityClick()
    {
        quality = GraphicQuality.Low;
        UnpressQualityButtons();
        low.sprite = pressedButton;
        var lowTextRect = low.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        lowTextRect.offsetMin = new Vector2(lowTextRect.offsetMin.x,0f);
        SetLowQuality();

    }
    public void MediumQualityClick()
    {
        quality = GraphicQuality.Medium;
        UnpressQualityButtons();
        medium.sprite = pressedButton;
        var mediumTextRect = medium.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        mediumTextRect.offsetMin = new Vector2(mediumTextRect.offsetMin.x, 0f);
        SetMediumQuality();
    }
    public void HighQualityClick()
    {
        quality = GraphicQuality.High;
        UnpressQualityButtons();
        high.sprite = pressedButton;
        var highTextRect = high.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        highTextRect.offsetMin = new Vector2(highTextRect.offsetMin.x,0f);
        SetHighQuality();
    }

    public void SetFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            Screen.fullScreen = true;
            isFullScreen = true;

            var noRect = no.gameObject.transform.Find("Text").GetComponent<RectTransform>();
            noRect.offsetMin = new Vector2(noRect.offsetMin.x, 7.75f);

            var yesRect = yes.gameObject.transform.Find("Text").GetComponent<RectTransform>();
            yesRect.offsetMin = new Vector2(yesRect.offsetMin.x, 0f);

            yes.sprite = pressedButton;
            no.sprite = normalButton;
        }
        else
        {
            Screen.fullScreen = false;
            isFullScreen = false;
            Debug.Log(no);
            var noRect = no.gameObject.transform.Find("Text").GetComponent<RectTransform>();
            noRect.offsetMin = new Vector2(noRect.offsetMin.x,0);

            var yesRect = yes.gameObject.transform.Find("Text").GetComponent<RectTransform>();
            yesRect.offsetMin = new Vector2(yesRect.offsetMin.x, 7.75f);

            no.sprite = pressedButton;
            yes.sprite = normalButton;

        }
        Debug.Log("GIT");
    }


    public void SetHighQuality()
    {
        TurnOnPostProcessing();
        TurnOnShadows();
        TurnOnWater();
    }
    public void SetMediumQuality()
    {
        TurnOnPostProcessing();
        TurnOffShadows();
        TurnOffWater();
    }
    public void SetLowQuality()
    {
        TurnOffPostProcessing();
        TurnOffShadows();
        TurnOffWater();
    }
    public void TurnOnPostProcessing()
    {
        var postProcesses = Resources.FindObjectsOfTypeAll(typeof(PostProcessVolume));
        foreach (var item in postProcesses)
        {
            ((PostProcessVolume)item).enabled = true;
        }
    }
    public void TurnOffPostProcessing()
    {
        var postProcesses = Resources.FindObjectsOfTypeAll(typeof(PostProcessVolume));
        foreach (var item in postProcesses)
        {
            ((PostProcessVolume)item).enabled = false;
        }
    }


    private void TurnOffShadows()
    {
        var lights = FindObjectsOfType<Light>();
        foreach (var item in lights)
        {
            item.shadows = LightShadows.None;
        }
    }
    private void TurnOnShadows()
    {
        var lights = FindObjectsOfType<Light>();
        foreach (var item in lights)
        {
            item.shadows = LightShadows.Soft;
        }
    }
    private void TurnOnWater()
    {
        var waters = FindObjectsOfType<Water>();
        foreach (var item in waters)
        {
            item.waterMode = Water.WaterMode.Refractive;
        }
    }
    private void TurnOffWater()
    {
        var waters = FindObjectsOfType<Water>();
        foreach (var item in waters)
        {
            item.waterMode = Water.WaterMode.Simple;
        }
    }


    public void LoadSettings()
    {
        Debug.Log(PlayerPrefs.GetFloat("volume"));
        if (PlayerPrefs.HasKey("quality"))  quality = (GraphicQuality)PlayerPrefs.GetInt("quality");
        else quality = GraphicQuality.High;

        switch (quality)
        {
            case GraphicQuality.Low:
                SetLowQuality();
                break;
            case GraphicQuality.Medium:
                SetMediumQuality();
                break;
            case GraphicQuality.High:
                SetHighQuality();
                break;
        }

        if (PlayerPrefs.HasKey("volume"))
        {
            volume = PlayerPrefs.GetFloat("volume");
            
        }
        else volume = 0.5f;

        if (PlayerPrefs.HasKey("fullScreen")) isFullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("fullScreen"));
        else isFullScreen=true;
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        AssignElements();
        switch (quality)
        {
            case GraphicQuality.Low:
                LowQualityClick();
                break;
            case GraphicQuality.Medium:
                MediumQualityClick();
                break;
            case GraphicQuality.High:
                HighQualityClick();
                break;
        }
        slider.value = volume;

        SetFullScreen(isFullScreen);

    }

    // Update is called once per frame

}
