using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponentInParent<Unity.FPS.Game.Health>();
        if (health)
        {
            health.Kill();
        }
    }
}
