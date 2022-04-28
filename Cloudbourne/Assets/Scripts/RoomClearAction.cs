using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomClearAction : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public UnityEvent OnClearEvent;
    private bool triggered;

    private void Update()
    {
        if (triggered) return;
        var enemiesTemp = enemies.ToArray();
        foreach (var item in enemiesTemp)
        {
            if (item == null)
            {
                enemies.Remove(item);
            }
        }
        if (enemies.Count == 0)
        {
            triggered = true;
            OnClearEvent.Invoke();
        }
    }

}
