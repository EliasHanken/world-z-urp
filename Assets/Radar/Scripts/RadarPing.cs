/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPing : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private float disappearTimer;
    private float disappearTimerMax;
    private Color color;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        disappearTimerMax = 1f;
        disappearTimer = 0f;
        color = new Color(1, 1, 1, 1f);
    }

    private void Update() {
        disappearTimer += Time.deltaTime;

        color.a = Mathf.Lerp(disappearTimerMax, 0f, disappearTimer / disappearTimerMax);
        spriteRenderer.color = color;

        if (disappearTimer >= disappearTimerMax) {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color) {
        this.color = color;
    }

    public void SetDisappearTimer(float disappearTimerMax) {
        this.disappearTimerMax = disappearTimerMax;
        disappearTimer = 0f;
    }

}
