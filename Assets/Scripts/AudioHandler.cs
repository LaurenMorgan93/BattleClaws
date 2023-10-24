using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource audioPlayer; //reference to the Audio handler Prefab
    public List<string> clipNames = new List<string>(); // list of clip names by string
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // where we'll store the clips we want

    private void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
    

        // Load all audio clips(Add these in the Unity Inspector) and store them in a dictionary for quick access.
        foreach (string clipName in clipNames) 
        {
            AudioClip audioClip = LoadAudioClip(clipName);
            if (audioClip != null)
            {
                //The key is the clip name (string) , and the value is the loaded AudioClip itself.
                audioClips.Add(clipName, audioClip);
            }
            else
            {
                // let us know if the clip wasn't found in the folder
                Debug.LogError("Audio clip not found: " + clipName);
            }
        }
    }

    public void PlaySoundEffect(string clipName)
    {
        //when the PlaySoundEffect fuction is called, check if the string (the name of the clip) matches one from the dictionary
        if (audioClips.ContainsKey(clipName))
        {
            audioPlayer.clip = audioClips[clipName];

            // if it does, play it once
            audioPlayer.Play();
        }
        else
        {
            //if not, Debug.Log
            Debug.LogError("Audio clip not found: " + clipName);
        }
    }

    public AudioClip LoadAudioClip(string clipName)
    {
        //IMPORTANT! THE CLIPS HAVE TO BE PLACED IN THE "RESOURCES" FOLDER IN THE ASSETS FOLDER 
        return Resources.Load<AudioClip>(clipName);
    }
}
