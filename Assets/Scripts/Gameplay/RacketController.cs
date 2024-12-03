using UnityEngine;

public class RacketController : MonoBehaviour
{
    public GameObject playerBall; // A bola do jogador
    public GameObject enemyBall;  // A bola do inimigo

    private Vector3 lastPosition; // Armazena a posição anterior da raquete
    public float racketSpeed { get; private set; } // Velocidade da raquete

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
            // Desativa a bola do inimigo
            enemyBall.SetActive(false);

            // Posiciona e ativa a bola do jogador
            playerBall.transform.position = transform.position + transform.forward * 0.5f; // Ajuste de posição
            playerBall.SetActive(true);
        }
    }
}
