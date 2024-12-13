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

}
