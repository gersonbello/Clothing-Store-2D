using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a variable type to store direction of the movement
/// </summary>
public enum Direction
{
    up,
    left,
    down,
    right,
    empty
}

public class MoveableObjects : MonoBehaviour
{
    // This script was made for other moving objects to inherit

    [Header("Moviment Configuration")]
    [Tooltip("Speed of the moviment")]
    [SerializeField]
    protected float speed;
    // Used to get last direction from object
    public Direction lastMovementDirection { get; protected set; } = Direction.down;

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
    [SerializeField]
    protected bool animate;

    [Space(5)]
    [Header("Equipped Skins")]
    [SerializeField]
    protected Skin hatSkin;
    [SerializeField]
    protected Skin headSkin;
    [SerializeField]
    protected Skin bodySkin;
    [SerializeField]
    protected Skin handsSkin;
    [SerializeField]
    protected Skin feetSkin;

    [Space(5)]
    [Header("Bodyparts References")]
    [SerializeField]
    protected SpriteRenderer hatSkinRenderer;
    [SerializeField]
    protected SpriteRenderer headSkinRenderer;
    [SerializeField]
    protected SpriteRenderer bodySkinRenderer;
    [SerializeField]
    protected SpriteRenderer hand1SkinRenderer;
    [SerializeField]
    protected SpriteRenderer hand2SkinRenderer;
    [SerializeField]
    protected SpriteRenderer foot1SkinRenderer;
    [SerializeField]
    protected SpriteRenderer foot2SkinRenderer;

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
        rigidBody.MovePosition(objectPosition + direction * speed * Time.fixedDeltaTime);
    }
    /// <summary>
    ///  Used for execute the moviment to the desired position
    /// </summary>
    /// <param name="newPosition">Desired Position</param>
    /// <param name="newSpeed">Desired Velocity</param>
    protected void Move(Vector2 newPosition, int? newSpeed)
    {
        GetObjectDirection((Vector2)transform.position - newPosition);
        rigidBody.MovePosition(newPosition * (newSpeed.HasValue ? newSpeed.Value : speed) * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Gets the last direction the object moved, can't be empty
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
    /// <summary>
    /// Gets the new direction to the refered direction variable
    /// </summary>
    /// <param name="direction">Moviment direction</param>
    /// <param name="newDirection">Direction variable</param>
    protected void GetObjectDirection(Vector2 direction, ref Direction newDirection)
    {
        if (direction.magnitude != 0)
        {
            if (direction.x != 0)
            {
                newDirection = direction.x == 1 ? Direction.right : Direction.left;
            }
            else
            {
                newDirection = direction.y == 1 ? Direction.up : Direction.down;
            }
        } else newDirection = Direction.empty;
    }
    #endregion

    #region Animation and Skin
    /// <summary>
    /// Handle the Animator parameters and skin sprites
    /// </summary>
    /// <param name="direction"></param>
    protected void HandleAnimation(Vector2 direction)
    {
        Direction newDirection = new Direction();
        GetObjectDirection(direction, ref newDirection);
        switch (newDirection)
        {
            case Direction.up:
                ChangeSkin(newDirection);
                animator.SetFloat("InputX", 0);
                animator.SetFloat("InputY", 1);
                break;
            case Direction.down:
                ChangeSkin(newDirection);
                animator.SetFloat("InputX", 0);
                animator.SetFloat("InputY", -1);
                break;
            case Direction.left:
                ChangeSkin(newDirection);
                animator.SetFloat("InputX", -1);
                animator.SetFloat("InputY", 0);
                break;
            case Direction.right:
                ChangeSkin(newDirection);
                animator.SetFloat("InputX", 1);
                animator.SetFloat("InputY", 0);
                break;
            case Direction.empty:
                animator.SetFloat("InputX", 0);
                animator.SetFloat("InputY", 0);
                break;
        }
        animator.SetFloat("Speed", direction.sqrMagnitude * speed * Time.deltaTime);
    }

    /// <summary>
    /// Change the skin based on equiped skin and direction
    /// </summary>
    /// <param name="skinRenderer">SpriteRenderer to be changed</param>
    /// <param name="newSkin">New sprite</param>
    public void ChangeSkin(Direction newDirection)
    {
        switch (newDirection)
        {
            case Direction.up:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Up);
                SetSkin(headSkinRenderer, headSkin.Up);
                SetSkin(bodySkinRenderer, bodySkin.Up);
                SetSkin(hand1SkinRenderer, handsSkin.Up);
                SetSkin(hand2SkinRenderer, handsSkin.Up);
                SetSkin(foot1SkinRenderer, feetSkin.Up);
                SetSkin(foot2SkinRenderer, feetSkin.Up);
                break;
            case Direction.down:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Down);
                SetSkin(headSkinRenderer, headSkin.Down);
                SetSkin(bodySkinRenderer, bodySkin.Down);
                SetSkin(hand1SkinRenderer, handsSkin.Down);
                SetSkin(foot1SkinRenderer, feetSkin.Down);
                SetSkin(hand2SkinRenderer, handsSkin.Down);
                SetSkin(foot2SkinRenderer, feetSkin.Down);
                break;
            case Direction.left:
                Vector3 newScale = startScale;
                newScale.x *= -1;
                transform.localScale = newScale;
                SetSkin(hatSkinRenderer, hatSkin.Left);
                SetSkin(headSkinRenderer, headSkin.Left);
                SetSkin(bodySkinRenderer, bodySkin.Left);
                SetSkin(hand1SkinRenderer, handsSkin.Left);
                SetSkin(foot1SkinRenderer, feetSkin.Left);
                SetSkin(hand2SkinRenderer, handsSkin.Left);
                SetSkin(foot2SkinRenderer, feetSkin.Left);
                break;
            case Direction.right:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Right);
                SetSkin(headSkinRenderer, headSkin.Right);
                SetSkin(bodySkinRenderer, bodySkin.Right);
                SetSkin(hand1SkinRenderer, handsSkin.Right);
                SetSkin(foot1SkinRenderer, feetSkin.Right);
                SetSkin(hand2SkinRenderer, handsSkin.Right);
                SetSkin(foot2SkinRenderer, feetSkin.Right);
                break;
        }
    }
    /// <summary>
    /// Change the skin based on equiped skin and direction
    /// </summary>
    /// <param name="skinRenderer">SpriteRenderer to be changed</param>
    /// <param name="newSkin">New sprite</param>
    protected void ChangeSkin()
    {
        switch (lastMovementDirection)
        {
            case Direction.up:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Up);
                SetSkin(headSkinRenderer, headSkin.Up);
                SetSkin(bodySkinRenderer, bodySkin.Up);
                SetSkin(hand1SkinRenderer, handsSkin.Up);
                SetSkin(hand2SkinRenderer, handsSkin.Up);
                SetSkin(foot1SkinRenderer, feetSkin.Up);
                SetSkin(foot2SkinRenderer, feetSkin.Up);
                break;
            case Direction.down:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Down);
                SetSkin(headSkinRenderer, headSkin.Down);
                SetSkin(bodySkinRenderer, bodySkin.Down);
                SetSkin(hand1SkinRenderer, handsSkin.Down);
                SetSkin(foot1SkinRenderer, feetSkin.Down);
                SetSkin(hand2SkinRenderer, handsSkin.Down);
                SetSkin(foot2SkinRenderer, feetSkin.Down);
                break;
            case Direction.left:
                Vector3 newScale = startScale;
                newScale.x *= -1;
                transform.localScale = newScale;
                SetSkin(hatSkinRenderer, hatSkin.Left);
                SetSkin(headSkinRenderer, headSkin.Left);
                SetSkin(bodySkinRenderer, bodySkin.Left);
                SetSkin(hand1SkinRenderer, handsSkin.Left);
                SetSkin(foot1SkinRenderer, feetSkin.Left);
                SetSkin(hand2SkinRenderer, handsSkin.Left);
                SetSkin(foot2SkinRenderer, feetSkin.Left);
                break;
            case Direction.right:
                transform.localScale = startScale;
                SetSkin(hatSkinRenderer, hatSkin.Right);
                SetSkin(headSkinRenderer, headSkin.Right);
                SetSkin(bodySkinRenderer, bodySkin.Right);
                SetSkin(hand1SkinRenderer, handsSkin.Right);
                SetSkin(foot1SkinRenderer, feetSkin.Right);
                SetSkin(hand2SkinRenderer, handsSkin.Right);
                SetSkin(foot2SkinRenderer, feetSkin.Right);
                break;
        }
    }

    /// <summary>
    /// Changing the Sprite in this metod for easily update it later
    /// </summary>
    /// <param name="skinRenderer">SpriteRenderer to be changed</param>
    /// <param name="newSkin">New sprite</param>
    protected void SetSkin(SpriteRenderer skinRenderer, Sprite newSkin)
    {
        skinRenderer.sprite = newSkin;
    }

    /// <summary>
    /// Changing the Sprite in this metod for easily update it later
    /// </summary>
    /// <param name="skinRenderer">SpriteRenderer to be changed</param>
    /// <param name="newSkin">New sprite</param>
    public void SetSkin(Skin newSkin, StoreSection section)
    {
        switch (section)
        {
            case StoreSection.Hats: hatSkin = newSkin; break;
        }
        ChangeSkin();
    }
    #endregion
}
