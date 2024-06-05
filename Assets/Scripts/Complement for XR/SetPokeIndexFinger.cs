using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeIndexFinger : MonoBehaviour
{
    public Transform IndexFinger;
    [SerializeField]
    private XRPokeInteractor xrPokeInteractor;
    [SerializeField]
    private GameObject Circle;
    void OnEnable()
    {
        Debug.Log(transform.parent.parent.gameObject.name);
        xrPokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
        
        SetPokePoint();
    }
    public void SetPokePoint()
    {

        xrPokeInteractor.attachTransform = IndexFinger;
        Circle.transform.localScale = new Vector3(xrPokeInteractor.pokeSelectWidth, xrPokeInteractor.pokeSelectWidth, xrPokeInteractor.pokeSelectWidth);
    }
}
