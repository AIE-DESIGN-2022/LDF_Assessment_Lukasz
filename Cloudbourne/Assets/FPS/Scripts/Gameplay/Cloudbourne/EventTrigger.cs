using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent Tiggers;

    private void OnTriggerEnter(Collider other)
    {
        Tiggers.Invoke();
    }
}
