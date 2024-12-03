using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Camera camera;
    [SerializeField] private RectTransform notified;
    [SerializeField] private GameObject mask;
    private bool isNotified;
    private bool isPaused = false;
    private Volume blur;
    
    // Start is called before the first frame update
    void Start()
    {
        blur = camera.GetComponent<Volume>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MoveMaskToFarthestAlly();
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
            BGMPlayer.Instance.PauseMusic();
            blur.enabled = true;
        }
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Win()
    {
        resultPanel.SetActive(true);
        resultText.text = "You Win";
        PauseGame();
    }

    public void Lose()
    {
        resultPanel.SetActive(true);
        resultText.text = "You Lose";
        PauseGame();
    }

    public void UnPauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;   // Resume the game time
            isPaused = false;     // Set paused state to false
            BGMPlayer.Instance.ResumeMusic();
            blur.enabled = false;
        }
    }

    public void DoNotified(string text)
    {
        if (!isNotified)
        {
            notified.GetComponent<TMP_Text>().text = text;
            notified.DOAnchorPos(new Vector2(notified.anchoredPosition.x, 450f), 1f)  // Move down
                .SetEase(Ease.InOutQuad)
                .OnStart(() =>
                {
                    // Set the flag to true when the movement starts
                    if (!isNotified)
                    {
                        isNotified = true;
                    }
                })
                .OnComplete(() =>
                {
                    // Wait for 1 second, then move back up
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        notified.DOAnchorPos(new Vector2(notified.anchoredPosition.x, 600f), 1f)
                            .SetEase(Ease.InOutQuad)
                            .OnComplete(() =>
                            {
                                // Once the movement is complete, reset the flag
                                isNotified = false;
                            });
                    });
                });
        }
    }

    public void ShakeButton(RectTransform transform, Vector3 original)
    {
        //Original for coming in same exact pos
        transform.DOShakePosition(
                duration: 0.5f,
                strength: new Vector3(5f, 5f, 0f),
                vibrato: 8,
                randomness: 70)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Slowly move back to the original position
                transform.DOLocalMove(original, 0.5f) // 0.5f is the return duration
                    .SetEase(Ease.OutSine);
            });
    }

    public void DoSpawnButton(RectTransform transform, Vector3 originalScale, int index)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale , 0.5f)
            .SetEase(Ease.OutBack);
    }
    
    public void MoveMaskToFarthestAlly()
    {
        // Get all active CharacterEntity objects in the scene
        CharacterEntity[] allCharacters = FindObjectsOfType<CharacterEntity>();

        // Ensure there are allies to evaluate
        if (allCharacters.Length == 0)
            return;

        CharacterEntity farthestAlly = null;
        float maxDistance = float.MinValue;

        // Loop through all characters to find the farthest ally
        foreach (var character in allCharacters)
        {
            if (character.characterSide == CharacterEntity.CharacterSide.Ally)
            {
                float distance = character.transform.position.x;

                // Update the farthest ally if this one is farther
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestAlly = character;
                }
            }
        }

        // Move the mask if a farthest ally was found
        if (farthestAlly != null)
        {
            mask.transform.position = new Vector3(farthestAlly.transform.position.x - 35, mask.transform.position.y, mask.transform.position.z);
            Debug.Log($"Mask moved to the farthest ally at position: {farthestAlly.transform.position}");
        }
        else
        {
            Debug.LogWarning("No allies found to move the mask.");
        }
    }
}
