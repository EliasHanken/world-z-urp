using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicCrosshair : MonoBehaviour
{
    private RectTransform reticle;
    public float restingSize;
    public float maxSize;
    public float changeSpeed;
    private float currentSize;
    public Rigidbody playerRb;
    public Camera playerCam;
    public float toolTipAndColorRange;

    public Color enemyTarget;
    public Color friendlyTarget;
    public Color defaultTarget;
    public TextMeshProUGUI tooltip;
    int interactable_layer;
    void Start()
    {
        reticle = GetComponent<RectTransform>();
        tooltip.text = "";
        interactable_layer = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRb.velocity.sqrMagnitude > 1){
            currentSize = Mathf.Lerp(currentSize, playerRb.velocity.sqrMagnitude * 5, Time.deltaTime * changeSpeed);
        }else{
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * changeSpeed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);

        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, toolTipAndColorRange)){
            if(hit.collider.GetComponentInParent<EnemyAi>() != null){
                foreach(Image img in reticle.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    cc.r = enemyTarget.r;
                    cc.g = enemyTarget.g;
                    cc.b = enemyTarget.b;

                    img.color = cc;

                    tooltip.text = hit.collider.GetComponentInParent<EnemyAi>().enemyName;
                    tooltip.color = cc;
                }
            }if(hit.collider.GetComponentInParent<Zombie>() != null){
                foreach(Image img in reticle.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    cc.r = enemyTarget.r;
                    cc.g = enemyTarget.g;
                    cc.b = enemyTarget.b;

                    img.color = cc;

                    tooltip.text = "Zombie";
                    tooltip.color = cc;
                }
            }
            else{
                foreach(Image img in reticle.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    cc.r = defaultTarget.r;
                    cc.g = defaultTarget.g;
                    cc.b = defaultTarget.b;

                    img.color = cc;

                    tooltip.color = cc;
                    tooltip.text = "";
                }
            }
        }else{
            foreach(Image img in reticle.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    cc.r = defaultTarget.r;
                    cc.g = defaultTarget.g;
                    cc.b = defaultTarget.b;

                    img.color = cc;

                    tooltip.color = cc;
                    tooltip.text = "";
                }
        }
    }
}
