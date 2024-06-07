using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoTweenEffects : MonoBehaviour
{
    public void PicturePreviewWindow()
    {
        transform.DOScale(new Vector3(5f, 5f, 5f), 0);
        transform.DOLocalMove(new Vector3(0.06f, -0.05f, -1.4f), 0);
        transform.DOScale(new Vector3(1f, 1f, 1f), 1);
        transform.DOLocalMove(new Vector3(1.77f, -3.58f, -1.4f), 1);
    }
    public void ButtonPhotoScale()
    {
        float originalScale = transform.localScale.x;
        transform.DOScale(new Vector3(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f, transform.localScale.z * 1.01f), 0.1f);
        transform.DOScale(new Vector3(originalScale, originalScale, originalScale), 0f);
    }
    public void OpenWindow()
    {
        float originalScale = transform.localScale.x;
        transform.DOScale(Vector3.zero, 0f);
        transform.DOScale(new Vector3(originalScale, originalScale, originalScale), 1f);
    }
}
