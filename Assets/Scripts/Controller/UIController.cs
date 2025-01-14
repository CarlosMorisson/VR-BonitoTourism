using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [Header("Start Canva")]
    public GameObject StartCanva;
    //Elements
    [SerializeField]
    private Image initialImage;
    [SerializeField]
    private Image kissImage;
    [Header("Hud Canva")]
    public GameObject HudCanva;
    //Elements
    [SerializeField]
    private Image hudImage;
    [SerializeField]
    private TextMeshProUGUI playerScore, enemyScore;
    [SerializeField]
    private TextMeshProUGUI pointText;
    [Header("Final Canva")]
    public GameObject FinalCanva;
    //Elements
    [SerializeField]
    private TextMeshProUGUI playerFinalScore, enemyFinalScore;
    [SerializeField]
    private Image finalImage;
    private void Start()
    {
        instance = this;
    }
    public void UpdateScore(bool isEnemy, int score)
    {
        if (isEnemy)
        {
            enemyScore.text = score.ToString();
            //enemyFinalScore.text = score.ToString();
            pointText.text = "Ponto da Victoria";
        }
        else
        {
            playerScore.text = score.ToString();
            //playerFinalScore.text = score.ToString();
            pointText.text = "Pontuou Amor";
        }

        pointText.DOFade(1, 3)
           .SetEase(Ease.InSine) // Suaviza a subida e descida
            .OnComplete(() =>
            {
                // Executa o segundo salto ao terminar o primeiro
                pointText.DOFade(0, 0.5f)
                       .SetEase(Ease.OutSine)
                       .OnComplete(() =>
                       {
                           pointText.text = "Pode Sacar meu amor";
                           pointText.DOFade(1, 2f)
                                  .SetEase(Ease.OutSine).OnComplete(() =>
                                  {
                                      pointText.DOFade(0, 2f)
                                             .SetEase(Ease.OutSine);
                                  });
                       });
            });
    }
}