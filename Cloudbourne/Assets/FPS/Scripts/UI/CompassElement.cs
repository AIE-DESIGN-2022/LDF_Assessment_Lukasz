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
        bool isRegistered = false;

        void Awake()
        {
            m_Compass = FindObjectOfType<Compass>();
            DebugUtility.HandleErrorIfNullFindObject<Compass, CompassElement>(m_Compass, this);

            objective = GetComponentInParent<Objective>();
        }

        private void Update()
        {
            if (objective != null)
            {
                if (objective.isActivated)
                {
                    if (!isRegistered) Register();
                }
                else
                {
                    if (isRegistered) Deregister();
                }
            }
            else
            {
                if (!isRegistered)
                {
                    Register();
                }
            }
        }

        public void Register()
        {
            var markerInstance = Instantiate(CompassMarkerPrefab);
            isRegistered = true;
            markerInstance.Initialize(this, TextDirection);
            m_Compass.RegisterCompassElement(transform, markerInstance);
        }

        public void Deregister()
        {
            isRegistered = false;
            m_Compass.UnregisterCompassElement(transform);
        }

        void OnDestroy()
        {
            m_Compass.UnregisterCompassElement(transform);
        }
    }
}