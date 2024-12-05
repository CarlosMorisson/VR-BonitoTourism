using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class BallController : MonoBehaviour
{
    [Header("Ball Values")]
    public float baseSpeed = 10f;        // Velocidade base da bola
    public const float GRAVITY = 9.8f;         // Intensidade da gravidade
    public float bounceForce = 5f;       // For�a do quique no ch�o
    public LayerMask groundLayer;        // Camada do ch�o
    public LayerMask racketLayer;        // Camada da raquete
    public LayerMask courtBoundsLayer;   // Camada que delimita a quadra
    public float racketSpeedInfluence = 0.5f; // Fator de influ�ncia da velocidade da raquete

    public Vector3 velocity;            // Velocidade atual da bola
    // Define se a bola est� ativa
    [Header ("Enemy ball")]
    [SerializeField]
    private Transform[] enemyDestiny;
    private Dictionary<(int, int), Action> _actionMap;
    private float enemySpeedForce;
    public enum BallType
    {
        Enemy,
        Player
    };
    public BallType ballType;
    private void Start()
    {
        // Inicializa a bola como inativa
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if (BallType.Enemy == ballType)
            return;
        // Aplica gravidade � velocidade vertical
        velocity.y -= GRAVITY * Time.deltaTime;

        // Atualiza a posi��o da bola
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detec��o do ch�o
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            if (velocity.y < 0) // Apenas quicar se estiver descendo
            {
                velocity.y = bounceForce;
            }
            VFXController.Instance.PlayEffect(gameObject.transform, 2);
        }

        // Detec��o da raquete
        else if (((1 << other.gameObject.layer) & racketLayer) != 0)
        {
        /*
            // Obter a velocidade da raquete
            RacketController racket = other.GetComponent<RacketController>();
            if (racket != null)
            {
                Vector3 collisionNormal = other.transform.position; // Dire��o da superf�cie da raquete
                Vector3 reflectedDirection = Vector3.Reflect(racket.enemyBall.transform.position.normalized, collisionNormal); // Dire��o refletida
                float racketSpeed = racket.racketSpeed; // Velocidade da raquete
                Launch(reflectedDirection, racketSpeed);
                 Debug.Log(reflectedDirection);
            }
            //racketSplash.Play();
            // Desativa a bola atual
            Debug.Log(racket.enemyBall.transform.position.normalized);
           */
        }
        else if (other.gameObject.layer == 4)
        {
            VFXController.Instance.PlayEffect(gameObject.transform, 1);
        }
        // Detec��o de fora da quadra
        else if (((1 << other.gameObject.layer) & courtBoundsLayer) != 0)
        {
            // L�gica para marcar pontos (voc� pode implementar sua pr�pria l�gica aqui)
            Debug.Log("Ponto marcado!");
            ResetBall();
        }
    }

    public void Launch(Vector3 direction, float racketSpeed)
    {
        // Ajusta a velocidade com base na velocidade da raquete
        float adjustedSpeed = baseSpeed + (racketSpeed * racketSpeedInfluence);
        velocity = direction.normalized * adjustedSpeed;
    }
    #region EnemyAtack
    private void EnemyAttack()
    {
        _actionMap = new Dictionary<(int, int), Action>
        {
            { (6, 9), ActionForRange6To9 },
            { (3, 6), ActionForRange3To6 },
            { (0, 3), ActionForRange0To3 },
            { (9, 11), ActionForRange9To11 }
        };

        // Gera um n�mero aleat�rio
        enemySpeedForce = UnityEngine.Random.Range(2, 4);
        int randomAttack = UnityEngine.Random.Range(0, 12);
        Debug.Log($"Random attack: {randomAttack}");

        // Executa a a��o correspondente
        PerformAction(randomAttack);

    }
    void PerformAction(int value)
    {
        foreach (var range in _actionMap)
        {
            // Verifica se o valor est� no intervalo
            if (value >= range.Key.Item1 && value < range.Key.Item2)
            {
                range.Value.Invoke(); // Executa a a��o associada ao intervalo
                return;
            }
        }

    }
    void ActionForRange6To9() => transform.DOJump(enemyDestiny[0].transform.position, baseSpeed, 2, enemySpeedForce);
    void ActionForRange3To6() => transform.DOJump(enemyDestiny[1].transform.position, baseSpeed, 2, enemySpeedForce);
    void ActionForRange0To3() => transform.DOJump(enemyDestiny[2].transform.position, baseSpeed, 2, enemySpeedForce);
    void ActionForRange9To11() => transform.DOJump(enemyDestiny[3].transform.position, baseSpeed, 2, enemySpeedForce);
    #endregion
    private void ResetBall()
    {
        // Reseta a posi��o da bola e desativa
        gameObject.SetActive(false);
        velocity = Vector3.zero;
    }
}