using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TakePhoto : MonoBehaviour
{
    public static TakePhoto instance;
    #region TakePhoto Settings
    [Header("TakePhoto Settings")]
    [SerializeField]
    private Camera captureCamera; // Referência para a câmera que captura a screenshot
    [SerializeField]
    private Image picture;
    [SerializeField]
    private Image secondPicture;
    #endregion
    #region Check Collider
    [Header("CheckCollider")]
    [Range(0,5)]
    [SerializeField]
    public float radius = 1.0f;
    public bool checkCollision=false;
    public LayerMask layerMask; 
    private RenderTexture _renderTexture;
    private Texture2D _textureForPicture;
    #endregion
    #region AudioSettings
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip nearFraud, farFraud;
    #endregion
    #region CheckDistance
    [Header("CheckDistance")]
    [SerializeField]
    Transform pocketTransform, playerTransform;
    #endregion
    void Start()
    {
        instance = this;
        _audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        _renderTexture = new RenderTexture(captureCamera.pixelWidth, captureCamera.pixelHeight, 24);
    }
    #region CapturePicture
    public void CaptureScreenshotAndConvert()
    {
        RenderTexture currentRT = captureCamera.targetTexture;
        captureCamera.targetTexture = _renderTexture;
        captureCamera.Render();
        _textureForPicture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = _renderTexture;
        _textureForPicture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        _textureForPicture.Apply();
       
        captureCamera.targetTexture = currentRT;
        RenderTexture.active = null;
        Sprite sprite = Sprite.Create(_textureForPicture, new Rect(0, 0, _textureForPicture.width, _textureForPicture.height), Vector2.zero);
        picture.sprite = sprite;
        secondPicture.sprite = sprite;
    }
    public void TakePic()
    {
        
        CheckColliders();
        CaptureScreenshotAndConvert();
        PlayAudioFeedback();
    }
    private void PlayAudioFeedback()
    {
        if (checkCollision)
        {
            _audioSource.clip = nearFraud;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = farFraud;
            _audioSource.Play();
        }
    }
    #endregion
    #region CheckColliders
    private void CheckColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Water"))
            {
                checkCollision = true;
            }
        }
    }
    #endregion
    #region CheckDistance
    private void CheckDistanceToPlayer()
    {
        float distance = Vector3.Distance(playerTransform.position, this.transform.position);
        if (distance > 1)
        {
            transform.position = pocketTransform.position;
        }
    }
    #endregion
    public void Update()
    {
        CheckDistanceToPlayer();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
