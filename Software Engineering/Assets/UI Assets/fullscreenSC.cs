using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreenSC : MonoBehaviour
{
    public void changescreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
