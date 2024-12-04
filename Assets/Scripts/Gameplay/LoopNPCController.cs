using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class LoopNPCController : MonoBehaviour
{
    [SerializeField]
    private Transform _npcParent;
    [SerializeField]
    private Transform[] _allNpc;
    private int _randomSpeed;
    void Start()
    {
        _npcParent = GameObject.FindGameObjectWithTag("NPC").transform;
        _allNpc = _npcParent.transform.GetComponentsInChildren<Transform>()
                .Where(t => t != _npcParent.transform) // Filtra o objeto pai
                .ToArray();
        foreach(Transform transform in _allNpc)
        {
            float fixedX = transform.localEulerAngles.x;
            float fixedY = transform.localEulerAngles.y;
            _randomSpeed = Random.RandomRange(1, 10);
            transform.DOLocalRotate(new Vector3(fixedX, fixedY, 20f), _randomSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        }
    }
}
