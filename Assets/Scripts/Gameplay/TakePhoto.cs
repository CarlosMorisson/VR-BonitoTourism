using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TakePhoto : MonoBehaviour
{
    [Header("TakePhoto Settings")]
    [SerializeField]
    private Camera captureCamera; // Referência para a câmera que captura a screenshot
    [SerializeField]
    private Image picture;
    [SerializeField]
    private Image secondPicture;
    [Header("CheckCollider")]
    [Range(0,5)]
    [SerializeField]
    public float radius = 1.0f; 
    public LayerMask layerMask; 
    private RenderTexture _renderTexture;
    private Texture2D _textureForPicture;
    void Start()
    {
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
        picture.material.mainTexture = _textureForPicture;
        secondPicture.sprite = sprite;
    }

    #endregion
    #region CheckColliders
    private void CheckColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        // Itera sobre os colisores encontrados
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Water"))
            {
                // Faça algo quando colidir com um objeto que tenha a tag "Water"
                Debug.Log("Collision with Water object detected!");
            }
        }
    }
    #endregion
    public void TakePic()
    {
        CaptureScreenshotAndConvert();
        CheckColliders();
    }
    public void Update()
    {
        picture.material.mainTexture = _textureForPicture;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
