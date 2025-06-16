using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject OptionCanvas;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;


    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private  Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextvalue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;


    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPrompt = null;

    [Header("Levels To Load")]
    private string levelToLoad;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        DialogManager.Instance.LoadingDialog.Hide();
    }


    #region OnClick

    public void OnClick_NewGame()
    {
        LoadGamePlayScene(DataStore.FIRST_GAME_LEVEL);
    }

    public void OnClick_LoadGame()
    {
        levelToLoad = PlayerPrefs.GetString("SavedLevel");

        if(levelToLoad == null || levelToLoad == "")
        {
            DialogManager.Instance.ConfirmOnlyDialog.Show(
                "",
                "No Save File Found",
                null
            );
            return;
        }

        DialogManager.Instance.YesNoDialog.Show(
            "",
            "Are you sure you want to load?",
            delegate(bool isTrue)
            {
                if (isTrue)
                {
                    levelToLoad = PlayerPrefs.GetString("SavedLevel");
                    LoadGamePlayScene(levelToLoad);
                }
            }
        );
    }


    public void OnClick_Option()
    {
        this.OptionCanvas.gameObject.SetActive(true);
    }

    public void OnClick_Exit()
    {
        DialogManager.Instance.YesNoDialog.Show(
            "",
            "Are you sure you want to leave?",
            delegate (bool isTrue)
            {
                if (isTrue)
                {
                    Application.Quit();
                }
            }
        );
    }


    #endregion




    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    protected void LoadGamePlayScene(string sceneName)
    {
        MainMenuCanvas.gameObject.SetActive(false);
        DialogManager.Instance.LoadingDialog.Show();
        DataStore.Instance.CurrentBattleScene = sceneName;
        SceneManager.LoadScene(DataStore.BATTLE_SCENE);
    }


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            //invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            //Not invert
        }

        PlayerPrefs.SetFloat("masterSen",mainControllerSen);
        StartCoroutine (ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextvalue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }
    
    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GrpahicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //change your brightness with your post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine (ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextvalue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GrpahicsApply();
        }

        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        
        if(MenuType == "Gameplay")
        {
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }

    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPrompt.SetActive(false);
    }
}