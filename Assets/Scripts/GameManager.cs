using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float lastWidth;
    private float lastHeight;

    void Start()
    {
        Screen.SetResolution(1080, 1920, FullScreenMode.FullScreenWindow);
    }

    void Update()
    {
        if (lastWidth != Screen.width)
        {
            Screen.SetResolution(Screen.width, (int)(Screen.width * (16f / 9f)), FullScreenMode.FullScreenWindow);
        }
        else if (lastHeight != Screen.height)
        {
            Screen.SetResolution((int)(Screen.height * (9f / 16f)), Screen.height, FullScreenMode.FullScreenWindow);
        }

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}