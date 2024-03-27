using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_set : MonoBehaviour
{
    public FloatValue targetFPS;
    void Update()
    {
        Application.targetFrameRate = (int)targetFPS.value;
    }
}
