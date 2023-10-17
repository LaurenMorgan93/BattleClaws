using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure the audio doesn't play automatically on object creation.
    }

    public void PlaySoundEffect(string clipName)
    {
        AudioClip audioClip = LoadAudioClip(clipName);

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Audio clip not found: " + clipName);
        }
    }

    private AudioClip LoadAudioClip(string clipName)
    {
        string audioFilePath = "Audio/" + clipName;
        AudioClip audioClip = Resources.Load<AudioClip>(clipName);

        return audioClip;
    }
}
