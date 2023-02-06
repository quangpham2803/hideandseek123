using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;
    public static bool isSoundSetting = true;
    public static bool isMusicSetting = true;
    public AudioSource backGroundInGame;
    public AudioClip backGroundInGameClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public enum AUDIOLIST
    {
        btn_tap = 0,
        walk = 1,
        run = 2,
        three = 3,
        two = 4,
        one = 5,
        go = 6,
        win = 7,
        lose = 8,
        attack = 9
    };

    public void PlaySound(AUDIOLIST audioIndex)
    {
        if (!isSoundSetting)
        {
            return;
        }
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            { 
                audioSource.clip = audioClips[(int)audioIndex];
                audioSource.Play();
                break;
            }
        }
    }   
    public void StopSound(AUDIOLIST audioIndex)
    {
        if (!isSoundSetting)
        {
            return;
        }
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClips[(int)audioIndex])
            {
                audioSource.Stop();
                break;
            }
        }
    }
}
