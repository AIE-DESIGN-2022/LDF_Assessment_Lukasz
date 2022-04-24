using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour
{
    [SerializeField] float targetHeight;
    [SerializeField] float elevatorSpeed;

    bool isMoving = false;
    bool isAtOrigin = true;
    Vector3 elevatorTarget;

    [SerializeField] UnityEvent OnMoveStart;
    [SerializeField] UnityEvent OnReachTop;
    [SerializeField] UnityEvent OnReachBottom;

    Door door;
    Switch sw;

    private void Awake()
    {
        door = GetComponentInChildren<Door>();
        sw = GetComponentInChildren<Switch>();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        if (door != null)
        {
            if (!door.IsOpen())
            {
                ElevatorMovement();
            }
        }
        else
        {
            ElevatorMovement();
        }
        
        
    }

    private void ElevatorMovement()
    {
        float step = elevatorSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, elevatorTarget, step);

        if (Vector3.Distance(transform.position, elevatorTarget) < 0.02)
        {
            transform.position = elevatorTarget;
            isMoving = false;

            if (isAtOrigin)
            {
                isAtOrigin = false;
                OnReachTop.Invoke();
            }
            else
            {
                isAtOrigin = true;
                OnReachBottom.Invoke();
            }

            sw.SetStateActivated();
            if (door != null) door.OpenDoors();
        }

    }

    public void ElevatorToggle()
    {
        if (isMoving) return;

        isMoving = true;

        if (isAtOrigin)
        {
            elevatorTarget = new Vector3(transform.position.x, transform.position.y + targetHeight, transform.position.z);
        }
        else
        {
            elevatorTarget = new Vector3(transform.position.x, transform.position.y - targetHeight, transform.position.z);
        }

        if (door != null) door.CloseDoors();

        sw.SetStateActivating();
        OnMoveStart.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }

    public void OpenDoors()
    {
        if (door != null) door.OpenDoors();
        if (sw != null) sw.SetStateActivated();
    }
}
