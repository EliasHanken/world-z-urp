using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public enum ObjectiveType{
        entity_kills,
        target_kill,
        interactable
    }
    [SerializeField]private List<GameObject> objectiveList;
    public List<GameObject> instantiatedObj;
    public GameObject missionComplete;
    private bool pauseTriggered = false;
    void Start()
    {
        instantiatedObj = new List<GameObject>();
        foreach(GameObject obj in objectiveList){
            GameObject ins = Instantiate(obj);
            ins.transform.parent = this.transform;
            instantiatedObj.Add(ins);
        }
        objectiveList.Clear();
    }

    void Update()
    {
        
    }
    public void TriggerUpdate(){
        
        foreach(GameObject obj in instantiatedObj){
            if(obj.GetComponent<ObjectiveKill>() != null){
                ObjectiveKill objectiveKill = obj.GetComponent<ObjectiveKill>();
                if(objectiveKill.kills >= objectiveKill.targetKills){
                    objectiveKill.status = Objective.Status.Passed;
                    StartCoroutine(destroyObjective(obj));
                }
            }
            if(obj.GetComponent<ObjectiveInteract>() != null){
                ObjectiveInteract objectiveInteract = obj.GetComponent<ObjectiveInteract>();
                if(objectiveInteract.status == Objective.Status.Passed){
                    StartCoroutine(destroyObjective(obj));
                }
            }
        }
        
        
    }

    IEnumerator destroyObjective(GameObject objective){
        
        if(instantiatedObj.Count == 1){
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>()._override = true;
            missionComplete.SetActive(true);
                Time.timeScale = 0.1f;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            //AudioListener.volume = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
            pauseTriggered = true;
        }
        bool destroyed = false;
        while(!destroyed){
            yield return new WaitForSeconds(2);
            instantiatedObj.Remove(objective);
            Destroy(objective);
            destroyed = true;
        }
    }

    public List<GameObject> getObjective(ObjectiveType type){
        List<GameObject> list = new List<GameObject>();
        if(type == ObjectiveType.entity_kills){
            foreach(GameObject obj in instantiatedObj){
                if(obj.GetComponent<ObjectiveKill>() != null){
                    list.Add(obj);
                }
            }
        }
        else if(type == ObjectiveType.interactable){
            foreach(GameObject obj in instantiatedObj){
                if(obj.GetComponent<ObjectiveInteract>() != null){
                    list.Add(obj);
                }
            }
        }
        return list;
    }
}
