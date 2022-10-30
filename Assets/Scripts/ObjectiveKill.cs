using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKill : Objective
{
    public string tag;
    public int kills;
    public int targetKills;
    public override void setStatus(Status status)
    {
        this.status = status;
    }

    // Start is called before the first frame update
    void Start()
    {
        setStatus(Status.Ongoing);
        objectiveHandler = GetComponentInParent<ObjectiveHandler>();
        progressText.text = kills + "/" + targetKills;
    }

    public void increaseProgressByOne(){
        kills++;
        progressText.text = kills + "/" + targetKills;
        objectiveHandler.TriggerUpdate();
    }
}
