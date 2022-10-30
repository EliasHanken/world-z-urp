using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarRotate : MonoBehaviour {

    private void Update() {
        transform.eulerAngles += new Vector3(0, 0, -180f * Time.deltaTime);
    }

}
