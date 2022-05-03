using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class CompassElement : MonoBehaviour
    {
        [Tooltip("The marker on the compass for this element")]
        public CompassMarker CompassMarkerPrefab;

        [Tooltip("Text override for the marker, if it's a direction")]
        public string TextDirection;

        Compass m_Compass;
        Objective objective;
        bool objectiveRegistered = false;

        void Awake()
        {
            //Objective.OnObjectiveActivated += ObjectiveRegistration;
            m_Compass = FindObjectOfType<Compass>();
            DebugUtility.HandleErrorIfNullFindObject<Compass, CompassElement>(m_Compass, this);

            objective = GetComponent<Objective>();
            if (objective != null) return;

            RegistrationLogic();
        }

        private void Update()
        {
            if (objective != null)
            {
                if (objective.IsActivated() && !objectiveRegistered)
                {
                    RegistrationLogic();
                    objectiveRegistered = true;
                }
            }

        }

        private void RegistrationLogic()
        {
            var markerInstance = Instantiate(CompassMarkerPrefab);

            markerInstance.Initialize(this, TextDirection);
            m_Compass.RegisterCompassElement(transform, markerInstance);
        }

        private void ObjectiveRegistration(Objective an_objective)
        {
            RegistrationLogic();
        }

        void OnDestroy()
        {
            m_Compass.UnregisterCompassElement(transform);
            //Objective.OnObjectiveActivated -= ObjectiveRegistration;
        }
    }
}