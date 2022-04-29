using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

public class PlayerSwitchInteraction : MonoBehaviour
{
    bool ActiveRaytracing = false;
    [SerializeField] KeyCode activatingKey = KeyCode.F;
    string openMsg;


    private void Start()
    {
        openMsg = "Press " + activatingKey + " to open.";
    }

    private void Update()
    {
        if (ActiveRaytracing) DoRaytracing();
    }

    public void ActivePlayerRaytracing(bool Tracing)
    {
        ActiveRaytracing = Tracing;
    }

    private void DoRaytracing()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f) && hit.collider.GetComponentInParent<Switch>())
        {
            Switch theSwitch = hit.collider.GetComponentInParent<Switch>();

            if (Input.GetKeyDown(activatingKey))
            {
                theSwitch.SwitchPressed();
            }
        }
        else
        {

        }
    }



}
