﻿using Unity.FPS.Game;
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

            objective = GetComponent<Objective>();
        }

        private void Update()
        {
            if (!isRegistered && objective != null)
            {
                if (objective.IsActivated())
                {
                    RegisterObjective();
                    isRegistered = true;
                }
            }
        }

        public void RegisterObjective()
        {
            var markerInstance = Instantiate(CompassMarkerPrefab);

            markerInstance.Initialize(this, TextDirection);
            m_Compass.RegisterCompassElement(transform, markerInstance);
        }

        void OnDestroy()
        {
            m_Compass.UnregisterCompassElement(transform);
        }
    }
}