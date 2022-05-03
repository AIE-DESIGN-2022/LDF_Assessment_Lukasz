using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class Door : MonoBehaviour
    {
        [SerializeField] bool isActive = false;
        [SerializeField] bool autoOpen = false;
        [SerializeField] bool autoClose = true;
        [SerializeField] bool linearInterpMovement = false;
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
        int enemyOccupied = 0;

        private void Start()
        {
            switches = GetComponentsInChildren<Switch>();
            elevator = GetComponentInParent<Elevator>();

            if (isActive) ActivateDoor();
            //else DeactivateDoor();
        }

        void Update()
        {
            if (autoClose)
            {
                CalculateTimeSinceDoorOccupied();
                CloseDoorIfUnoccupied();
            }
            if (autoOpen && doorOccupied)
            {
                OpenDoors();
            }

            if (enemyOccupied > 0)
            {
                OpenDoors();
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
                if (doorOccupied || enemyOccupied > 0)
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

                if (linearInterpMovement)
                {
                    if (leftDoor != null) leftDoor.transform.localPosition = Vector3.Lerp(leftDoor.transform.localPosition, leftDoorTarget, step);
                    if (rightDoor != null) rightDoor.transform.localPosition = Vector3.Lerp(rightDoor.transform.localPosition, rightDoorTarget, step);
                }
                else
                {
                    if (leftDoor != null) leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, leftDoorTarget, step);
                    if (rightDoor != null) rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, rightDoorTarget, step);
                }
                

                if (Vector3.Distance(leftDoor.transform.localPosition, leftDoorTarget) < 0.02)
                {
                    if (leftDoor != null) leftDoor.transform.localPosition = leftDoorTarget;
                    if (rightDoor != null) rightDoor.transform.localPosition = rightDoorTarget;
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
                if (leftDoor != null) leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x - doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
                if (rightDoor != null) rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x + doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);
                timeSinceDoorOccupied = 0;

            }
            else if (isOpen)
            {
                if (leftDoor != null) leftDoorTarget = new Vector3(leftDoor.transform.localPosition.x + doorMoveDistance, leftDoor.transform.localPosition.y, leftDoor.transform.localPosition.z);
                if (rightDoor != null) rightDoorTarget = new Vector3(rightDoor.transform.localPosition.x - doorMoveDistance, rightDoor.transform.localPosition.y, rightDoor.transform.localPosition.z);
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
                SetSwitchesToInactive();
            }
        }

        private void SetSwitchesToInactive()
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
            if (other.name != "Pickup_Health" && other.name != "Player") print("Entered trigger: " + other.name);

            if (other.transform == GameObject.FindGameObjectWithTag("Player").transform)
            {
                doorOccupied = true;
            }

            if (other.transform.GetComponent<EnemyDoorInteraction>() != null)
            {
                print("Enemy opening door");
                enemyOccupied++;

            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == GameObject.FindGameObjectWithTag("Player").transform)
            {
                doorOccupied = false;
            }

            if (other.transform.GetComponent<EnemyDoorInteraction>() != null)
            {
                enemyOccupied--;

            }
        }
    }
}