using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
	[Tooltip("The gameobject the player should teleport too.")]
    public Transform teleportToLocation;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9 || collider.gameObject.tag == "Player")
        {
            CharacterController cc = collider.GetComponent<CharacterController>();

            cc.enabled = false;
            collider.transform.position = teleportToLocation.position;
            cc.enabled = true;
        }
    }
}
