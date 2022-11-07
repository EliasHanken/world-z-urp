using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    public enum GraphicSettingPreset
    {
        Poop,Medium,High,Ultra,Custom
    }
    public GraphicSettingPreset graphicSettingPreset = GraphicSettingPreset.High;
    public float Sensitivity = 100;
    public float Volume = 1;

    public int max_fps = 2;

    public int graphic_preset_int = 1;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (max_fps) * 60;
    }

    //void Update()
    //{
    //    Application.targetFrameRate = (max_fps) * 60;
    //    QualitySettings.SetQualityLevel(graphic_preset_int);
    //}

    public void SetGraphicPreset(GraphicSettingPreset graphicSettingPreset){
        this.graphicSettingPreset = graphicSettingPreset;
    }

    public void set_max_fps(int fps)
    {
        max_fps = fps;
        Application.targetFrameRate = (max_fps) * 60;
    }

    public void set_graphic_preset_int(int level)
    {
        graphic_preset_int = level;
        QualitySettings.SetQualityLevel(graphic_preset_int);
    }

}
