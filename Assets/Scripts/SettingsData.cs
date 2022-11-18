using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public float sensitivity;
    public float adsMultiplier;
    public float volumeSFX;
    public float volumeMaster;
    public float volumeMusic;
    public int qualityPresetIndex;
    public int resolutionIndex;
    public int maxFPSIndex;

    public SettingsData(float adsMultiplier, float sensitivity,float volumeSFX,float volumeMaster,float volumeMusic,int resolutionIndex,int qualityPresetIndex,int maxFPSIndex)
    {
        this.adsMultiplier = adsMultiplier;
        this.sensitivity = sensitivity;
        this.volumeSFX = volumeSFX;
        this.volumeMaster = volumeMaster;
        this.volumeMusic = volumeMusic;
        this.qualityPresetIndex = qualityPresetIndex;
        this.maxFPSIndex = maxFPSIndex;
        this.resolutionIndex = resolutionIndex;
    }
}
