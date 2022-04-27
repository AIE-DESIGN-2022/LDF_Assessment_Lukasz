using System;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public abstract class Objective : MonoBehaviour
    {
        [Tooltip("Name of the objective that will be shown on screen")]
        public string Title;

        [Tooltip("Short text explaining the objective that will be shown on screen")]
        public string Description;

        [Tooltip("Whether the objective is required to win or not")]
        public bool IsOptional;

        [Tooltip("Delay before the message becomes visible")]
        public float DelayBeforeDisplay;

        [Tooltip("Delay before the objective becomes visible")]
        public float DelayVisible;

        public bool IsCompleted { get; private set; }
        public bool IsBlocking() => !(IsOptional || IsCompleted);

        public static event Action<Objective> OnObjectiveCreated;
        public static event Action<Objective> OnObjectiveCompleted;

        [SerializeField] bool startActivated = true;
        [SerializeField] Objective[] nextObjectives;
        [SerializeField] UnityEvent OnObjectiveComplete;

        public Objective motherObjective;
        public int numberOfChildObjectives;

        bool isActivated = false;

        protected virtual void Start()
        {
            if (startActivated) ActivateObjective();
            DelayVisible += DelayBeforeDisplay;
        }

        public virtual void ActivateObjective()
        {
            if (isActivated) return;
            isActivated = true;

            OnObjectiveCreated?.Invoke(this);
            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = Title;
            displayMessage.DelayBeforeDisplay = this.DelayBeforeDisplay;
            EventManager.Broadcast(displayMessage);
        }

        public void UpdateObjective(string descriptionText, string counterText, string notificationText)
        {
            ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
            evt.Objective = this;
            evt.DescriptionText = descriptionText;
            evt.CounterText = counterText;
            evt.NotificationText = notificationText;
            evt.IsComplete = IsCompleted;
            EventManager.Broadcast(evt);
        }

        public void CompleteObjective(string descriptionText, string counterText, string notificationText)
        {
            IsCompleted = true;

            ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
            evt.Objective = this;
            evt.DescriptionText = descriptionText;
            evt.CounterText = counterText;
            evt.NotificationText = notificationText;
            evt.IsComplete = IsCompleted;
            EventManager.Broadcast(evt);

            if (motherObjective) motherObjective.numberOfChildObjectives--;
            ActivateNextObjectives();
            OnObjectiveComplete.Invoke();
            OnObjectiveCompleted?.Invoke(this);
        }

        public bool IsActivated()
        {
            return isActivated;
        }

        private void ActivateNextObjectives()
        {
            if (nextObjectives == null) return;

            foreach (var obj in nextObjectives)
            {
                obj.ActivateObjective();
            }
        }
    }
}