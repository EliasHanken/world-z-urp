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

    public AudioClip settingPressSound;
    public GraphicSettingPreset graphicSettingPreset = GraphicSettingPreset.High;
    public float Sensitivity = 100;
    public float Volume = 1;

    public int max_fps = 5;

    public int graphic_preset_int = 1;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = max_fps;
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

        GameObject instantiatedAS = new GameObject();
        instantiatedAS.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        AudioSource audioSource = instantiatedAS.AddComponent<AudioSource>();
                        AudioReverbFilter filter = instantiatedAS.AddComponent<AudioReverbFilter>();
                        filter.reverbPreset = AudioReverbPreset.Arena;
                        audioSource.spatialBlend = 0.0f;
                        audioSource.pitch = 0.7f;
                        audioSource.clip = settingPressSound;
                        audioSource.Play();
                        StartCoroutine(DestroyComponent(instantiatedAS));
    }

    IEnumerator DestroyComponent(GameObject go){
        yield return new WaitForSeconds(4f);
        //Destroy(go);
    }

}
