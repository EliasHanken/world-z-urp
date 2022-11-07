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

    void Start()
    {
    }

    public void SetGraphicPreset(GraphicSettingPreset graphicSettingPreset){
        this.graphicSettingPreset = graphicSettingPreset;
    }

}
