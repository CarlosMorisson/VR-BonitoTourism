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
    [SerializeField]
    float distanceFromPlayer, spacing, verticalOffset;
    private const float ANIMATIONTIME=5;
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
            OnEndGame();
        }
    }
    void OnEndGame()
    {
        StartCoroutine(AlignAndLookAtPlayerOneByOne());
    }

    IEnumerator AlignAndLookAtPlayerOneByOne()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 playerPosition = player.position;
        Vector3 playerForward = player.forward;
        // Número de NPCs por linha
        int npcsPerLine = _allNpc.Length / 2;

        for (int i = 0; i < _allNpc.Length; i++)
        {
            // Determina em qual linha o NPC estará (0 ou 1)
            int line = i / npcsPerLine;

            // Calcula a posição do NPC na linha
            Vector3 offset = playerForward * distanceFromPlayer + player.right * (i % npcsPerLine - npcsPerLine / 2) * spacing;

            // Ajusta a posição para a segunda linha (acima da primeira)
            if (line == 1)
            {
                offset += player.up * verticalOffset; // Adiciona um deslocamento vertical
            }

            // Define a posição final do NPC
            _allNpc[i].DOMove(playerPosition + offset, 1f).SetEase(Ease.InOutSine);

            // Espera até que o movimento termine
            yield return _allNpc[i].DOMove(playerPosition + offset, 1f).WaitForCompletion();

            // Faz o NPC olhar para o jogador
            _allNpc[i].DOLookAt(playerPosition, 0.5f).SetEase(Ease.InOutSine);

            // Espera até que a rotação termine
            yield return _allNpc[i].DOLookAt(playerPosition, 0.5f).WaitForCompletion();
        }
    }
}