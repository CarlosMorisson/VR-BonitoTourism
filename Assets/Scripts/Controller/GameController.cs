using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    private float _cronometer=3;
    private int _points;
    [SerializeField]
    GameObject GameOverCanva;
    private Transform _player;
    [SerializeField]
    Transform InitialPos;
    [Header("Audio")]
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip startAudio, endAudio;
    private void Start()
    {
        audioSource.clip = startAudio;
        audioSource.Play();
    }
    private void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _cronometer -= Time.deltaTime;
        UIController.instance.UpdateCronometer(_cronometer);
        if (_cronometer < 0f)
            EndGame();
    } 
    private void EndGame()
    {
        UIController.instance.SetGameOverCanva(_points);
        _player.transform.DOMove(InitialPos.position, 2f);
        audioSource.clip = endAudio;
        audioSource.Play();
    }

}
