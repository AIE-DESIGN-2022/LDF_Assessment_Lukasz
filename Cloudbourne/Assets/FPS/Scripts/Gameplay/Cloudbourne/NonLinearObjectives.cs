using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

public class NonLinearObjectives : MonoBehaviour
{
    [SerializeField] Objective[] objectives;
    [SerializeField] Objective[] autoComplete;
    [SerializeField] UnityEvent OnTrigger;

    public void ActivateAllObjectives()
    {
        foreach (Objective objective in objectives)
        {
            if (objective != null && !objective.IsActivated())
            {
                //print("Activating: " + objective.name);
                objective.ActivateObjective();
            }
        }
    }

    public void AutoCompletePreviousObjectives()
    {
        foreach (Objective objective in autoComplete)
        {
            if (objective != null)
            {
                objective.AutoComplete();
            }
        }
    }

    public void ActivateAndComplete()
    {
        AutoCompletePreviousObjectives();
        ActivateAllObjectives();
        OnTrigger?.Invoke();
        Destroy(gameObject);
    }
}
