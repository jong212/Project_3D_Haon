using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip baseAttackSound;
    [SerializeField] private AudioClip biologySound;
    [SerializeField] private AudioClip nonBiologySound;
    [SerializeField] private AudioClip skillASound;
    [SerializeField] private AudioClip skillBSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip monsterDieSound;
    
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

    public void Dash()
    {
        audioSource.clip = dashSound;
        audioSource.PlayOneShot(dashSound);
    }

    public void SkillA()
    {
        audioSource.clip = skillASound;
        audioSource.PlayOneShot(skillASound);
    }

    public void SkillB()
    {
        audioSource.clip = skillBSound;
        audioSource.PlayOneShot(skillBSound);
    }

    public void MonsterDie()
    {
        audioSource.clip = monsterDieSound;
        audioSource.PlayOneShot(monsterDieSound);
    }
}
