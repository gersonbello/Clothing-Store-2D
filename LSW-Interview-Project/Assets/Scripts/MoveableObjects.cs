using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a variable type to store direction of the movement
/// </summary>
public enum Direction
{
    up,
    down,
    left,
    right
}

public class MoveableObjects : MonoBehaviour
{
    // This script was made for other moving objects to inherit

    [Header("Moviment Configuration")]
    [Tooltip("Speed of the moviment")]
    [SerializeField]
    protected float speed;
    // Used to get last direction from object
    public Direction lastMovementDirection { get; protected set; }

    [Space(5)]
    [Header("References")]
    [Tooltip("Referenced RigidBody")]
    [SerializeField]
    protected Rigidbody2D rigidBody;

    /// <summary>
    /// Used for execute the moviment to the desired direction
    /// </summary>
    protected void Move(Vector2 direction)
    {
        GetObjectDirection(direction);
        Vector2 objectPosition = transform.position;
        rigidBody.MovePosition(objectPosition + direction * speed * Time.deltaTime);
    }
    /// <summary>
    ///  Used for execute the moviment to the desired position
    /// </summary>
    /// <param name="newPosition">Desired Position</param>
    /// <param name="newSpeed">Desired Velocity</param>
    protected void Move(Vector2 newPosition, int? newSpeed)
    {
        GetObjectDirection((Vector2)transform.position - newPosition);
        rigidBody.MovePosition(newPosition * (newSpeed.HasValue ? newSpeed.Value : speed) * Time.deltaTime);
    }

    /// <summary>
    /// Gets the last direction the object moved
    /// </summary>
    /// <param name="direction"></param>
    protected void GetObjectDirection(Vector2 direction)
    {
        if (direction.magnitude != 0)
        {
            if (direction.x != 0)
            {
                lastMovementDirection = direction.x == 1 ? Direction.right : Direction.left;
            }
            else
            {
                lastMovementDirection = direction.y == 1 ? Direction.up : Direction.down;
            }
        }
    }
}
