using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform howToPlayButton;
    [SerializeField] private RectTransform quitButton;

    [SerializeField] private CanvasGroup canvasGroup_HowToPlay;

    [SerializeField] private float target;
    [SerializeField] private float tweenDuration;

    private bool isSceneTransitioning = false; // Tracks if the coroutine is already running

    private void OnEnable()
    {
        ShowStartMenu();
    }
    
    private void Awake()
    {
        playButton.GetComponent<CanvasGroup>().alpha = 0;
        howToPlayButton.GetComponent<CanvasGroup>().alpha = 0;
        quitButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Start()
    {
        
    }

    private void ShowStartMenu()
    {
        BGMPlayer.Instance.PlayMainMenuBGM();
        
        Sequence menuSequence = DOTween.Sequence();

        // Animate title first
        menuSequence.Append(title.DOAnchorPosY(title.anchoredPosition.y - target, tweenDuration));

        // Animate playButton after title, including both position and fade
        menuSequence.Append(playButton.DOAnchorPosY(playButton.anchoredPosition.y - target, tweenDuration));
        menuSequence.Join(playButton.GetComponent<CanvasGroup>().DOFade(1f, tweenDuration));

        // Animate howToPlayButton after playButton
        menuSequence.Append(howToPlayButton.DOAnchorPosY(howToPlayButton.anchoredPosition.y - target, tweenDuration));
        menuSequence.Join(howToPlayButton.GetComponent<CanvasGroup>().DOFade(1f, tweenDuration));

        // Animate quitButton after howToPlayButton
        menuSequence.Append(quitButton.DOAnchorPosY(quitButton.anchoredPosition.y - target, tweenDuration));
        menuSequence.Join(quitButton.GetComponent<CanvasGroup>().DOFade(1f, tweenDuration));
    }

    public void InitiateStartGameScene()
    {
        if (!isSceneTransitioning) // Prevent overlapping coroutines
        {
            StartCoroutine(StartGameSceneCoroutine());
        }
    }

    public void OpenHowToPlay()
    {
        if (!isSceneTransitioning) // Prevent overlapping coroutines
        {
            canvasGroup_HowToPlay.DOFade(1f, 0.5f);
            canvasGroup_HowToPlay.interactable = true;
            canvasGroup_HowToPlay.blocksRaycasts = true;
        }
    }

    public void CloseHowToPlay()
    {
        if (!isSceneTransitioning) // Prevent overlapping coroutines
        {
            canvasGroup_HowToPlay.DOFade(0f, 0.5f);
            canvasGroup_HowToPlay.interactable = false;
            canvasGroup_HowToPlay.blocksRaycasts = false;
        }
    }

    private IEnumerator StartGameSceneCoroutine()
    {
        isSceneTransitioning = true; // Mark the coroutine as running

        // Optionally, fade out the music or other effects before scene transition
        BGMPlayer.Instance.StopAllAudio(true);

        yield return new WaitForSeconds(1.5f); // Adjust based on your transition needs

        SceneManager.LoadScene(1);

        isSceneTransitioning = false; // Reset the flag after the transition
    }
}
