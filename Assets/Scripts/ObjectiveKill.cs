using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKill : Objective
{
    public string _tag;
    public int kills;
    public int targetKills;
    public RectTransform rect_transform;

    private Vector3 notVisible, visible;
    public override void setStatus(Status status)
    {
        this.status = status;
    }
    void Update(){
        switch(status){
            case Status.Inactive:
                break;
            case Status.Ongoing:
                float x = Mathf.Lerp(rect_transform.position.x,120,5 * Time.deltaTime);
                Vector3 newVec = rect_transform.position;
                newVec.x = x;
                rect_transform.position = newVec;
                break;
            case Status.Passed:
                float x2 = Mathf.Lerp(rect_transform.position.x,-120,5 * Time.deltaTime);
                Vector3 newVec2 = rect_transform.position;
                newVec2.x = x2;
                rect_transform.position = newVec2;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        visible = rect_transform.position;
        Vector3 notV = visible;
        notV.x -= 220;
        notVisible = notV;

        rect_transform.position = notVisible;
        setStatus(Status.Ongoing);

        status = Status.Ongoing;

        objectiveHandler = GetComponentInParent<ObjectiveHandler>();
        progressText.text = kills + "/" + targetKills;
    }

    public void increaseProgressByOne(){
        kills++;
        progressText.text = kills + "/" + targetKills;
        objectiveHandler.TriggerUpdate();
    }
}
