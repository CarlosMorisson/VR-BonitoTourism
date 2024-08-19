using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [Header("Hud")]
    [SerializeField]
    GameObject HudPlayer;
    [SerializeField]
    private TextMeshProUGUI FishScore;
    [SerializeField]
    private TextMeshProUGUI CronometerText;
    [Header("GameOver")]
    [SerializeField]
    GameObject GameOver;
    [SerializeField]
    private TextMeshProUGUI FishScoreGameOver;
    private void Start()
    {
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
        FishScoreGameOver.text = points.ToString();
    }
}
