using UnityEngine;

public class RacketController : MonoBehaviour
{
    public GameObject playerBall; // A bola do jogador
    public GameObject enemyBall;  // A bola do inimigo

    private Vector3 lastPosition; // Armazena a posição anterior da raquete
    public float racketSpeed { get; private set; } // Velocidade da raquete
    public Vector3 lastVelocity {get; private set; }
    private void Start()
    {
        // Inicializa a posição da raquete
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Calcula a velocidade da raquete com base no deslocamento
        racketSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se a raquete colidiu com a bola do inimigo
        if (other.gameObject == enemyBall)
        {
            // Obtém a direção da bola inimiga
            Vector3 relativePosition = enemyBall.transform.position - transform.position;

            // Verifica o lado da colisão com base no produto escalar
            float dotProduct = Vector3.Dot(relativePosition, transform.right);

            // Determina a direção de lançamento
            Vector3 launchDirection = dotProduct > 0 ? transform.right : -transform.right;
            // Desativa a bola do inimigo
            enemyBall.SetActive(false);

            // Posiciona e ativa a bola do jogador
            playerBall.transform.rotation = transform.rotation;
            playerBall.transform.position = transform.position + transform.forward * 0.5f; // Ajuste de posição
            playerBall.SetActive(true);

            // Lança a bola do jogador com a direção da bola inimiga
            float enemySpeed = enemyBall.GetComponent<BallController>().velocity.magnitude; // Obtém a velocidade da bola inimiga
            playerBall.GetComponent<BallController>().Launch(launchDirection, enemySpeed);
        }
    }
}
