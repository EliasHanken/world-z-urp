using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Tilt Sway")]
    public float amount;
    public float maxSway;
    public float smoothAmount;

    [Space]

    [Header("Rotational Sway")]
    public float tiltAmount;
    public float maxTiltSway;
    public float smoothAmountTilt;
    public bool tiltDirX, tiltDirY, tiltDirZ;

    Vector3 initialPos;
    Quaternion initialRot;

    void Start(){
        initialRot = transform.localRotation;
        initialPos = transform.localPosition;
    }

    void Update(){
        tiltSway();
        rotationalSway();
    }

    void tiltSway(){
        float moveX = Input.GetAxis("Mouse X") * amount;
        float moveY = Input.GetAxis("Mouse Y") * amount;
        moveX = Mathf.Clamp(moveX, -maxSway, maxSway);
        Vector3 finalPos = new Vector3(moveX, 0, moveY);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothAmount);
    }

    void rotationalSway(){
        float tiltY = Input.GetAxis("Mouse X") * tiltAmount;
        float tiltX = Input.GetAxis("Mouse Y") * tiltAmount;
        tiltY = Mathf.Clamp(tiltY, -maxTiltSway, maxTiltSway);
        tiltX = Mathf.Clamp(tiltX, -maxTiltSway, maxTiltSway);
        Quaternion finalRot = Quaternion.Euler(new Vector3(tiltDirX ? -tiltX : 0, tiltDirY ? tiltY : 0, tiltDirZ ? tiltY : 0));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot, Time.deltaTime * smoothAmountTilt);
    }
}
