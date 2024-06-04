using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionLocation : MonoBehaviour
{
    [SerializeField]
    private Transform _playerPos;
    private Transform[] _positionList;
    private int _indexPos;
    void Start()
    {
        _positionList = Introduction.instance._stats.arrowLocals;
        transform.position = _positionList[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _playerPos.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        //GetLocation();
    }
    private void GetLocation()
    {
        float distance = Vector3.Distance(_playerPos.position, transform.position);
        if (distance < 1)
        {
            _indexPos++;
            transform.position = _positionList[_indexPos].position;
        }
    }
}
