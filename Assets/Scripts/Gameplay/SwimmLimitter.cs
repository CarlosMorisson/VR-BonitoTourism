using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmLimitter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StopSwim"))
        {
            Debug.Log("Stop Swim");
            Swimmer.instance.StopSwim();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StopSwim"))
        {
            Swimmer.instance.stopSwimm=false;
        }
    }
}
