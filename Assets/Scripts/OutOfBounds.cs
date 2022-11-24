using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField]
    private float radius = 100.0f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private TextMeshProUGUI promptMessage;

    private bool coroutineStarted = false;
    private bool isOutOfBounds = false;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance >= radius && !coroutineStarted){
            isOutOfBounds = true;
            StartCoroutine(DealDamage());
        }else if(distance < radius){
            promptMessage.text = "";
            isOutOfBounds = false;
            coroutineStarted = false;
        }
    }

    IEnumerator DealDamage(){
        coroutineStarted = true;
        while(isOutOfBounds){
            if(player.GetComponent<EntityHealth>() != null){
                player.GetComponent<EntityHealth>().TakeDamage(2,false);
                promptMessage.text = "Out of bounds, return!";
                yield return new WaitForSeconds(2);
            }
            
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
