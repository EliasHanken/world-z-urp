using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown maxFPSDropdown;
    public Slider volumeSFX;
    public Slider volumeMaster;
    public Slider volumeMusic;
    public Slider sensitivitySlider;
    public Slider adsSlider;

    // private fields
    private float _sens;
    private float _ads;

    void Start(){
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        //int max_fps = Application.targetFrameRate;
        maxFPSDropdown.value = 4;
        maxFPSDropdown.RefreshShownValue();
        SetMaxFps(4);
        

        sensitivitySlider.value = 500f;
        adsSlider.value = 0.7f;

        if(SaveSystem.LoadSettingsData() != null){
            LoadSettings();
        }

    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    public void SetVolumeMusic(float volume){
        float dbVolume = Mathf.Log10(volume) * 20;
        if (volume == 0.0f){
            dbVolume = -80.0f;
        }
        audioMixer.SetFloat("volumeMusic", dbVolume);
    }

    public float getCorrectDBValue(float volume){
        float dbVolume = Mathf.Log10(volume) * 20;
        if (volume == 0.0f){
            dbVolume = -80.0f;
        }
        return dbVolume;
    }
    public void SetVolumeSFX(float volume){
        float dbVolume = Mathf.Log10(volume) * 20;
        if (volume == 0.0f){
            dbVolume = -80.0f;
        }
        audioMixer.SetFloat("volumeSFX", dbVolume);
    }
    public void SetVolumeMaster(float volume){
        float dbVolume = Mathf.Log10(volume) * 20;
        if (volume == 0.0f){
            dbVolume = -80.0f;
        }
        audioMixer.SetFloat("volumeMaster", dbVolume);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }

    public void SetMaxFps(int maxFps){
        if(maxFps == 0){
            Application.targetFrameRate = 30;
        }else{
            Application.targetFrameRate = maxFps * 60;
        }
        
    }

    public void SetSens(float sens){
        this._sens = sens;
    }

    public void SetAds(float ads){
        this._ads = ads;
    }

    public void SaveSettings(){
        SettingsData settingsData = new SettingsData(adsSlider.value,sensitivitySlider.value,
            volumeSFX.value,volumeMaster.value,volumeMusic.value,
            resolutionDropdown.value,
            qualityDropdown.value,
            maxFPSDropdown.value
        );
        SaveSystem.SaveSettings(settingsData);
    }

    public void LoadSettings(){
        SettingsData settingsData = SaveSystem.LoadSettingsData();

            // Set dropdowns
            resolutionDropdown.value = settingsData.resolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(settingsData.resolutionIndex);

            qualityDropdown.value = settingsData.qualityPresetIndex;
            qualityDropdown.RefreshShownValue();
            SetQuality(settingsData.qualityPresetIndex);

            maxFPSDropdown.value = settingsData.maxFPSIndex;
            maxFPSDropdown.RefreshShownValue();
            SetMaxFps(settingsData.maxFPSIndex);

            SetVolumeMaster(settingsData.volumeMaster);
            SetVolumeMusic(settingsData.volumeMusic);
            SetVolumeSFX(settingsData.volumeSFX);

            volumeSFX.value = settingsData.volumeSFX;
            volumeMaster.value = settingsData.volumeMaster;
            volumeMusic.value = settingsData.volumeMusic;

            _sens = settingsData.sensitivity;
            sensitivitySlider.value = settingsData.sensitivity;

            _ads = settingsData.adsMultiplier;
            adsSlider.value = settingsData.adsMultiplier;
    }
}
