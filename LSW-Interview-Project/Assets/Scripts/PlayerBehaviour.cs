using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MoveableObjects
{
    void Update()
    {
        Move(GetInputAxis());
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
}
