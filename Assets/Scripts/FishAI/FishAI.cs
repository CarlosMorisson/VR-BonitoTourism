using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FishAI : MonoBehaviour
{
    public BoxCollider boxCollider;  // Refer�ncia ao Box Collider
    [Header("Speed")]
    [Range(0, 5)]
    public float minSpeed, maxSpeed;
    private float moveSpeed;
    public float rotationSpeed = 5f;// Velocidade do movimento do peixe
    public float detectionRadius = 1f; // Raio de detec��o ao procurar um novo ponto

    private Vector3 targetPosition;    // Posi��o do alvo atual
    private bool isNavigating = false; // Flag para saber se est� navegando
    bool isSelected=false;
    [Header("Audios")]
    [SerializeField]
    AudioClip audioClip;
    [SerializeField]
    AudioSource audioSource;
    void Start()
    {
        // Verifica se o Box Collider foi atribu�do
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider n�o est� atribu�do!");
            return;
        }
        moveSpeed = Random.RandomRange(minSpeed, maxSpeed);
        float randomScale = Random.RandomRange(1f, 3f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        transform.DOScaleZ(1.2f, 2f).SetLoops(-1, LoopType.Yoyo);
        transform.DOScaleY(1.1f, 5f).SetLoops(-1, LoopType.Yoyo);
        // Define o ponto inicial de movimento
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (isNavigating)
        {
            MoveTowardsTarget();

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Chegou ao ponto, ent�o define um novo ponto
                SetRandomTargetPosition();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StopSwim"))
        {
            // Colidiu com um obst�culo, definir novo ponto
            Debug.Log("colidiu");
            SetRandomTargetPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        // Move o peixe em dire��o ao ponto alvo
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Faz o peixe olhar na dire��o do ponto alvo
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.right);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }

    private void SetRandomTargetPosition()
    {
        // Obt�m os limites do Box Collider
        Vector3 center = boxCollider.transform.position;
        Vector3 size = boxCollider.size;

        // Define um novo ponto aleat�rio dentro do Box Collider
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        targetPosition = new Vector3(x, transform.position.y, z);
        isNavigating = true; // Come�a a navegar
    }
    public void FishScore()
    {
        if (!isSelected)
        {
            GameController.instance.GetPoints();
            audioSource.clip = audioClip;
            audioSource.Play();
            isSelected = true;
        }
    }
}