using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public AudioClip attackSound, deathSound;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void PlayRunSound()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        source.PlayOneShot(clip);
    }

    private void PlayAttackSound()
    {
        source.PlayOneShot(attackSound);
    }

    private void PlayDeathSound()
    {
        source.PlayOneShot(deathSound);
    }
}
