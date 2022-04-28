using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Tooltip("The position to move this object to. The object will move back to it's starting position, if you want.")]
    public Transform targetPosition;
	[Tooltip("How fast the object moves, in units per second.")]
    public float moveSpeed;
	[Tooltip("Should the object start moving on scene start?")]
    public bool moveOnStart;
	[Tooltip("Should the object move back and forth?")]
	public bool	loop;
    private Vector3 startPostion;
    private bool moving, moveToTarget = true;

    private void Start()
    {
        startPostion = transform.position;
        if (targetPosition.parent == transform)
            targetPosition.parent = null;
        if (moveOnStart)
            Move();
    }
    private void Update()
    {
        if (!moving) return;
        Vector3 targetPos = moveToTarget ? targetPosition.position : startPostion;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
        if (transform.position == targetPos)
        {
            if (loop)
            {
                moveToTarget = !moveToTarget;
            }
            else
            {
                moving = false;
            }
        }
            
    }

    public void Move()
    {
        if (moving) return;
        moveToTarget = transform.position == startPostion ? true : false;
        moving = true;
    }

}