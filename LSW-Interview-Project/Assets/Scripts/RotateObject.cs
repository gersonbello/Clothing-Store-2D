using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplily rotates a object with the created axis
/// </summary>
public class RotateObject : MonoBehaviour
{
    [Header("Rotation Configuration")]
    [Tooltip("Set the value of rotation by every axis")]
    [SerializeField]
    private Vector3 rotateDirectionAngles;
    void Update()
    {
        transform.Rotate(rotateDirectionAngles);
    }
}
