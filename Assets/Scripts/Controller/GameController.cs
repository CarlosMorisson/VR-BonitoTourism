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
    [SerializeField]
    private float _cronometer=2;
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
    bool finished = false;
    bool started = false;
    private void Start()
    {
        instance = this;
        audioSource.clip = startAudio;
        audioSource.Play();
    }
    private void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if(started)
            _cronometer -= Time.deltaTime;
        UIController.instance.UpdateCronometer(_cronometer);
        if (_cronometer < 0f && !finished)
            EndGame();

    } 
    public void NewGame()
    {
        started = true;
    }
    private void EndGame()
    {

        _player.transform.DOMove(InitialPos.position, 2f);
        UIController.instance.SetGameOverCanva(_points);
        audioSource.clip = endAudio;
        audioSource.Play();
        finished=true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
    public void GetPoints()
    {
        Debug.Log(_points);
        _points++;
        UIController.instance.SetFishScore(_points);
    }
}
