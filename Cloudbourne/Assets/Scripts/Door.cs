using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] bool autoClose = true;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] float doorMoveDistance = 1.15f;
    [SerializeField] float doorSpeed = 1f;

    bool isOpen = false;
    bool isMoving = false;
    Vector3 leftDoorTarget;
    Vector3 rightDoorTarget;
    bool doorOccupied = false;
    float timeSinceDoorOccupied = Mathf.Infinity;
    public float timeToStayOpen = 2.0f;
    Switch[] switches;
    Elevator elevator;

    private void Start()
    {
        switches = GetComponentsInChildren<Switch>();
        elevator = GetComponentInParent<Elevator>();
    }

    void Update()
    {
        if (autoClose)
        {
            CalculateTimeSinceDoorOccupied();
            CloseDoorIfUnoccupied();
        }
    }

    private void FixedUpdate()
    {
        DoorMovement();
    }

    private void CloseDoorIfUnoccupied()
    {
        if (isOpen && timeSinceDoorOccupied > timeToStayOpen)
        {
            ToggleDoor();
        }
    }

    private void CalculateTimeSinceDoorOccupied()
    {
        if (isOpen && !isMoving)
        {
            if (doorOccupied)
            {
                timeSinceDoorOccupied = 0;
            }
            else
            {
                timeSinceDoorOccupied += Time.deltaTime;
            }
        }
    }

    private void DoorMovement()
    {
        if (isMoving)
        {
            float step = doorSpeed * Time.deltaTime;
            leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, leftDoorTarget, step);
            rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, rightDoorTarget, step);

            if (Vector3.Distance(leftDoor.transform.localPosition, leftDoorTarget) < 0.02)
            {
                leftDoor.transform.localPosition = leftDoorTarget;
                rightDoor.transform.localPosition = rightDoorTarget;
                isMoving = false;

                if (isOpen)
                {
                    isOpen = false;
                    if (elevator == null) SetSwitchesToStandby();
                }
                else
                {
                    isOpen = true;
                    if (elevator == null) SetSwitchesToActivated();
                }
            }
        }
    }

    public void ToggleDoor()
    {
        if (isMoving || !isActive) return;

        isMoving = true;
        if (!isOpen)
        {
            leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x - doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
            rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x + doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);
            timeSinceDoorOccupied = 0;

        }
        else if (isOpen)
        {
            leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x + doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
            rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x - doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);
        }

        if (elevator == null) SetSwitchesToActivating();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void OpenDoors()
    {
        if (!isOpen)
        {
            ToggleDoor();
        }
    }

    public void CloseDoors()
    {
        if (isOpen)
        {
            ToggleDoor();
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void ActivateDoor()
    {
        SetActive(true);
    }

    public void DeactivateDoor()
    {
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        isActive = active;
        
        if (isActive)
        {
            SetSwitchesToStandby();
        }
        else
        {
            SetSwitchesToInActive();
        }
    }

    private void SetSwitchesToInActive()
    {
        if (switches == null) return;

        foreach (Switch sw in switches)
        {
            sw.SetStateInactive();
        }
    }

    private void SetSwitchesToActivating()
    {
        if (switches == null) return;

        foreach (Switch sw in switches)
        {
            sw.SetStateActivating();
        }
    }

    private void SetSwitchesToActivated()
    {
        if (switches == null) return;

        foreach (Switch sw in switches)
        {
            sw.SetStateActivated();
        }
    }

    private void SetSwitchesToStandby()
    {
        if (switches == null) return;

        foreach (Switch sw in switches)
        {
            sw.SetStateStandby();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameObject.FindGameObjectWithTag("Player").transform)
        {
            doorOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameObject.FindGameObjectWithTag("Player").transform)
        {
            doorOccupied = false;
        }
    }
}
