using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public SoundData soundData;

    public AudioSource audioSource_1;
    public AudioSource audioSource_2;

    public void PlayMainMenu()
    {
        audioSource_1.clip = soundData.mainmenuSound;
        audioSource_1.Play();
    }

    public void PlayBG()
    {
        audioSource_1.clip = soundData.backgroundSound;
        audioSource_1.Play();
    }

    public void PlayHit()
    {
        audioSource_2.clip = soundData.hitSound;
        audioSource_2.PlayOneShot(audioSource_2.clip);
    }
    public void PlayPase()
    {
        audioSource_1.clip = soundData.pauseSound;
        audioSource_1.Play();
    }
    public void PlayUpgrade()
    {
        audioSource_2.clip = soundData.upgradeSound;
        audioSource_2.PlayOneShot(audioSource_2.clip);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}