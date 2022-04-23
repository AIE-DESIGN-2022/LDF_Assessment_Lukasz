using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] bool isActive = false;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public float doorMoveDistance = 1.15f;
    public float doorSpeed = 1f;

    bool IsOpen = false;
    bool IsMoving = false;

    Vector3 leftDoorTarget;
    Vector3 rightDoorTarget;

    bool doorOccupied = false;
    float timeSinceDoorOccupied = Mathf.Infinity;
    public float timeToStayOpen = 2.0f;

    [SerializeField] UnityEvent OnStartMoving;
    [SerializeField] UnityEvent OnDoorOpened;
    [SerializeField] UnityEvent OnDoorClosed;

    void Update()
    {
        CalculateTimeSinceDoorOccupied();
        CloseDoorIfUnoccupied();
    }

    private void FixedUpdate()
    {
        DoorMovement();
    }

    private void CloseDoorIfUnoccupied()
    {
        if (IsOpen && timeSinceDoorOccupied > timeToStayOpen)
        {
            ToggleDoor();
        }
    }

    private void CalculateTimeSinceDoorOccupied()
    {
        if (IsOpen && !IsMoving)
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
        if (IsMoving)
        {
            

            float step = doorSpeed * Time.deltaTime;
            leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, leftDoorTarget, step);
            rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, rightDoorTarget, step);


            if (Vector3.Distance(leftDoor.transform.localPosition, leftDoorTarget) < 0.02)
            {
                leftDoor.transform.localPosition = leftDoorTarget;
                rightDoor.transform.localPosition = rightDoorTarget;
                IsMoving = false;

                if (IsOpen)
                {
                    IsOpen = false;
                    OnDoorClosed.Invoke();
                }
                else
                {
                    IsOpen = true;
                    OnDoorOpened.Invoke();
                }
            }
        }
    }

    public void ToggleDoor()
    {
        if (IsMoving) return;

        IsMoving = true;
        timeSinceDoorOccupied = 0;

        if (!IsOpen)
        {
            leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x - doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
            rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x + doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);

        }
        else if (IsOpen)
        {
            leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x + doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
            rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x - doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);
        }

        OnStartMoving.Invoke();
    }

    public bool IsActive()
    {
        return isActive;
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
