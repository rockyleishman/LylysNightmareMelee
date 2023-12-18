using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/SoundData",fileName = "SoundData",order = 3)]
public class SoundData : ScriptableObject
{
    public AudioClip mainmenuSound;
    public AudioClip backgroundSound;
    public AudioClip pauseSound;
    public AudioClip hitSound;
    public AudioClip upgradeSound;
}
