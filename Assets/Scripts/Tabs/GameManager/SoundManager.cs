using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public static bool muted = false;

    private void Awake()
    {
        //So that we only have 1 instance at once
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Where we store ALL the sound clips
    public SoundAudioClip[] soundAudioClipArray;
}


//Class for audio clip (correspond enum sound to audioclip)
[System.Serializable]
public class SoundAudioClip
{
    public SoundManage.Sound sound;
    public AudioClip audioClip;
}



//This is where all the logic will be also where we will play sounds
public static class SoundManage
{
    public static GameObject oneShotGameObject;
    public static AudioSource oneShotAudioSource;

    public enum Sound
    {
        DeleteUserClick,
        DeleteUserPop,
        InteractTicket,
        MenuClick,
        SelectTab,
        SelectTicket,

    }

    //Play the sound from anywhere
    public static void PlaySound(Sound sound)
    {
        if (!SoundManager.muted)
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }


    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        //Check if we find a match (We may use dictionaries if this becomes performance culprit)
        foreach (SoundAudioClip soundAudioClip in SoundManager.Instance.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("AUDIO CLIP NOT FOUND");
        return null;
    }
}

