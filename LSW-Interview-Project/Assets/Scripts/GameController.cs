using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        // Locks at 60 fps, to avoid bugs with different performances, i used only for this project beside mobile ones
        Application.targetFrameRate = 60;
    }

}
