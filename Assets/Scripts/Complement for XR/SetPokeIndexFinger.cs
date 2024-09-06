using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeIndexFinger : MonoBehaviour
{
    public Transform IndexFinger;
    [SerializeField]
    private XRPokeInteractor xrPokeInteractor;
    public enum DifferentHands
    {
        MeshHand,
        ControlHand
    };
    public DifferentHands differentHand;
    [SerializeField]
    private GameObject Circle;
    [SerializeField]
    int NumOfContacts;
    bool _paused;
    void OnEnable()
    {
        

        switch (differentHand)
        {
            case (DifferentHands.ControlHand):
                xrPokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
                break;
            case (DifferentHands.MeshHand):
                Debug.Log(transform.GetChild(2).GetChild(1).gameObject.name);
                xrPokeInteractor = transform.GetChild(2).GetChild(1).GetComponentInChildren<XRPokeInteractor>();
                break;
        }
        SetPokePoint();
    }
    public void SetPokePoint()
    {
        xrPokeInteractor.attachTransform = IndexFinger;
        Circle.transform.localScale = new Vector3(xrPokeInteractor.pokeSelectWidth, xrPokeInteractor.pokeSelectWidth, xrPokeInteractor.pokeSelectWidth);
    }
}
