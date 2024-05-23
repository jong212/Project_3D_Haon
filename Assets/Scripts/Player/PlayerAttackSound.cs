using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip baseAttackSound;
    [SerializeField] AudioClip biologySound;
    [SerializeField] AudioClip nonBiologySound;

    void BaseAttack()
    { 
        audioSource.clip = baseAttackSound;
        audioSource.PlayOneShot(baseAttackSound);
    }

    void BiologyAttack()
    {
        audioSource.clip = biologySound;
        audioSource.PlayOneShot(biologySound);
    }

    void NonBiologyAttack()
    {
        audioSource.clip = nonBiologySound;
        audioSource.PlayOneShot(nonBiologySound);
    }
}
