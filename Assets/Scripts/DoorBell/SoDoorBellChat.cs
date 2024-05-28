using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class SoDoorBellChat :ScriptableObject
{
    [Header ("Audio List")]
    public List<AudioClip> AudioList;
    [Header ("Audio Animation")]
    public Image ImageSoundAnimation;
}
