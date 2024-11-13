using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform howToPlayButton;
    [SerializeField] private RectTransform quitButton;

    [SerializeField] private float target;
    [SerializeField] private float tweenDuration;
    // Start is called before the first frame update
    private void Awake()
    {
        playButton.GetComponent<CanvasGroup>().alpha = 0;
        howToPlayButton.GetComponent<CanvasGroup>().alpha = 0;
        quitButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Start()
    {
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowStartMenu()
    {
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
}
