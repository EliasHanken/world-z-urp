using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public enum ObjectiveType{
        entity_kills,
        target_kill
    }
    [SerializeField]private List<GameObject> objectiveList;
    public List<GameObject> instantiatedObj;
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
    public void TriggerUpdate(){
        foreach(GameObject obj in instantiatedObj){
            if(obj.GetComponent<ObjectiveKill>() != null){
                ObjectiveKill objectiveKill = obj.GetComponent<ObjectiveKill>();
                if(objectiveKill.kills >= objectiveKill.targetKills){
                    Destroy(obj);
                }
            }
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
        return list;
    }
}
