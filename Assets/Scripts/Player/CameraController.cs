using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public float mouseADSmultiplier = 0.7f;
    public Transform playerBody;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if(SaveSystem.LoadSettingsData() != null){
            SettingsData settingsData = SaveSystem.LoadSettingsData();
            mouseSensitivity = settingsData.sensitivity;
            mouseADSmultiplier = settingsData.adsMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f,90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void updateSens(){
        if(SaveSystem.LoadSettingsData() != null){
            SettingsData settingsData = SaveSystem.LoadSettingsData();
            mouseSensitivity = settingsData.sensitivity;
            mouseADSmultiplier = settingsData.adsMultiplier;
        }
    }
}
