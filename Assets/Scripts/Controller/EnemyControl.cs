using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform PlayerBall;                  // Refer�ncia � bola
    public Transform EnemyBall;
    public Transform racket;               // Refer�ncia � raquete do inimigo
    public float moveSpeed = 5f;           // Velocidade de movimento da raquete
    public float reactionDistance = 10f;   // Dist�ncia para reagir � bola

    private Vector3 initialPosition;       // Posi��o inicial da raquete inimiga 
    public float rotationSpeed = 360f; // Velocidade de rota��o (graus por segundo)
    private bool isRotating = false;
    private void Start()
    {
        initialPosition = racket.position; // Salva a posi��o inicial da raquete
    }

    private void Update()
    {
        // Calcula a dist�ncia at� a bola
        float distanceToBall = Vector3.Distance(PlayerBall.position, racket.position);

        if (distanceToBall <= reactionDistance)
        {
            // Move a raquete em dire��o � posi��o da bola no eixo X e Z
            Vector3 targetPosition = new Vector3(PlayerBall.position.x, racket.position.y, PlayerBall.position.z);
            racket.position = Vector3.MoveTowards(racket.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Retorna para a posi��o inicial quando a bola est� fora da dist�ncia de rea��o
            racket.position = Vector3.MoveTowards(racket.position, initialPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se a colis�o � com a bola
        if (other.gameObject == PlayerBall.gameObject)
        {
            // Inicia a rota��o
            StartCoroutine(Rotate360());
            Debug.Log("Enemy");
            PlayerBall.gameObject.SetActive(false);

            // Posiciona e ativa a bola do jogador
            EnemyBall.transform.rotation = transform.rotation;
            EnemyBall.transform.position = transform.position + transform.forward * 0.5f; // Ajuste de posi��o
            EnemyBall.gameObject.SetActive(true);
            EnemyBall.GetComponent<BallController>().EnemyAttack();
        }
      
    }

    private IEnumerator Rotate360()
    {
        isRotating = true; // Marca como rotacionando
        float totalRotation = 0f;

        // Rotaciona gradualmente at� completar 360 graus
        while (totalRotation < 360f)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            racket.Rotate(Vector3.up, rotationStep); // Roda ao longo do eixo Y (vertical)
            totalRotation += rotationStep;
            yield return null;
        }

        // Corrige a rota��o final (para evitar imprecis�es)
        racket.rotation = Quaternion.Euler(racket.rotation.eulerAngles.x, 0f, racket.rotation.eulerAngles.z);

        isRotating = false; // Marca como n�o rotacionando
    }
}
