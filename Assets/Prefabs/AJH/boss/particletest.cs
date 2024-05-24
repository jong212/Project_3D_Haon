using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particletest : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private Dictionary<GameObject, float> lastCollisionTime;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        lastCollisionTime = new Dictionary<GameObject, float>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "Player2" ||
            other.gameObject.tag == "Player3" ||
            other.gameObject.tag == "Player4")
        {
            if (!lastCollisionTime.ContainsKey(other))
            {
                lastCollisionTime[other] = -3f; // Initialize with a value that allows immediate damage
            }

            if (Time.time - lastCollisionTime[other] >= 3f)
            {
                other.gameObject.GetComponent<playerAnimator>().TakeDamage(30);
                lastCollisionTime[other] = Time.time;
            }
        }
    }
}
