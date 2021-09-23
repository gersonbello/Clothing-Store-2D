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
    // Get the start scale to reference when changing direction
    protected Vector3 startScale;

    [Space(5)]
    [Header("Animation Configuration")]
    [Tooltip("Referenced Animator")]
    [SerializeField]
    protected Animator animator;

    private void Start()
    {
        startScale = transform.localScale;
    }

    #region Movement
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

    #endregion

    #region Animation
    protected void HandleAnimation(Vector2 direction)
    {
        switch (lastMovementDirection)
        {
            case Direction.up:
                transform.localScale = startScale;
                animator.SetFloat("InputX", 0);
                animator.SetFloat("Inputy", 1);
                break;
            case Direction.down:
                transform.localScale = startScale;
                animator.SetFloat("InputX", 0);
                animator.SetFloat("Inputy", -1);
                break;
            case Direction.left:
                Vector3 newScale = startScale;
                newScale.x *= -1;
                transform.localScale = newScale;
                animator.SetFloat("InputX", -1);
                animator.SetFloat("Inputy", 0);
                break;
            case Direction.right:
                transform.localScale = startScale;
                animator.SetFloat("InputX", 1);
                animator.SetFloat("Inputy", 0);
                break;
        }
        animator.SetFloat("Speed", direction.sqrMagnitude * speed * Time.deltaTime);
    }

    protected void SetAnimatorParameters(Vector2 direction)
    {

    }
    #endregion
}
