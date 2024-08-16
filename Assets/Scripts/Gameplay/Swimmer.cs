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
    [Header("References")]
    [SerializeField] bool isUnderWater;
    [SerializeField] Camera playerCamera;

    [SerializeField] Transform trackingReference;
    //[HideInInspector]
    public bool goForward;
    Rigidbody _rigidbody;
    float _cooldownTimer;

    void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
      
    }

    void FixedUpdate()
    {
        _cooldownTimer += Time.fixedDeltaTime;
        if (_cooldownTimer > minTimeBetweenStrokes
            && isUnderWater && goForward)
        {
            _rigidbody.AddForce(playerCamera.transform.forward * swimForce, ForceMode.Acceleration);
            _cooldownTimer = 0f;
            goForward = false;
            
        }

        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }
    }
    private void OnTriggerStay(Collider other)
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
