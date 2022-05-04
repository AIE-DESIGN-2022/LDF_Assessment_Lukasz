using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent Tiggers;
    [SerializeField] Objective[] objectivesToActivate;
    [SerializeField] bool destroyOnTrigger = false;

    bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != GameObject.FindGameObjectWithTag("Player").transform) return;
        TriggerEvent();
    }

    public void DestroyTrigger()
    {
        Destroy(gameObject);
    }

    public void TriggerEvent()
    {
        if (hasTriggered) return;
        //print("Triggering " + gameObject.name);
        hasTriggered = true;
        Tiggers?.Invoke();
        ActivateObjectives();
        if (destroyOnTrigger) DestroyTrigger();
    }

    private void ActivateObjectives()
    {
        foreach(Objective objective in objectivesToActivate)
        {
            objective.ActivateObjective();
        }
    }
    
}
