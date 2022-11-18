using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SaveSettings(SettingsData settingsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.data";
        FileStream stream = new FileStream(path,FileMode.Create);

        formatter.Serialize(stream,settingsData);
        stream.Close();
    }

    public static SettingsData LoadSettingsData(){
        string path = Application.persistentDataPath + "/settings.data";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            SettingsData settingsData = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return settingsData;
        }else{
            return null;
        }
    }
}
