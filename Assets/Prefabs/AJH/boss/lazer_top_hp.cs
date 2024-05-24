using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazer_top_hp : MonoBehaviour
{
    public float hp;
    [SerializeField] ParticleSystem Lazer_Effect2;
    [SerializeField] ParticleSystem Lazer_Effect3;
    [SerializeField] ReactivationManager reactivationManager; // Reference to the ReactivationManager
    private bool isDeactivated = false;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !isDeactivated)
        {
            StartCoroutine(DeactivateAndReactivate());
            return;
        }

        if (hp == 50)
        {
            Lazer_Effect2.Play();
        }
        if (hp == 1)
        {
            
            Lazer_Effect2.Stop();
            Lazer_Effect3.Play();
            
        }
    }

    public void hitDamage(float damage)
    {
        hp -= damage;
    }

    public void ResetHP()
    {
        hp = 100; // Reset hp
        isDeactivated = false;
    }

    private IEnumerator DeactivateAndReactivate()
    {
        isDeactivated = true;
        gameObject.SetActive(false);
        reactivationManager.ReactivateAfterDelay(gameObject, 10);
        yield return null;
    }
}
