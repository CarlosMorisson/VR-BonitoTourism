using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleCollider : MonoBehaviour
{
    public ParticleSystem ripple;
    private bool inWater;
    public enum BodyType {
        LeftHad,
        RightHand,
        None
    };
    public BodyType bodyType;

    // Start is called before the first frame update yup
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        ripple.transform.position = transform.position;
        Shader.SetGlobalVector("_Player", transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            ripple.Emit(transform.position, Vector3.zero, 5, 0.1f, Color.white);
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            ripple.Emit(transform.position, Vector3.zero, 5, 0.1f, Color.white);
        }
        if (other.gameObject.CompareTag("Swim"))
        {
            switch (bodyType)
            {
                case BodyType.LeftHad:
                    Swimmer.instance.goForward = true;
                    break;
                case BodyType.RightHand:

                    break;
            }
        }

    }
}
