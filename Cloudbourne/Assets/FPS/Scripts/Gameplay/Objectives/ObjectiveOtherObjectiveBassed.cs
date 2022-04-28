using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveOtherObjectiveBassed : Objective
    {
        [Tooltip("Visible transform that will be destroyed once the objective is completed")]
        public Transform DestroyRoot;

        public Objective[] otherObjectives;
        public int numberOfActive;

        void Awake()
        {
            if (DestroyRoot == null)
                DestroyRoot = transform;

            otherObjectives = GetComponentsInChildren<Objective>();
            numberOfActive = otherObjectives.Length;
        }

        private void Update()
        {
            if (!CheckIfHasChildrenActive()) Completed();
        }

        private bool CheckIfHasChildrenActive()
        {
            int activeLeft = numberOfActive;

            foreach (Objective obj in otherObjectives)
            {
                if (obj == null) activeLeft--;
            }

            return activeLeft > 1;
        }

        public void Completed()
        {
            if (IsCompleted) return;
            CompleteObjective(string.Empty, string.Empty, "Objective complete : " + Title);
            Destroy(DestroyRoot.gameObject);
        }
    }
}