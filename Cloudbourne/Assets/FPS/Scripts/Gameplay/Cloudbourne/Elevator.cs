using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] bool isActive = false;
        [SerializeField] float targetHeight;
        [SerializeField] bool linearInterpMovement = false;
        [SerializeField] float elevatorSpeed;
        [SerializeField] bool startsAtBottom = true;
        [SerializeField] GameObject bottomElevatorObject;
        [SerializeField] GameObject topElevatorObject;

        bool isMoving = false;
        bool isAtBottom = true;
        Vector3 elevatorTarget;
        Door door;
        Switch sw;
        Door bottomDoor;
        Door topDoor;
        Switch bottomSwitch;
        Switch topSwitch;

        private void Awake()
        {
            isAtBottom = startsAtBottom;
            door = GetComponentInChildren<Door>();
            sw = GetComponentInChildren<Switch>();

            if (bottomElevatorObject != null)
            {
                bottomDoor = bottomElevatorObject.GetComponent<Door>();
                bottomSwitch = bottomElevatorObject.GetComponentInChildren<Switch>();
                if (bottomSwitch == null) bottomSwitch = bottomElevatorObject.GetComponent<Switch>();
            }

            if (topElevatorObject != null)
            {
                topDoor = topElevatorObject.GetComponent<Door>();
                topSwitch = topElevatorObject.GetComponentInChildren<Switch>();
                if (topSwitch == null) topSwitch = topElevatorObject.GetComponent<Switch>();
            }
        }

        private void Start()
        {
            if (bottomSwitch != null) bottomSwitch.SetElevatorCallSwitch(this, false);
            if (topSwitch != null) topSwitch.SetElevatorCallSwitch(this, true);

            if (isActive) ActivateElevator();
            //else DeactivateElevator();
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

        public void ElevatorToggle()
        {
            if (isMoving) return;
            isMoving = true;
            SetElevatorTarget();
            CloseDoors();
            SetSwitchesStateActivating();
        }

        private void SetElevatorTarget()
        {
            if (isAtBottom)
            {
                elevatorTarget = new Vector3(transform.position.x, transform.position.y + targetHeight, transform.position.z);
            }
            else
            {
                elevatorTarget = new Vector3(transform.position.x, transform.position.y - targetHeight, transform.position.z);
            }
        }

        private void SetSwitchesStateActivating()
        {
            if (sw != null) sw.SetStateActivating();
            if (topSwitch != null) topSwitch.SetStateActivating();
            if (bottomSwitch != null) bottomSwitch.SetStateActivating();
        }

        public void OpenDoors()
        {
            if (door != null) door.OpenDoors();
            if (sw != null) sw.SetStateActivated();
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.parent = transform;
        }

        private void OnTriggerExit(Collider other)
        {
            other.transform.parent = null;
        }

        private void ElevatorMovement()
        {
            float step = elevatorSpeed * Time.deltaTime;

            if (linearInterpMovement)
            {
                transform.position = Vector3.Lerp(transform.position, elevatorTarget, step);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, elevatorTarget, step);
            }
            

            if (Vector3.Distance(transform.position, elevatorTarget) < 0.02)
            {
                transform.position = elevatorTarget;
                isMoving = false;

                if (isAtBottom)
                {
                    isAtBottom = false;
                    OnArrivalTop();
                }
                else
                {
                    isAtBottom = true;
                    OnArrivalBottom();
                }
            }

        }

        public void CallElevator(bool callingSwitchIsBottom)
        {
            if (callingSwitchIsBottom)
            {
                if (isAtBottom) OnArrivalBottom();
                else ElevatorToggle();
            }
            else if (!callingSwitchIsBottom)
            {
                if (isAtBottom) ElevatorToggle();
                else OnArrivalTop();
            }
        }

        private void OnArrivalTop()
        {
            if (door != null) door.OpenDoors();
            if (sw != null) sw.SetStateActivated();
            if (topDoor != null) topDoor.OpenDoors();
            if (topSwitch != null) topSwitch.SetStateActivated();
            if (bottomSwitch != null) bottomSwitch.SetStateStandby();
        }

        private void OnArrivalBottom()
        {
            if (door != null) door.OpenDoors();
            if (sw != null) sw.SetStateActivated();
            if (bottomDoor != null) bottomDoor.OpenDoors();
            if (bottomSwitch != null) bottomSwitch.SetStateActivated();
            if (topSwitch != null) topSwitch.SetStateStandby();
        }

        private void CloseDoors()
        {
            if (door != null) door.CloseDoors();
            if (bottomDoor != null) bottomDoor.CloseDoors();
            if (topDoor != null) topDoor.CloseDoors();
        }

        public void ActivateElevator()
        {
            if (sw != null) sw.SetStateActivated();
            if (door != null) door.ActivateDoor();

            if (bottomDoor != null) bottomDoor.ActivateDoor();
            else if (bottomSwitch != null && isAtBottom) bottomSwitch.SetStateActivated();
            else if (bottomSwitch != null && !isAtBottom) bottomSwitch.SetStateStandby();

            if (topDoor != null) topDoor.ActivateDoor();
            else if (topSwitch != null && isAtBottom) topSwitch.SetStateStandby();
            else if (topSwitch != null && !isAtBottom) topSwitch.SetStateActivated();

        }

        public void DeactivateElevator()
        {
            if (sw != null) sw.SetStateInactive();
            if (door != null) door.DeactivateDoor();

            if (bottomDoor != null) bottomDoor.DeactivateDoor();
            else if (bottomSwitch != null) bottomSwitch.SetStateInactive();

            if (topDoor != null) topDoor.DeactivateDoor();
            else if (topSwitch != null) topSwitch.SetStateInactive();
        }
    }
}