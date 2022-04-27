using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveOtherObjectiveBassed : Objective
    {
        [Tooltip("Visible transform that will be destroyed once the objective is completed")]
        public Transform DestroyRoot;

        [SerializeField] Objective[] otherObjectives;

        void Awake()
        {
            if (DestroyRoot == null)
                DestroyRoot = transform;
        }


        void Start()
        {
            foreach (Objective otherObjective in otherObjectives)
            {
                otherObjective.motherObjective = this;
                numberOfChildObjectives++;
            }
        }

        private void Update()
        {
            if (numberOfChildObjectives <= 0) Completed();
        }

        public void Completed()
        {
            if (IsCompleted) return;
            CompleteObjective(string.Empty, string.Empty, "Objective complete : " + Title);
            Destroy(DestroyRoot.gameObject);

        }
    }
}