using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MoveableObjects
{
    #region Variables
    [Header("Player Status Configuration")]
    [Tooltip("Set if the player will respond to the input")]
    [SerializeField]
    private bool getInput = true;
    [Tooltip("Set if it is a copy from player")]
    [SerializeField]
    private bool copy;
    #endregion

    void Update()
    {
        HandlePlayerFunctions();
    }

    #region Player
    /// <summary>
    /// Handle player based in status like be able to use inputs
    /// </summary>
    private void HandlePlayerFunctions()
    {
        if (copy)
        {
            PlayerBehaviour pb = GameController.gcInstance.playerBehaviour;
            SetSkin(pb.hatSkin, StoreSection.Hats, lastMovementDirection);
            SetSkin(pb.bodySkin, StoreSection.Bodys, lastMovementDirection);
            SetSkin(pb.handsSkin, StoreSection.Hands, lastMovementDirection);
            SetSkin(pb.feetSkin, StoreSection.Feets, lastMovementDirection);
            return;
        }

        if (!getInput) return;

        Vector2 inputDirection = GetInputAxis();
        if (inputDirection.magnitude != 0)
        {
            targetWalkNode = null;
            Move(inputDirection);
        }
        if (targetWalkNode != null) AutoMove();
        else HandleAnimation(inputDirection);
    }
    #endregion

    #region Input
    /// <summary>
    /// Return the input values based on axis
    /// </summary>
    /// <returns></returns>
    private Vector2 GetInputAxis()
    {
        Vector2 inputAxis;
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");
        return inputAxis;
    }

    /// <summary>
    /// Turn off the player input
    /// </summary>
    public void TurnOffInput()
    {
        getInput = false;
    }
    /// <summary>
    /// Turn on the player input
    /// </summary>
    public void TurnOnInput()
    {
        getInput = true;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D coll)
    {
    }
}
