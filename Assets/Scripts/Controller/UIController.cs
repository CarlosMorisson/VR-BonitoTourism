using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField]
    private GameObject Questions;
    [SerializeField]
    private GameObject Feedback;
    [SerializeField]
    private TextMeshProUGUI _textGiveAnwser, _textRightAnwser;
    
    void Start()
    {
        instance = this;
        StartCoroutine(closeQuestions());
        
    }
    public void WinOrLoseCanva(bool win)
    {
        Feedback.SetActive(true);
        _textGiveAnwser.text = "Resposta Dada: " + GameController.instance._playerAnwser;
        _textRightAnwser.text = "Resposta Correta: " + GameController.instance.stats.Answer;
        if (win)
        {
            _textGiveAnwser.color = Color.green;
            _textRightAnwser.color = Color.green;
        }
        else
        {
            _textGiveAnwser.color = Color.red;
            _textRightAnwser.color = Color.red;
        }
    }
    private IEnumerator closeQuestions()
    {
        yield return new WaitForSeconds(2f);
        Questions.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
