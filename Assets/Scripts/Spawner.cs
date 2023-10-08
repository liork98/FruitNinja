using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
   private Collider spawnArea;
   public GameObject[] _fruitsArr;
   public float minSpawnDelay = 0.25f;
   public float maxSpawnDelay = 1f;
   public float minAngle = -15f;
   public float maxAngle = 15f;
   public float minForce = 18f;
   public float maxForce = 22f;
   public float maxLifetime = 5f;
   public GameObject _bomb;
   [Range(0f,1f)]
   public float bombRate = 0.05f;
   
   private void Awake()
   {
      spawnArea = GetComponent<Collider>();
   }

   private void OnEnable()
   {
      StartCoroutine(Spawn());
   }

   private void OnDisable()
   {
      StopAllCoroutines();
   }

   private IEnumerator Spawn()
   {
      yield return new WaitForSeconds(2f);
      while (enabled)
      {
         GameObject prefab = _fruitsArr[Random.Range(0, _fruitsArr.Length)]; //the fruit that will be chosen

         if (Random.value < bombRate)
         {
            prefab = _bomb;
         }
         
         Vector3 position = new Vector3();
         //random a new position for the fruit
         position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
         position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
         position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

         Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

         GameObject fruit = Instantiate(prefab, position, rotation); //create new fruit on screen
         Destroy(fruit, maxLifetime); //destroy it

         float force = Random.Range(minForce, maxForce);
         fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
         
         yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
      }
   }
}
