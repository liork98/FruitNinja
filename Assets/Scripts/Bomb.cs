using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector]
    public AudioSource bombAudio;

    private void Awake()
    {
        bombAudio = transform.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect collusion between a bomb and the blade
        if (other.CompareTag(("Player")))
        {
            FindObjectOfType<GameManager>().GameOver();
            bombAudio.Play();
        }
    }
}
