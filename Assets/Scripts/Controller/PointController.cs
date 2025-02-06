using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PointController : MonoBehaviour
{
    public static PointController Instance;
    [Header("Layers")]
    public LayerMask netLayer;          // Camada da rede
    public LayerMask courtBoundsLayer;  // Camada das bordas da quadra
    public LayerMask courtLayer;        // Camada da quadra válida

    [Header("Ball and Players")]
    public Transform enemyBall;
    public Transform ball;             // Referência à bola
    public Transform playerRacket;     // Raquete do jogador
    public Transform enemyRacket;      // Raquete do inimigo
    public Transform courtCenter;      // Centro da quadra para reset
    public Transform startPos;

    private int playerPoints = 0;      // Pontos do jogador
    private int enemyPoints = 0;       // Pontos do adversário
    private int bounceCount = 0;       // Número de quiques da bola
    private bool canPoint=true;
    private bool isPointAwarded = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf)
            return;
        // Detecta colisões com a rede
        if (IsInLayerMask(other.gameObject, netLayer))
        {
            HandleNetHit();
        }
        // Detecta colisões com os limites da quadra
        else if (IsInLayerMask(other.gameObject, courtBoundsLayer) && !isPointAwarded)
        {
            HandleOutOfBounds();
            isPointAwarded = true;
        }
        // Verifica se quicou na quadra válida
        else if (IsInLayerMask(other.gameObject, courtLayer))
        {
            Debug.Log("Ativou aqui");
            HandleBounceInCourt();
        }
    }

    private void HandleNetHit()
    {
        Debug.Log("Bola bateu na rede!");
        AwardPointToOpponent();
    }

    private void HandleOutOfBounds()
    {
        Debug.Log("Bola fora da quadra!");
        if (GetComponent<BallController>().ballType == BallController.BallType.Enemy)
        {
            if (bounceCount != 0)
                AwardPointToOpponent();
            else
                AwardPointToPlayer();
        }
        else if(GetComponent<BallController>().ballType == BallController.BallType.Player)
        {
            if (bounceCount != 0)
                AwardPointToPlayer();
            else
                AwardPointToOpponent();
        }
           
    }

    private void HandleBounceInCourt()
    {
        bounceCount++;
        Debug.Log($"A bola quicou na quadra! Quiques: {bounceCount}");

        if (bounceCount == 1)
        {
            // Primeiro quique está correto
            if (GetComponent<BallController>().ballType == BallController.BallType.Enemy || GetComponent<BallController>().ballType == BallController.BallType.Player)
            {
                Debug.Log("A bola foi devolvida!");
                //bounceCount = 0;
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
                AwardPointToPlayer();
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }

    private void AwardPointToPlayer()
    {
        if (canPoint)
        {
            canPoint = false;
            playerPoints++;
            Debug.Log($"Jogador marcou um ponto! Pontuação: {playerPoints}");
            UIController.instance.UpdateScore(false, playerPoints);
            ResetBall();
        }
    }

    private void AwardPointToOpponent()
    {
        if (canPoint)
        {
            canPoint = false;
            enemyPoints++;
            Debug.Log("ativou");
            Debug.Log($"Adversário marcou um ponto! Pontuação: {enemyPoints}");
            UIController.instance.UpdateScore(true, enemyPoints);
            ResetBall();
        }
    }

    public void ResetBall()
    {
        // Desativar a bola do jogador
        ball.gameObject.SetActive(false);

        // Ativar a bola do inimigo e posicioná-la
        enemyBall.gameObject.SetActive(true);
        StartCoroutine(GoToStartPoint());
         
        bounceCount = 0;

        // Permitir a pontuação novamente
        canPoint = true;
       
        isPointAwarded = false;
    }
    private IEnumerator GoToStartPoint()
    {
        yield return new WaitForSeconds(5f);
        enemyBall.position = startPos.position;

        // Zerar a velocidade da bola do inimigo
        enemyBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemyBall.transform.DOMoveZ(20, 5).SetLoops(-1, LoopType.Yoyo);
    }
    public void StartMatch()
    {
        playerPoints = 0;
        enemyPoints = 0;
        ResetBall();
        Debug.Log("Partida iniciada!");
    }
    private void Start()
    {
        Instance = this;
    }
}
