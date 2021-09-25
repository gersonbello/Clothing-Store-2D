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
        Move(inputDirection);
        HandleAnimation(inputDirection);
    }

    private void FixedUpdate()
    {
        
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


    private void OnCollisionEnter2D(Collision2D coll)
    {
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Money Bag"))
        {
            FindObjectOfType<MoneyController>().AddMoney(50);
            Destroy(coll.gameObject);
        }
    }
}
