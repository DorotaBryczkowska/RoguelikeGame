using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public AudioClip attackSound;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void PlayAttackSound()
    {
        source.PlayOneShot(attackSound);
    }

}
