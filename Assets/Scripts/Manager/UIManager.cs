using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Camera camera;
    private bool isPaused = false;
    private PostProcessVolume blur;
    // Start is called before the first frame update
    void Start()
    {
        blur = camera.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Pause()
    {
        if (!isPaused)
        {
            PauseGame();
            pausePanel.SetActive(true);
        }
        else if (isPaused)
        {
            UnPauseGame();
            pausePanel.SetActive(false); 
        }
    }
    
    
    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;   // Freeze the game time
            isPaused = true;      // Set paused state to tru
            blur.enabled = true;
        }
    }

    public void UnPauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;   // Resume the game time
            isPaused = false;     // Set paused state to false
            blur.enabled = false;
        }
    }
}
