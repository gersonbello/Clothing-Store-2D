using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToMousePosition : MonoBehaviour
{
    void Update()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
