using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Introduction : MonoBehaviour
{
    public static Introduction instance;
    public SOIntroduction _stats;
    private AudioSource _audioSource;
    private GameObject _home;
    [SerializeField]
    private GameObject UIgame;
    [SerializeField]
    [Range(0, 5)]
    private float _timeToGrowUp;
    #region GetValues
    private Transform[] _arrowLocals;
    private AudioClip _introductionAudio;
    private GameObject _particles;
    private float _timeToStart;
    private Transform _homeTransform;
    #endregion
    void Start()
    {
        instance = this;
        _audioSource= GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        
        _home = GameObject.FindGameObjectWithTag("Home");
        _home.transform.position = new Vector3(_home.transform.position.x, _home.transform.position.y - 10, _home.transform.position.z);
        GetValues();
        PlayAudio();
    }
    private void GetValues()
    {
        _arrowLocals = _stats.arrowLocals;
        _introductionAudio = _stats.IntroductionAudio;
        _particles = _stats.DustParticle;
        _homeTransform = _stats.HomeTransform;
        _timeToStart = _stats.TimeToStartGame;
    }
    private void PlayAudio()
    {
        _audioSource.clip = _introductionAudio;
        _audioSource.Play();
        _timeToStart = _introductionAudio.length;
    }
    public void PressStartGame()
    {
        StartCoroutine(StartIntroduction());
    }
    private IEnumerator StartIntroduction()
    {
        GameObject Particles=Instantiate(_particles, _homeTransform.position, _homeTransform.rotation);
        yield return new WaitForSeconds(_timeToStart);
        HomeAnimation();
        UIgame.transform.DOScale(0, _timeToGrowUp);
        yield return new WaitForSeconds(_timeToGrowUp);
        ParticleSystem[] particles=Particles.GetComponentsInChildren<ParticleSystem>();
        for(int i=0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
        Destroy(Particles, 5f);
        Destroy(UIgame);
    }
    private void HomeAnimation()
    {
        _home.transform.DOLocalMoveY(_home.transform.position.y + 10, _timeToGrowUp);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
