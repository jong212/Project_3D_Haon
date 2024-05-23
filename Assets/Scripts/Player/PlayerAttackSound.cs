using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip baseAttackSound;
    [SerializeField] AudioClip biologySound;
    [SerializeField] AudioClip nonBiologySound;
    [SerializeField] Animator animator;

    ////Attack Sound 추가(준후)
    //if (otherCollider.GetComponent<MonsterType>().monsterType == 1)
    //{
    //    attackSound.BiologyAttack();
    //}
    //else if (otherCollider.GetComponent<MonsterType>().monsterType == 2)
    //{
    //    attackSound.NonBiologyAttack();
    //}
    //else
    //{ 
    //    attackSound.BaseAttack();
    //}

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
