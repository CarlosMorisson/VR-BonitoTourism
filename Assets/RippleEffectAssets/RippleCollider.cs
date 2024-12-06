using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleCollider : MonoBehaviour
{
    public ParticleSystem ripple;
    [SerializeField]
    private float bodyCount;
    private bool inWater;
    [SerializeField]
    private float _count;
    public enum BodyType {
        LeftHad,
        RightHand,
        Bark,
        Body,
        None
    };
    public BodyType bodyType;
    [SerializeField] private ParticleSystem[] _particleSystem;
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
        if (BodyType.Bark == bodyType)
            ripple.Emit(transform.position, Vector3.zero, 5, 0.1f, Color.white);
        else if (BodyType.Body == bodyType)
        {
            _count += Time.deltaTime;
            if (_count > bodyCount)
            {
                ripple.Emit(transform.position, Vector3.zero, 5, 0.1f, Color.white);
                _count = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            ripple.Emit(transform.position, Vector3.zero, 5, 0.1f, Color.white);
            if (BodyType.Bark == bodyType)
            {
                
              foreach(ParticleSystem particle in _particleSystem)
                {
                    particle.Play();
                }
            }
            
        }
        if (other.gameObject.gameObject.CompareTag("Destiny"))
        {
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
                    break;
                case BodyType.RightHand:

                    break;
            }
        }

        if (other.gameObject.CompareTag("Hand"))
        {
            
        }
    }
}
