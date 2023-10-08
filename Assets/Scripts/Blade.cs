using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera _mainCamera;
    private Collider _bladeCollider;
    private TrailRenderer _bladeTrail;
    private bool isSlicing;
    public Vector3 direction { get; private set; }
    public float sliceForce = 5f;
    public float minSliceSpeed = 0.01f;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _bladeCollider = GetComponent<Collider>();
        _bladeTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//if the left click is pressed
        {
            StartSlicing();
        }
        else if (Input.GetMouseButtonUp(0))//if not pressed
        {
            StopSlicing();
        }
        else if (isSlicing)//while slicing
        {
            ContinueSlicing();
        }
    }

    private void StartSlicing()
    {
        //init the blade position
        Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;
        
        isSlicing = true;
        _bladeCollider.enabled = true;
        _bladeTrail.enabled = true;
        _bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        isSlicing = false;
        _bladeCollider.enabled = false;
        _bladeTrail.enabled = false;
    }
    
    private void ContinueSlicing()
    {
        Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position; //moving blade direction

        float speed = direction.magnitude / Time.deltaTime; //moving blade speed
        _bladeCollider.enabled = speed > minSliceSpeed;

        transform.position = newPosition;
    }
}
