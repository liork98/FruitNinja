using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
   public GameObject _whole;
   public GameObject _sliced;
   private Rigidbody fruitRigidBody;
   private Collider fruitCollider;
   private ParticleSystem _juice;
   [HideInInspector]
   public AudioSource sliceAudio;
   
   private void Awake()
   {
      fruitRigidBody = GetComponent<Rigidbody>();
      fruitCollider = GetComponent<Collider>();
      _juice = GetComponentInChildren<ParticleSystem>();
      sliceAudio = transform.GetComponent<AudioSource>();
   }

   private void Slice(Vector3 direction, Vector3 position, float force)
   {
      FindObjectOfType<GameManager>().AddPoint();
      _whole.SetActive(false);
      _sliced.SetActive(true);
      fruitCollider.enabled = false;
      _juice.Play();
      
      //set fruit rotation when sliced
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      _sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
       
      Rigidbody[] slices = _sliced.GetComponentsInChildren<Rigidbody>();

      foreach (Rigidbody slice in slices)
      {
         slice.velocity = fruitRigidBody.velocity;
         slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
      }
   }
   
   private void OnTriggerEnter(Collider other)
   {
      //detect collusion between fruit and the blade only
      if (other.CompareTag(("Player")))
      {
         Blade blade = other.GetComponent<Blade>();
         sliceAudio.Play();
         Slice(blade.direction, blade.transform.position, blade.sliceForce);
      }
   }
}
