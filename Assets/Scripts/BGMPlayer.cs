using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;

    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _inGameMusic;

    [SerializeField] private float fadeDuration = 1.0f; // Duration for fade-in/out

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the entire GameObject
        }
        else
        {
            Destroy(gameObject);
        }

        if (_musicAudioSource == null)
        {
            _musicAudioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayMainMenuBGM()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToNewClip(_mainMenuMusic));
    }

    public void PlayInGameBGM()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToNewClip(_inGameMusic));
    }

    public void StopAllAudio(bool fadeOut = true)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (fadeOut)
        {
            fadeCoroutine = StartCoroutine(FadeOutAndStop());
        }
        else
        {
            _musicAudioSource.Stop();
        }
    }

    /// <summary>
    /// Fades out the current music and fades in the new clip.
    /// </summary>
    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        if (_musicAudioSource.isPlaying)
        {
            yield return FadeOut(); // Fade out the current music
        }

        _musicAudioSource.clip = newClip;
        _musicAudioSource.Play();

        yield return FadeIn(); // Fade in the new music
    }

    /// <summary>
    /// Gradually fades out the audio source.
    /// </summary>
    private IEnumerator FadeOut()
    {
        float startVolume = _musicAudioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _musicAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        _musicAudioSource.volume = 0;
        _musicAudioSource.Stop();
    }

    /// <summary>
    /// Gradually fades out the audio source and stops it.
    /// </summary>
    private IEnumerator FadeOutAndStop()
    {
        float startVolume = _musicAudioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _musicAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        _musicAudioSource.volume = 0;
        _musicAudioSource.Stop();
    }

    /// <summary>
    /// Gradually fades in the audio source.
    /// </summary>
    private IEnumerator FadeIn()
    {
        _musicAudioSource.volume = 0;
        float targetVolume = 1.0f; // Assuming max volume is 1.0

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _musicAudioSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }

        _musicAudioSource.volume = targetVolume;
    }
}
