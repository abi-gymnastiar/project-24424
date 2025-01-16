using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverPanel : MonoBehaviour
{
    // unity event
    public UnityEvent onRestartPressed;
    // key to restart
    public KeyCode restartKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            onRestartPressed.Invoke();
            // unpause the game
            Time.timeScale = 1;
            // reload the scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
