using UnityEngine;
using DG.Tweening;
public class PointController : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask netLayer;          // Camada da rede
    public LayerMask courtBoundsLayer;  // Camada das bordas da quadra
    public LayerMask courtLayer;        // Camada da quadra v�lida

    [Header("Ball and Players")]
    public Transform enemyBall;
    public Transform ball;             // Refer�ncia � bola
    public Transform playerRacket;     // Raquete do jogador
    public Transform enemyRacket;      // Raquete do inimigo
    public Transform courtCenter;      // Centro da quadra para reset
    public Transform startPos;

    private int playerPoints = 0;      // Pontos do jogador
    private int enemyPoints = 0;       // Pontos do advers�rio
    private int bounceCount = 0;       // N�mero de quiques da bola

    private void OnTriggerEnter(Collider other)
    {
        // Detecta colis�es com a rede
        if (IsInLayerMask(other.gameObject, netLayer))
        {
            HandleNetHit();
        }
        // Detecta colis�es com os limites da quadra
        else if (IsInLayerMask(other.gameObject, courtBoundsLayer))
        {
            HandleOutOfBounds();
        }
        // Verifica se quicou na quadra v�lida
        else if (IsInLayerMask(other.gameObject, courtLayer))
        {
            HandleBounceInCourt();
        }
    }

    private void HandleNetHit()
    {
        Debug.Log("Bola bateu na rede!");
        //AwardPointToOpponent();
    }

    private void HandleOutOfBounds()
    {
        Debug.Log("Bola fora da quadra!");
        //AwardPointToOpponent();
    }

    private void HandleBounceInCourt()
    {
        bounceCount++;
        Debug.Log($"A bola quicou na quadra! Quiques: {bounceCount}");

        if (bounceCount == 1)
        {
            // Primeiro quique est� correto
            if (GetComponent<BallController>().ballType == BallController.BallType.Enemy || GetComponent<BallController>().ballType == BallController.BallType.Player)
            {
                Debug.Log("A bola foi devolvida!");
                bounceCount = 0;
            }
        }
        else if (bounceCount > 1)
        {
            Debug.Log("Double bounce detectado!");
            if (GetComponent<BallController>().ballType == BallController.BallType.Enemy)
            {
                AwardPointToOpponent();
            }
            else if (GetComponent<BallController>().ballType == BallController.BallType.Player)
            {
                //AwardPointToPlayer();
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }

    private void AwardPointToPlayer()
    {
        playerPoints++;
        Debug.Log($"Jogador marcou um ponto! Pontua��o: {playerPoints}");
        UIController.instance.UpdateScore(false, playerPoints);
        ResetBall();
    }

    private void AwardPointToOpponent()
    {
        enemyPoints++;
        Debug.Log($"Advers�rio marcou um ponto! Pontua��o: {enemyPoints}");
        UIController.instance.UpdateScore(true, enemyPoints);
        ResetBall();
    }

    private void ResetBall()
    {
        // Reseta a posi��o da bola no centro da quadra
        ball.gameObject.SetActive(false);
        enemyBall.gameObject.SetActive(true);
        enemyBall.position = startPos.position;
        enemyBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bounceCount = 0;
        enemyBall.transform.DOMoveZ(20, 5).SetLoops(-1, LoopType.Yoyo);
    }

    public void StartMatch()
    {
        playerPoints = 0;
        enemyPoints = 0;
        ResetBall();
        Debug.Log("Partida iniciada!");
    }
}
