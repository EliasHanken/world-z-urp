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
using UnityEngine.UI;

public class Radar : MonoBehaviour {

    [SerializeField] private Transform pfRadarPing;
    [SerializeField] private LayerMask radarLayerMask;

    private Transform sweepTransform;
    [SerializeField]
    private float rotationSpeed = 180f;
    private List<Collider2D> colliderList;
    public float radarDistance = 20, blipSize = 15;
    public bool usePlayerDirection = true;
    private bool remove_blips = false;
    public Transform player;
    public GameObject blipRedPrefab,blipGreenPrefab,blipBluePrefab,blipYellowPrefab;
    public string redBlipTag = "Zombie", greenBlipTag = "NPC", blueBlipTag = "Player", yellowBlipTag = "Objective";
 
    private float radarWidth, radarHeight, blipWidth, blipHeight;

    private void Awake() {
        sweepTransform = transform.Find("Sweep");
        colliderList = new List<Collider2D>();
    }

    private void Start(){
        radarWidth  = GetComponent<RectTransform>().rect.width;
        radarHeight = GetComponent<RectTransform>().rect.height;
        blipHeight  = radarHeight * blipSize/100;
        blipWidth   = radarWidth * blipSize/100;

        StartCoroutine(displayBlipsEachTime(2,2f));
    }

    private void Update() {
        float previousRotation = (sweepTransform.eulerAngles.z % 360) - 180;
        sweepTransform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        float currentRotation = (sweepTransform.eulerAngles.z % 360) - 180;
        if (previousRotation < 0 && currentRotation >= 0) {
            // Half rotation
            colliderList.Clear();
        }

        if(remove_blips){
            GameObject[] blips = GameObject.FindGameObjectsWithTag("RadarBlip");
            
            foreach (GameObject blip in blips){
                Color newColor = blip.GetComponent<RawImage>().color;
                newColor.a = 0;
                blip.GetComponent<RawImage>().color = Color.Lerp(blip.GetComponent<RawImage>().color,newColor,2*Time.deltaTime);
            }
        }
    }

    IEnumerator displayBlipsEachTime(float time, float alive){
        bool displayed = false;;
        while(true){
            if(displayed){
                remove_blips = true;
                yield return new WaitForSeconds(alive);
                RemoveAllBlips();
                displayed = false;
            }
            yield return new WaitForSeconds(time);
            
            displayed = true;
            DisplayBlips(redBlipTag, blipRedPrefab);
            DisplayBlips(greenBlipTag, blipGreenPrefab); 
            DisplayBlips(blueBlipTag, blipBluePrefab); 
            DisplayBlips(yellowBlipTag, blipYellowPrefab); 
        }
    }
    private void DisplayBlips(string tag, GameObject prefabBlip) {
        Vector3 playerPos = player.position;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
 
        foreach (GameObject target in targets) {
            Vector3 targetPos = target.transform.position;
            float distanceToTarget = Vector3.Distance(targetPos, playerPos);
 
            if(distanceToTarget <= radarDistance) {
 
                Vector3 normalisedTargetPosition = NormalisedPosition(playerPos, targetPos);
                Vector2 blipPosition = CalculateBlipPosition(normalisedTargetPosition);
                if(target.GetComponent<EntityHealth>() != null){
                    if(target.GetComponent<EntityHealth>().health > 0){
                        DrawBlip(blipPosition, prefabBlip);
                    }
                }else{
                    DrawBlip(blipPosition, prefabBlip);
                }
                
            }
        }
    }
     
    private void RemoveAllBlips() {
        GameObject[] blips = GameObject.FindGameObjectsWithTag("RadarBlip");
        foreach (GameObject blip in blips)
            Destroy(blip);
    }
 
    private Vector3 NormalisedPosition(Vector3 playerPos, Vector3 targetPos) {
        float normalisedTargetX = (targetPos.x - playerPos.x)/radarDistance;
        float normalisedTargetZ = (targetPos.z - playerPos.z)/radarDistance;
         
        return new Vector3(normalisedTargetX, 0, normalisedTargetZ);
    }
 
    private Vector2 CalculateBlipPosition(Vector3 targetPos) {
        // find the angle from the player to the target.
        float angleToTarget = Mathf.Atan2(targetPos.x,targetPos.z) * Mathf.Rad2Deg;
 
        // The direction the player is facing.
        float anglePlayer = usePlayerDirection? player.eulerAngles.y : 0;
 
        // Subtract the player angle, to get the relative angle to the object. Subtract 90
        // so 0 degrees (the same direction as the player) is Up.
        float angleRadarDegrees = angleToTarget - anglePlayer - 90;
 
        // Calculate the xy position given the angle and the distance.
        float normalisedDistanceToTarget = targetPos.magnitude;
        float angleRadians = angleRadarDegrees * Mathf.Deg2Rad;
        float blipX = normalisedDistanceToTarget * Mathf.Cos(angleRadians);
        float blipY = normalisedDistanceToTarget * Mathf.Sin(angleRadians);
 
        // Scale the blip position according to the radar size.
        blipX *= radarWidth*.5f;
        blipY *= radarHeight*.5f;
 
        // Offset the blip position relative to the radar center
        blipX += (radarWidth*.5f) - blipWidth*.5f;
        blipY += (radarHeight*.5f) - blipHeight*.5f;
 
        return new Vector2(blipX, blipY);
    }
 
    private void DrawBlip(Vector2 pos, GameObject blipPrefab) {
        GameObject blip = (GameObject) Instantiate(blipPrefab);
        blip.transform.SetParent(transform);
        RectTransform rt = blip.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,pos.x, blipWidth);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,pos.y, blipHeight);
    }
}
