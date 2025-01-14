using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class BallController : MonoBehaviour
{
    [Header("Ball Values")]
    public float baseSpeed = 10f;        // Velocidade base da bola
    public const float GRAVITY = 9.8f;  // Intensidade da gravidade
    public float bounceForce = 5f;      // Força do quique no chão
    public LayerMask groundLayer;       // Camada do chão
    public LayerMask racketLayer;       // Camada da raquete
    public LayerMask courtBoundsLayer;  // Camada que delimita a quadra
    public float racketSpeedInfluence = 0.5f; // Fator de influência da velocidade da raquete
    public Vector3 velocity;            // Velocidade atual da bola

    [Header("Trail Renderer")]
    private LineRenderer lineRenderer;  // LineRenderer para desenhar a trajetória
    public int predictionSteps = 50;    // Número de passos para prever a trajetória
    public float predictionTimeStep = 0.05f; // Intervalo de tempo para os cálculos

    [Header("Enemy ball")]
    [SerializeField]
    private Transform[] enemyDestiny;
    private Dictionary<(int, int), Action> _actionMap;
    private float enemySpeedForce;
    [SerializeField]
    private float enemyJumpForce;

    public enum BallType
    {
        Enemy,
        Player
    };

    public BallType ballType;

    private void Start()
    {
        // Configura o LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configurações básicas do LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(255, 255, 0,0.3f);
        lineRenderer.endColor = new Color(255, 0, 0, 0.3f);
    }

    private void Update()
    {
        if (BallType.Enemy == ballType)
            return;

        // Aplica gravidade à velocidade vertical
        velocity.y -= GRAVITY * Time.deltaTime;

        // Atualiza a posição da bola
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecção do chão
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            if (velocity.y < 0) // Apenas quicar se estiver descendo
            {
                velocity.y = bounceForce;
            }
            VFXController.Instance.PlayEffect(gameObject.transform, 2);
        }

        // Detecção da raquete
        else if (((1 << other.gameObject.layer) & racketLayer) != 0)
        {
            ResetTrail();
        }

        // Detecção do inimigo
        else if (other.CompareTag("Enemy"))
        {
            ResetTrail();
        }

        // Detecção de fora da quadra
        else if (((1 << other.gameObject.layer) & courtBoundsLayer) != 0)
        {
            Debug.Log("Ponto marcado!");
            ResetBall();
        }
    }

    public void Launch(Vector3 direction, float racketSpeed, float lastSpeed)
    {
        // Ajusta a velocidade com base na velocidade da raquete
        float adjustedSpeed = baseSpeed + (racketSpeed * racketSpeedInfluence) + lastSpeed / 2;
        velocity = direction.normalized * adjustedSpeed;

        // Desenha a trajetória antes do lançamento
    }

    private void DrawParabolicTrajectory(Vector3 startPoint, Vector3 endPoint, float height, int resolution)
    {
        lineRenderer.positionCount = resolution + 1;
        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            Vector3 point = CalculateParabolaPoint(startPoint, endPoint, height, t);
            lineRenderer.SetPosition(i, point);
        }
    }

    private Vector3 CalculateParabolaPoint(Vector3 startPoint, Vector3 endPoint, float height, float t)
    {
        float parabolicT = 4 * t * (1 - t);
        Vector3 heightOffset = Vector3.up * height * parabolicT;
        return Vector3.Lerp(startPoint, endPoint, t) + heightOffset;
    }

    private void ResetTrail()
    {
        // Reseta o rastro da bola
        lineRenderer.positionCount = 0;
    }

    private void ResetBall()
    {
        // Reseta a posição e desativa a bola
        gameObject.SetActive(false);
        velocity = Vector3.zero;

        // Reseta o trail
        ResetTrail();
    }

    #region EnemyAttack
    public void EnemyAttack()
    {
        _actionMap = new Dictionary<(int, int), Action>
        {
            { (6, 9), ActionForRange6To9 },
            { (3, 6), ActionForRange3To6 },
            { (0, 3), ActionForRange0To3 },
            { (9, 11), ActionForRange9To11 }
        };

        // Gera um número aleatório
        enemySpeedForce = UnityEngine.Random.Range(2, 3);
        int randomAttack = UnityEngine.Random.Range(0, 12);
        Debug.Log($"Random attack: {randomAttack}");

        // Executa a ação correspondente
        PerformAction(randomAttack);
    }

    void PerformAction(int value)
    {
        foreach (var range in _actionMap)
        {
            if (value >= range.Key.Item1 && value < range.Key.Item2)
            {
                range.Value.Invoke();
                return;
            }
        }
    }

    void ActionForRange3To6()
    {
        Vector3 start = transform.position;
        Vector3 end = enemyDestiny[0].position;
        float jumpHeight = enemyJumpForce; // Ajuste conforme necessário
        int trajectoryResolution = 20; // Número de pontos na linha

        // Desenha a trajetória parabólica
        DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);
        // Primeiro salto
        transform.DOJump(end, enemyJumpForce, 1, enemySpeedForce)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // Ativa o LineRenderer no impacto
                Vector3 nextEnd = enemyDestiny[0].GetChild(0).position;
                DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);

                // Segundo salto
                transform.DOJump(nextEnd, enemyJumpForce / 2, 1, enemySpeedForce)
                    .SetEase(Ease.OutSine);
            });
    }

    void ActionForRange6To9()
    {
        Vector3 start = transform.position;
        Vector3 end = enemyDestiny[1].position;
        float jumpHeight = enemyJumpForce; // Ajuste conforme necessário
        int trajectoryResolution = 20; // Número de pontos na linha

        // Desenha a trajetória parabólica
        DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);
        // Primeiro salto
        transform.DOJump(end, enemyJumpForce, 1, enemySpeedForce)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // Ativa o LineRenderer no impacto
                Vector3 nextEnd = enemyDestiny[1].GetChild(0).position;
                DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);

                // Segundo salto
                transform.DOJump(nextEnd, enemyJumpForce / 2, 1, enemySpeedForce)
                    .SetEase(Ease.OutSine);
            });
    }

    void ActionForRange0To3()
    {
        Vector3 start = transform.position;
        Vector3 end = enemyDestiny[2].position;
        float jumpHeight = enemyJumpForce; // Ajuste conforme necessário
        int trajectoryResolution = 20; // Número de pontos na linha

        // Desenha a trajetória parabólica
        DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);
        // Primeiro salto
        transform.DOJump(end, enemyJumpForce, 1, enemySpeedForce)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // Ativa o LineRenderer no impacto
                Vector3 nextEnd = enemyDestiny[2].GetChild(0).position;
                DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);

                // Segundo salto
                transform.DOJump(nextEnd, enemyJumpForce / 2, 1, enemySpeedForce)
                    .SetEase(Ease.OutSine);
            });
    }

    void ActionForRange9To11()
    {
        Vector3 start = transform.position;
        Vector3 end = enemyDestiny[3].position;
        float jumpHeight = enemyJumpForce; // Ajuste conforme necessário
        int trajectoryResolution = 20; // Número de pontos na linha

        // Desenha a trajetória parabólica
        DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);
        // Primeiro salto
        transform.DOJump(end, enemyJumpForce, 1, enemySpeedForce)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // Ativa o LineRenderer no impacto
                Vector3 nextEnd = enemyDestiny[3].GetChild(0).position;
                DrawParabolicTrajectory(start, end, jumpHeight, trajectoryResolution);

                // Segundo salto
                transform.DOJump(nextEnd, enemyJumpForce / 2, 1, enemySpeedForce)
                    .SetEase(Ease.OutSine);
            });
    }


    #endregion
}