using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOIntroduction : ScriptableObject
{
    [Header("Home Transform")]
    public Transform HomeTransform;
    [Header("Effects")]
    public GameObject DustParticle;
    [Header("Audio Introduction")]
    public AudioClip IntroductionAudio;
    [Header("Arrow Local")]
    public Transform[] arrowLocals;
    [Header("Time to Start")]
    public float TimeToStartGame;
}
