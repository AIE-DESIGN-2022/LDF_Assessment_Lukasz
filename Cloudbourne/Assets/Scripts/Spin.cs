using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
	[Tooltip("The direction and speed in units per second the object spins.")]
    public Vector3 SpinMagnitude;
    public bool hasRigidbody;
    private Rigidbody _rb;

    private void Start()
    {
        if (hasRigidbody)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if (hasRigidbody)
        {
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(SpinMagnitude * Time.deltaTime));
        }
        else
        {
            transform.Rotate(SpinMagnitude * Time.deltaTime);
        }
        
    }
}
