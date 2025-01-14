using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public SOGameController stats;
    [SerializeField]
    private AudioClip positiveFeedback, negativeFeedback, takePhotoFirst;
    private AudioSource _audioSource;
    [SerializeField]
    private string[] _questionList= new string[6];
    private GameObject[] _textQuestion;
    [HideInInspector]
    public string _playerAnwser;

    #region GetValues
    private string _answer;
    private string[] _questionsAlternatives;
    #endregion

    void Start()
    {
        instance = this;
        _audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        GetAwnser();
        GetQuestions();
        RandomizeQuestions();
        DisplayQuestions();
    }
    #region GetAndRandomizeQuestions
    private void GetQuestions()
    {
        _textQuestion = GameObject.FindGameObjectsWithTag("Question");
        Debug.Log(_textQuestion[0].gameObject.name) ;
        for(int i = 0; i < _textQuestion.Length; i++)
        {
            _questionList[i] = _textQuestion[i].GetComponentInChildren<TextMeshProUGUI>().text;
        }
    }
    private void RandomizeQuestions()
    {
        List<string> randomAlternatives = new List<string>(_questionsAlternatives);
        randomAlternatives.Remove(_answer);
        List<string> selectedAlternatives = new List<string>();

        for (int i = 0; i < _questionList.Length - 1; i++)
        {
            int randomIndex = Random.Range(0, randomAlternatives.Count);
            selectedAlternatives.Add(randomAlternatives[randomIndex]);
            randomAlternatives.RemoveAt(randomIndex);
        }
        int answerPosition = Random.Range(0, _questionList.Length);
        selectedAlternatives.Insert(answerPosition, _answer);
        for (int i = 0; i < _questionList.Length; i++)
        {
            _questionList[i] = selectedAlternatives[i];
        }
    }
    private void DisplayQuestions()
    {
        for (int i = 0; i < _textQuestion.Length; i++)
        {
            _textQuestion[i].GetComponentInChildren<TextMeshProUGUI>().text = _questionList[i];
        }
    }
    #endregion
    public void GetAwnser()
    {
        _questionsAlternatives = stats.AnswerAlternatives;
        _answer = stats.Answer;
    }
    private IEnumerator EndGame()
    {
        
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);

    }
    #region GetAnwserOfPlayer
    public void GetInteractedAnwser(TextMeshProUGUI text)
    {
        _playerAnwser = text.text;
    }
    public void SendAnwser()
    {
        if (TakePhoto.instance.checkCollision)
        {
            if (_answer == _playerAnwser)
            {
                UIController.instance.WinOrLoseCanva(true);
                _audioSource.clip = positiveFeedback;
                _audioSource.Play();
                StartCoroutine(EndGame());
            }
            else
            {
                UIController.instance.WinOrLoseCanva(false);
                _audioSource.clip = negativeFeedback;
                _audioSource.Play();
                StartCoroutine(EndGame());
            }
        }
        else
        {
            _audioSource.clip = takePhotoFirst;
            _audioSource.Play();
        }
        
    }
    #endregion
}
