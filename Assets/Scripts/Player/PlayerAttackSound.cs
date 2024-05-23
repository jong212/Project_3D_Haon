using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip baseAttackSound;
    [SerializeField] AudioClip biologySound;
    [SerializeField] AudioClip nonBiologySound;

    public void BaseAttack()
    { 
        audioSource.clip = baseAttackSound;
        audioSource.PlayOneShot(baseAttackSound);
    }

    public void BiologyAttack()
    {
        audioSource.clip = biologySound;
        audioSource.PlayOneShot(biologySound);
    }

    public void NonBiologyAttack()
    {
        audioSource.clip = nonBiologySound;
        audioSource.PlayOneShot(nonBiologySound);
    }
}
