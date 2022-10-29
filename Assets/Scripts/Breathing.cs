using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public AnimationCurve verticalCurve, horizontalCurve;
    public float amplitude, timeElapsed, frequency, rotationalAmplitude, phaseShift, horizontalAmplitude, motionSpeed;
    public float multiplier;

    void Start(){
        multiplier = 1;
    }

    void Update(){
        timeElapsed += Time.deltaTime;

        Vector3 pos = new Vector3(horizontalCurve.Evaluate(timeElapsed) * horizontalAmplitude * motionSpeed, horizontalCurve.Evaluate(timeElapsed * frequency) * amplitude, transform.localPosition.z);
        transform.localPosition = pos;

        Quaternion rot = Quaternion.Euler(verticalCurve.Evaluate(timeElapsed * frequency + phaseShift) * rotationalAmplitude,0,0);
    }
}
