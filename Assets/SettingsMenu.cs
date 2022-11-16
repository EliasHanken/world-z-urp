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

        int max_fps = Application.targetFrameRate;
        if(max_fps == -1){
            maxFPSDropdown.value = 4;
        }
        SetMaxFps(4);
        maxFPSDropdown.RefreshShownValue();
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
}
