using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/OptionMenu_Dialog")]
public class OptionMenu_Dialog : DialogBase
{

    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SoundSlider;

    private Dictionary<string, Resolution> allowResolution = new Dictionary<string, Resolution>()
    {
        {"2560 X 1440", new Resolution(){width = 2560, height = 1440} },
        {"1920 X 1080", new Resolution(){width = 2560, height = 1440} },
        {"1280 X 720", new Resolution(){width = 1280, height = 720} },
    };

    private Dictionary<string, FullScreenMode> allowDisplayMode = new Dictionary<string, FullScreenMode>()
    {
        { "Fullscreen", FullScreenMode.ExclusiveFullScreen },
        { "Windowed", FullScreenMode.Windowed },
        { "Fullscreen Window", FullScreenMode.FullScreenWindow }
    };

    [SerializeField, ReadOnly] private bool hasLoadedFromPlayerPref = false;
    [SerializeField, ReadOnly] private string currentResolution = "";
    [SerializeField, ReadOnly] private string currentDisplayMode = "";
    [SerializeField, ReadOnly] private float bgmValue = 0.8f;
    [SerializeField, ReadOnly] private float soundValue = 0.8f;


    private Action OnClose = null;

    #region Init And Functions

    public void InitData(Action OnClose)
    {
        this.OnClose = OnClose;
        if (hasLoadedFromPlayerPref)
        {
            return;
        }

        CollectOptions();

        SetDataToDefault();
        LoadPlayerPrefOrDefault();

        FillDataToUI();

        hasLoadedFromPlayerPref = true;
    }

    private void CollectOptions()
    {
        List<string> resOptions = new List<string>();
        foreach(var reso in this.allowResolution)
        {
            resOptions.Add(reso.Key);
        }
        resolutionDropDown.ClearOptions();
        resolutionDropDown.AddOptions(resOptions);


        List<string> displayOptions = new List<string>();
        foreach(var dm in allowDisplayMode)
        {
            displayOptions.Add(dm.Key);
        }
        displayModeDropDown.ClearOptions();
        displayModeDropDown.AddOptions(displayOptions);
    }

    private void FillDataToUI()
    {
        displayModeDropDown.value = displayModeDropDown.options.FindIndex(a => a.text == currentDisplayMode);
        resolutionDropDown.value = resolutionDropDown.options.FindIndex(a => a.text == currentResolution);
        BGMSlider.value = bgmValue;
        SoundSlider.value = soundValue;
    }

    private void SetDataToDefault()
    {
        currentDisplayMode =  "Windowed";
        currentResolution ="1920 x 1080";
        bgmValue = 0.8f;
        soundValue = 0.8f;
    }
    private void LoadPlayerPrefOrDefault()
    {
        bgmValue = PlayerPrefs.GetFloat(DataStore.PREF_BGM_TAG, 0.8f);
        soundValue = PlayerPrefs.GetFloat(DataStore.PREF_SOUND_TAG, 0.8f);
        currentResolution = PlayerPrefs.GetString(DataStore.PREF_RESOLUTION_TAG, "1920 x 1080");
        currentDisplayMode = PlayerPrefs.GetString(DataStore.PREF_DISPLAY_MODE_TAG, "Windowed");
    }

    private void SavePlayerPref()
    {
        PlayerPrefs.SetFloat(DataStore.PREF_BGM_TAG, bgmValue);
        PlayerPrefs.SetFloat(DataStore.PREF_SOUND_TAG, soundValue);
        PlayerPrefs.SetString(DataStore.PREF_RESOLUTION_TAG, currentResolution);
        PlayerPrefs.SetString(DataStore.PREF_DISPLAY_MODE_TAG, currentDisplayMode);
        PlayerPrefs.Save();
    }

    #endregion

    #region Option Change

    public void OnChange_DisplayMode()
    {
        Debug.LogError($"OnChange_DisplayMode => {this.displayModeDropDown.value}");
        if (allowResolution.ContainsKey(currentResolution)
            && allowDisplayMode.ContainsKey(currentDisplayMode))
        {
            Screen.SetResolution(
                allowResolution[currentResolution].width,
                allowResolution[currentResolution].height,
                allowDisplayMode[currentDisplayMode]
            );
        }
        if(hasLoadedFromPlayerPref) AudioManager.Instance.PlaySFX(DataStore.SFX_BUTTON_CLICK);
    }
    public void OnChange_Resolution()
    {
        Debug.LogError($"OnChange_DisplayMode => {this.resolutionDropDown.value}");
        if (allowResolution.ContainsKey(currentResolution)
            && allowDisplayMode.ContainsKey(currentDisplayMode)) {
            Screen.SetResolution(
                allowResolution[currentResolution].width, 
                allowResolution[currentResolution].height,
                allowDisplayMode[currentDisplayMode]
            );
        }
        if (hasLoadedFromPlayerPref) AudioManager.Instance.PlaySFX(DataStore.SFX_BUTTON_CLICK);
    }
    public void OnChange_BGM()
    {
        Debug.Log($"OnChange_BGM => {BGMSlider.value}");
        bgmValue = BGMSlider.value;
        AudioManager.Instance.SetBGMVolume(bgmValue);
    }
    public void OnChange_Sound()
    {
        Debug.Log($"OnChange_BGM => {SoundSlider.value}");
        soundValue = SoundSlider.value;
        AudioManager.Instance.SetSFXVolume(soundValue);

        //AudioManager.Instance.PlaySFX(DataStore.SFX_VICTORY);
    }

    #endregion

    #region Buttons

    public void OnClick_Apply()
    {
        SavePlayerPref();
        SelfHide(false);
        this.OnClose?.Invoke();
    }
    public void OnClick_Cancel()
    {
        LoadPlayerPrefOrDefault();
        SelfHide(false);
        this.OnClose?.Invoke();
    }
    public void OnClick_ResetToDefault()
    {
        SetDataToDefault();
        FillDataToUI();
    }

    #endregion
}
