using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    public bool fullScreen = true;
    public float lastScreenWidth = 0f;

    void Start()
    {
        lastScreenWidth = Screen.width;
        AdjustResolution();
    }

    private void Update()
    {
        if (lastScreenWidth != Screen.width)
        {
            fullScreen = Screen.fullScreen;
            lastScreenWidth = Screen.width;
            AdjustResolution();
        }
    }

    private void AdjustResolution()
    {
        int targetWidth = Screen.height * 9 / 16;
        Screen.SetResolution(targetWidth, Screen.height, fullScreen);
    }


}