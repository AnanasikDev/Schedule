using UnityEngine;

public class SystemController : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void SetTimeSpeed(int speed) => Time.timeScale = speed;

    private void Start()
    {
        Application.runInBackground = true;
        Screen.fullScreen = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
