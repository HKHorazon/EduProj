using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ResPath("Dialog/OptionMenu_Dialog")]
public class OptionMenu_Dialog : DialogBase
{

    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private CanvasGroup areaResetProgress;

    private Dictionary<string, Resolution> allowResolution = new Dictionary<string, Resolution>()
    {
        {"2560 X 1440", new Resolution(){width = 2560, height = 1440} },
        {"1920 X 1080", new Resolution(){width = 1920, height = 1080} },
        {"1280 X 720", new Resolution(){width = 1280, height = 720} },
    };

    private Dictionary<string, FullScreenMode> allowDisplayMode = new Dictionary<string, FullScreenMode>()
    {
        { "全螢幕", FullScreenMode.ExclusiveFullScreen },
        { "視窗", FullScreenMode.Windowed },
        { "全螢幕視窗", FullScreenMode.FullScreenWindow }
    };

    [SerializeField, ReadOnly] private bool hasLoadedFromPlayerPref = false;
    [SerializeField, ReadOnly] private string currentResolution = "";
    [SerializeField, ReadOnly] private string currentDisplayMode = "";
    [SerializeField, ReadOnly] private float bgmValue = 0.8f;
    [SerializeField, ReadOnly] private float soundValue = 0.8f;


    private Action OnClose = null;

    #region Init And Functions

    public void StartGameLoading()
    {
        if (hasLoadedFromPlayerPref)
        {
            return;
        }
        SetDataToDefault();
        LoadPlayerPrefOrDefault();

        hasLoadedFromPlayerPref = true;
    }

    public void InitData(bool showReset, Action OnClose)
    {
        this.OnClose = OnClose;

        StartGameLoading();

        areaResetProgress.alpha = showReset ? 1 : 0;
        CollectOptions();

        FillDataToUI();

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
        currentDisplayMode = "視窗";
        currentResolution ="1920 x 1080";
        bgmValue = 0.8f;
        soundValue = 0.8f;
    }
    private void LoadPlayerPrefOrDefault()
    {
        bgmValue = PlayerPrefs.GetFloat(DataStore.PREF_BGM_TAG, 0.8f);
        soundValue = PlayerPrefs.GetFloat(DataStore.PREF_SOUND_TAG, 0.8f);
        currentResolution = PlayerPrefs.GetString(DataStore.PREF_RESOLUTION_TAG, "1920 x 1080");
        currentDisplayMode = PlayerPrefs.GetString(DataStore.PREF_DISPLAY_MODE_TAG, "視窗");
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
        string displayText = this.displayModeDropDown.options[this.displayModeDropDown.value].text;

        Debug.LogError($"OnChange_Resolution => {displayText}");

        if (allowDisplayMode.ContainsKey(displayText))
        {
            currentDisplayMode = displayText;
        }

        if (allowResolution.ContainsKey(currentResolution)
            && allowDisplayMode.ContainsKey(currentDisplayMode))
        {
            Screen.SetResolution(
                allowResolution[currentResolution].width,
                allowResolution[currentResolution].height,
                allowDisplayMode[currentDisplayMode]
            );
            Debug.LogError($"Screen {allowDisplayMode[currentDisplayMode]} :　{allowResolution[currentResolution].width}x{allowResolution[currentResolution].height}");
        }
        if(hasLoadedFromPlayerPref) AudioManager.Instance.PlaySFX(DataStore.SFX_BUTTON_CLICK);
    }
    public void OnChange_Resolution()
    {
        string displayText = this.resolutionDropDown.options[this.resolutionDropDown.value].text;

        Debug.LogError($"OnChange_Resolution => {displayText}");

        if (allowResolution.ContainsKey(displayText))
        {
            currentResolution = displayText;
        }

        if (allowResolution.ContainsKey(currentResolution)
            && allowDisplayMode.ContainsKey(currentDisplayMode)) {
            Screen.SetResolution(
                allowResolution[currentResolution].width, 
                allowResolution[currentResolution].height,
                allowDisplayMode[currentDisplayMode]
            );
            Debug.LogError($"Screen {allowDisplayMode[currentDisplayMode]} :　{allowResolution[currentResolution].width}x{allowResolution[currentResolution].height}");
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

    public void OnClick_ResetGameProgress()
    {
        this.SelfHide(true);
        YesNo_Dialog ynDialog = DialogManager.Instance.Show<YesNo_Dialog>();
        ynDialog.SetData(
            "",
            ConstString.RESET_ALL_PROGRESS_PROMT,
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    PlayerPrefs.DeleteKey(DataStore.PREF_SAVED_LEVEL);
                    PlayerPrefs.Save();
                    ConfirmOnly_Dialog cDialog = DialogManager.Instance.Show<ConfirmOnly_Dialog>();
                    cDialog.SetData(
                        "",
                        "All stages have been reset.",
                        null
                    );
                    DialogManager.Instance.Show<OptionMenu_Dialog>();
                }
                else
                {
                    DialogManager.Instance.Show<OptionMenu_Dialog>();
                }
            }
        );
    }
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
