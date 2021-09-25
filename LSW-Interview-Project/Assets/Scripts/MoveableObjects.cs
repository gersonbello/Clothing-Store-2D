using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // Automovement Configuration
    [HideInInspector]
    public List<Node> walkPath = new List<Node>();
    private Queue<Node> walkPathQueue = new Queue<Node>();
    // Reference to the next node to walk in and the last one walked
    protected Node targetWalkNode, lastWalkNode;

    // Events to execute after walking
    [SerializeField]
    protected UnityEvent targetWalkEvents;

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
        rigidBody.MovePosition(objectPosition + direction.normalized * speed * Time.fixedDeltaTime);
    }
    /// <summary>
    ///  Used for execute the moviment to the desired position
    /// </summary>
    /// <param name="newPosition">Desired Position</param>
    /// <param name="newSpeed">Desired Velocity</param>
    protected void Move(Vector2 newPosition, float? newSpeed)
    {
        Vector2 newDirection = (newPosition - (Vector2)transform.position).normalized;
        Vector2 objectPosition = transform.position;
        rigidBody.MovePosition(objectPosition + newDirection * (newSpeed.HasValue ? newSpeed.Value : speed) * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Set the path for automovement
    /// </summary>
    public void SetPath()
    {
        targetWalkEvents = null;
        walkPath.Clear();
        walkPathQueue.Clear();
        GameController.gcInstance.worldGrid.FindPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), ref walkPath);
        foreach (Node n in walkPath) walkPathQueue.Enqueue(n);
        if (walkPath == null || walkPath.Count == 0) return;
        targetWalkNode = walkPathQueue.Peek();
        lastWalkNode = new Node(new Vector2(), transform.position, true);
    }
    /// <summary>
    /// Set the path for automovement by mouse position with events at the end
    /// </summary>
    /// <param name="newEvents"></param>
    public void SetPath(UnityEvent newEvents)
    {
        targetWalkEvents = newEvents;
        walkPath.Clear();
        walkPathQueue.Clear();
        GameController.gcInstance.worldGrid.FindPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), ref walkPath);
        foreach (Node n in walkPath) walkPathQueue.Enqueue(n);
        if (walkPath == null || walkPath.Count == 0) return;
        targetWalkNode = walkPathQueue.Peek();
        lastWalkNode = new Node(new Vector2(), transform.position, true);
    }
    /// <summary>
    /// Set the path for automovement with events at the end
    /// </summary>
    /// <param name="newEvents"></param>
    public void SetPath(Vector2 targetPos,UnityEvent newEvents)
    {
        targetWalkEvents = newEvents;
        walkPath.Clear();
        walkPathQueue.Clear();
        GameController.gcInstance.worldGrid.FindPath(transform.position, targetPos, ref walkPath);
        foreach (Node n in walkPath) walkPathQueue.Enqueue(n);
        if (walkPath == null || walkPath.Count == 0) return;
        targetWalkNode = walkPathQueue.Peek();
        lastWalkNode = new Node(new Vector2(), transform.position, true);
    }
    /// <summary>
    /// Automatically moves character
    /// </summary>
    protected void AutoMove()
    {
        float distanceFromTarget = Vector2.Distance(transform.position, targetWalkNode.nodedWorldPosition);
        if (walkPathQueue.Count >= 0 && targetWalkNode != null)
        {
            if(walkPathQueue.Count == 1 && targetWalkEvents != null && distanceFromTarget <= .1f)
            {
                targetWalkEvents.Invoke();
                targetWalkNode = null;
                HandleAnimation(new Vector2());
                return;
            }
            if (distanceFromTarget > .1f)
            {
                GetObjectDirection((targetWalkNode.nodedWorldPosition - lastWalkNode.nodedWorldPosition));
                Move(targetWalkNode.nodedWorldPosition, speed);
                HandleAnimation(GetDirectionVector(lastMovementDirection));
            }
            else
            {
                lastWalkNode = targetWalkNode;
                if (walkPathQueue.Count > 0)
                    targetWalkNode = walkPathQueue.Dequeue();
                else
                {
                    targetWalkNode = null;
                    HandleAnimation(new Vector2());
                }
            }
        }
    }

    /// <summary>
    /// Gets the last direction the object moved, can't be empty
    /// </summary>
    /// <param name="direction"></param>
    protected void GetObjectDirection(Vector2 direction)
    {
        if (direction.magnitude != 0)
        {
            if (direction.x != 0 && direction.y != 0)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) direction.y = 0;
                else direction.x = 0;
            }
            if (direction.x != 0)
            {
                lastMovementDirection = direction.x > 0 ? Direction.right : Direction.left;
            }
            else
            {
                lastMovementDirection = direction.y > 0 ? Direction.up : Direction.down;
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
    /// <summary>
    /// Gets the vector2 from direction
    /// </summary>
    /// <param name="direction">Moviment direction</param>
    /// <param name="newDirection">Direction variable</param>
    protected Vector2 GetDirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.up: return Vector2.up; 
            case Direction.down: return Vector2.down; 
            case Direction.left: return Vector2.left; 
            case Direction.right: return Vector2.right;
            default: return new Vector2();
        }
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
    public void SetSkin(Skin newSkin, StoreSection section, Direction newDirection)
    {
        lastMovementDirection = newDirection;
        switch (section)
        {
            case StoreSection.Hats: hatSkin = newSkin; break;
        }
        ChangeSkin();
    }
    #endregion
}
