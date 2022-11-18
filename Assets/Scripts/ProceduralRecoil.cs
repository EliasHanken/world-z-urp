using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    public Transform cam;

    [SerializeField] public float recoilX;
    [SerializeField] public float recoilY;
    [SerializeField] public float recoilZ;

    [SerializeField] public float kickBackZ;

    public float snappiness, returnAmount;

    void Start(){
        initialGunPosition = transform.localPosition;
    }

    void Update(){
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);
        cam.localRotation = Quaternion.Euler(currentRotation);
        back();
    }

    public void recoil(){
        targetPosition -= new Vector3(0,0,kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ,recoilZ));
    }

    void back(){
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}
