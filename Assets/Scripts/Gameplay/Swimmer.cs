using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    public static Swimmer instance;
    [Header("Values")]
    [SerializeField] float swimForce = 2f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce;
    [SerializeField] float minTimeBetweenStrokes;
    [SerializeField] float buoyancyForce = 5f;
    [SerializeField] float floatThreshold = 0.1f; 
    [SerializeField] bool isUnderWater;
    [Header("References")]
    [SerializeField] Camera playerCamera;

    [SerializeField] Transform trackingReference;
    [HideInInspector]
    public bool goForward, stopSwimm;
    Rigidbody _rigidbody;
    float _cooldownTimer;
    [Header("Audios")]
    [SerializeField]
    AudioSource AudioSourcePlayer;
    [SerializeField]
    AudioClip[] AudioDive;

    void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        _cooldownTimer += Time.fixedDeltaTime;

        // Movimento de nado
        if (_cooldownTimer > minTimeBetweenStrokes && isUnderWater && goForward)
        {
            _rigidbody.AddForce(playerCamera.transform.forward * swimForce, ForceMode.Acceleration);
            _cooldownTimer = 0f;
            goForward = false;

            // Som de nado
            AudioSourcePlayer.clip = AudioDive[UnityEngine.Random.Range(0, AudioDive.Length)];
            AudioSourcePlayer.Play();
        }

        // Aplicar força de arrasto
        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }

        if (isUnderWater && _rigidbody.velocity.magnitude < floatThreshold)
        {

            _rigidbody.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }
    }

    public void StopSwim()
    {
        _rigidbody.velocity = Vector3.zero;
        goForward = false;
        stopSwimm = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            isUnderWater = true;
            _rigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            isUnderWater = false;
            _rigidbody.useGravity = true;
        }
    }
}
