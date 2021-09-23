using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Movement Configuration")]

    [Tooltip("Target to follow")]
    [SerializeField]
    private Transform target;

    [Tooltip("Speed of the camera")]
    [SerializeField]
    private float speed;

    [Tooltip("Distance for the camera to look ahead in the direction of the camera")]
    [SerializeField]
    private float lookAheadForce;

    void LateUpdate()
    {
        FollowTarget();
    }

    /// <summary>
    /// Follow the target and look in front of it
    /// </summary>
    private void FollowTarget()
    {
        // Get the position with the direction to look ahead
        Vector2 desiredPosition = new Vector2();
        MoveableObjects targetMoveableObjects = target.GetComponent<MoveableObjects>();
        if (targetMoveableObjects != null)
            switch (targetMoveableObjects.lastMovementDirection)
            {
                case Direction.up:
                    desiredPosition = (Vector2)target.position + Vector2.up * lookAheadForce;
                    break;
                case Direction.down:
                    desiredPosition = (Vector2)target.position + Vector2.down * lookAheadForce;
                    break;
                case Direction.left:
                    desiredPosition = (Vector2)target.position + Vector2.left * lookAheadForce;
                    break;
                case Direction.right:
                    desiredPosition = (Vector2)target.position + Vector2.right * lookAheadForce;
                    break;
            }         
        else
            desiredPosition = target.position;


        // Mantain the camera in the right z position
        Vector3 newPosition = Vector2.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }
}
