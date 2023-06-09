using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlarmTrigger : MonoBehaviour
{
    private AudioSource _audioSource;

    private float _maxVolume = 0.5f;
    private float _zeroVolume = 0f;
    private float _fadeTime = 10f;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _audioSource.Play();

            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float currentVolume = _audioSource.volume;

        while (elapsedTime < _fadeTime)
        {
            _audioSource.volume = Mathf.MoveTowards(currentVolume, _maxVolume, (elapsedTime) / _fadeTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _audioSource.volume = _maxVolume;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float currentVolume = _audioSource.volume;

        while (elapsedTime < _fadeTime)
        {
            _audioSource.volume = Mathf.MoveTowards(currentVolume, _zeroVolume, (elapsedTime) / _fadeTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _audioSource.volume = _zeroVolume;

        _audioSource.Stop();
    }
}
