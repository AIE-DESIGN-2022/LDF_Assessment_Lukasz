using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveTriggerBased : Objective
    {
        [Tooltip("Visible transform that will be destroyed once the objective is completed")]
        public Transform DestroyRoot;

        void Awake()
        {
            if (DestroyRoot == null)
                DestroyRoot = transform;
        }

        public void TriggerObjective(Switch sw)
        {
            if (IsCompleted) return;
            if (sw != null) sw.SetStateActivated();
            CompleteObjective(string.Empty, string.Empty, "Objective complete : " + Title);
            Destroy(DestroyRoot.gameObject);

        }
    }
}