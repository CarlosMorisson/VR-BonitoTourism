using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public SOGameController stats;
    #region GetValues
    private string _answer;

    #endregion

    void Start()
    {
        instance = this;

    }

    public void GetAwnser()
    {

    }
}
