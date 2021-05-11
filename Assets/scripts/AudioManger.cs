using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger instance;
    public AudioSource bgm;
    public AudioSource[] soundEffects;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlaySFX(int sfxNumber)
    {
        soundEffects[sfxNumber].Stop();
        soundEffects[sfxNumber].Play();
    }

    public void StopSFX(int sfxNumber)
    {
        soundEffects[sfxNumber].Stop();
    }
}
