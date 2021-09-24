using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Movement Configuration")]

    [Tooltip("Target to follow")]
    [SerializeField]
    private Transform target;

    [Tooltip("Time of the camera repositining by SmoothDamp")]
    [SerializeField]
    private float smoothTime;
    private Vector2 velocity = Vector2.zero;

    [Tooltip("Distance for the camera to look ahead in the direction of the camera")]
    [SerializeField]
    private float lookAheadForce;

    [Tooltip("The max x and y position")]
    [SerializeField]
    private Vector2 maxCameraPosition;
    [Tooltip("The min x and y position")]
    [SerializeField]
    private Vector2 minCameraPosition;


    private void Start()
    {
        Vector3 newPos = target.position;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
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

        Vector3 newPosition = Vector2.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
        // Mantain the camera in the right z position
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraPosition.x, maxCameraPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraPosition.y, maxCameraPosition.y);
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }
}
