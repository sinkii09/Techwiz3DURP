using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public AudioSource audioSource; // Only one AudioSource for all SFX

    // Footstep, jump, and dash sound effects
    public AudioClip footstepClip;
    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip spikeClip;
    public AudioClip hurtClip;

    public void PlayHurtClip()
    {
        audioSource.PlayOneShot(hurtClip);
    }
    // Function to play a footstep sound
    public void PlayFootstep()
    {
        audioSource.PlayOneShot(footstepClip);
    }

    // Function to play a jump sound
    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    // Function to play a dash sound
    public void PlayDash()
    {
        audioSource.PlayOneShot(dashClip);
    }
    public void PlaySpike()
    {
        audioSource.PlayOneShot(spikeClip);
    }
}
