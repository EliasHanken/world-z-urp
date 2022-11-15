using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour
{
    public TextMeshProUGUI description;
    public TextMeshProUGUI progressText;
    public ObjectiveHandler objectiveHandler;
    public enum Status{
        Inactive,
        Ongoing,
        Passed
    }
    
    public Status status = Status.Inactive;
    public abstract void setStatus(Status status);
}
