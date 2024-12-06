using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform PlayerBall;                  // Referência à bola
    public Transform EnemyBall;
    public Transform racket;               // Referência à raquete do inimigo
    public float moveSpeed = 5f;           // Velocidade de movimento da raquete
    public float reactionDistance = 10f;   // Distância para reagir à bola

    private Vector3 initialPosition;       // Posição inicial da raquete inimiga 
    public float rotationSpeed = 360f; // Velocidade de rotação (graus por segundo)
    private bool isRotating = false;
    private void Start()
    {
        initialPosition = racket.position; // Salva a posição inicial da raquete
    }

    private void Update()
    {
        // Calcula a distância até a bola
        float distanceToBall = Vector3.Distance(PlayerBall.position, racket.position);

        if (distanceToBall <= reactionDistance)
        {
            // Move a raquete em direção à posição da bola no eixo X e Z
            Vector3 targetPosition = new Vector3(PlayerBall.position.x, racket.position.y, PlayerBall.position.z);
            racket.position = Vector3.MoveTowards(racket.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Retorna para a posição inicial quando a bola está fora da distância de reação
            racket.position = Vector3.MoveTowards(racket.position, initialPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se a colisão é com a bola
        if (other.gameObject == PlayerBall.gameObject)
        {
            // Inicia a rotação
            StartCoroutine(Rotate360());
            Debug.Log("Enemy");
            PlayerBall.gameObject.SetActive(false);

            // Posiciona e ativa a bola do jogador
            EnemyBall.transform.rotation = transform.rotation;
            EnemyBall.transform.position = transform.position + transform.forward * 0.5f; // Ajuste de posição
            EnemyBall.gameObject.SetActive(true);
            EnemyBall.GetComponent<BallController>().EnemyAttack();
        }
      
    }

    private IEnumerator Rotate360()
    {
        isRotating = true; // Marca como rotacionando
        float totalRotation = 0f;

        // Rotaciona gradualmente até completar 360 graus
        while (totalRotation < 360f)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            racket.Rotate(Vector3.up, rotationStep); // Roda ao longo do eixo Y (vertical)
            totalRotation += rotationStep;
            yield return null;
        }

        // Corrige a rotação final (para evitar imprecisões)
        racket.rotation = Quaternion.Euler(racket.rotation.eulerAngles.x, 0f, racket.rotation.eulerAngles.z);

        isRotating = false; // Marca como não rotacionando
    }
}
