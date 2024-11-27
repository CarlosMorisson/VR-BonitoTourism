using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [Header("Hud")]
    [SerializeField]
    public GameObject HudPlayer;
    [SerializeField]
    private TextMeshProUGUI FishScore;
    [SerializeField]
    private TextMeshProUGUI CronometerText;
    [Header("GameOver")]
    [SerializeField]
    GameObject GameOver;
    [SerializeField]
    private TextMeshProUGUI FishScoreGameOver;
    [Header("PauseGame")]
    [SerializeField]
    GameObject Pause;
    int NumOfContacts;
    bool _paused;

    private void Start()
    {
        Time.timeScale = 0;
        instance = this;
    }
    public void UpdateCronometer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        CronometerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    public void SetGameOverCanva(int points)
    {
        HudPlayer.SetActive(false);
        GameOver.SetActive(true);
        GameOver.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 5);
;        FishScoreGameOver.text = points.ToString();
        Time.timeScale = 0;
    }
    public void GamePauseCanva()
    {
        Pause.SetActive(true);
        HudPlayer.SetActive(false);
        Time.timeScale = 0;
    }
    public void SetFishScore(int points)
    {
        FishScore.text = points.ToString();
    }
    public void ReturnFromPause()
    {
        NumOfContacts = 0;
        _paused = false;
        Time.timeScale = 1;
    }
    public void IncrementPokeToPause()
    {
        NumOfContacts++;
       
        if (NumOfContacts >= 5)
        {
            _paused = true;
            GamePauseCanva();
        }
        StartCoroutine(DiscardNumOfContacts());
    }
    IEnumerator DiscardNumOfContacts()
    {
        yield return new WaitForSeconds(6f);
        if (!_paused)
            NumOfContacts = 0;

    }
}
