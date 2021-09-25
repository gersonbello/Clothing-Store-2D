using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MoveableObjects
{
    [Header("Player Status Configuration")]
    [Tooltip("Set if the player will respond to the input")]
    [SerializeField]
    private bool getInput = true;


    void Update()
    {
        if (!getInput) return;
        Vector2 inputDirection = GetInputAxis();
        if (inputDirection.magnitude != 0)
        {
            targetWalkNode = null;
            Move(inputDirection);
        }
        if(targetWalkNode != null) AutoMove();
        else HandleAnimation(inputDirection);
    }
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

    private void OnTriggerEnter2D(Collider2D coll)
    {
    }
}
