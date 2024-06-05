using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using Unity.XR.CoreUtils;

public class NewXROrigin : MonoBehaviour
{
    XROrigin xrOrigin;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    [SerializeField]
    private Transform playerModel;
    //
    private Transform headRig;
    private Transform leftRig;
    private Transform rightRig;
    void Awake()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        //
        headRig = xrOrigin.transform.Find("Camera Offset/Main Camera");
        leftRig = xrOrigin.transform.Find("Camera Offset/LeftHand Controller");
        rightRig = xrOrigin.transform.Find("Camera Offset/RightHand Controller");
/*
        foreach(var item in GetComponentsInChildren<Renderer>())
        {
            item.enabled = false;
        }
*/
    }
    
    // Update is called once per frame
    void Update()
    {
        Quaternion headRotation = head.rotation;

        Quaternion newRotation = Quaternion.Euler(playerModel.eulerAngles.x, headRotation.eulerAngles.y, playerModel.eulerAngles.z);

        playerModel.rotation = newRotation;
        playerModel.position = new Vector3(head.transform.position.x, head.transform.position.y-0.8f, head.transform.position.z);
        #region Coments
        /*
         rightHand.gameObject.SetActive(false);
         leftHand.gameObject.SetActive(false);
         head.gameObject.SetActive(false);

         MapPosition(head, headRig);
         MapPosition(leftHand, leftRig);
         MapPosition(rightHand, rightRig);*/
        #endregion

    }
    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;

        rightHand.gameObject.SetActive(true);
        leftHand.gameObject.SetActive(true);
        head.gameObject.SetActive(true);
        target.rotation = rigTransform.rotation;
    }
}
