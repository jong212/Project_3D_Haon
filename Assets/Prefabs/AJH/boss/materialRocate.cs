using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialRocate : MonoBehaviour
{
    [SerializeField] public Material material;
    [SerializeField] private float initialSpeed = 0.05f; // Initial speed at which the texture starts scrolling
    [SerializeField] private float maxSpeed = 0.8f; // Maximum speed the texture scrolls
    [SerializeField] private float acceleration = 0.01f; // Rate at which the scroll speed increases
    [SerializeField] private float deceleration = 0.02f; // Rate at which the scroll speed decreases
    [SerializeField] private bool isScrolling = false; // Initial value is false to start with no scrolling

    private float scrollSpeed;
    private Vector2 offset = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrolling)
        {
            // Accelerate the scroll speed until it reaches maxSpeed
            if (scrollSpeed < maxSpeed)
            {
                scrollSpeed += acceleration * Time.deltaTime;
                if (scrollSpeed > maxSpeed)
                {
                    scrollSpeed = maxSpeed;
                }
            }
        }
        else
        {
            // Decelerate the scroll speed until it stops
            if (scrollSpeed > 0)
            {
                scrollSpeed -= deceleration * Time.deltaTime;
                if (scrollSpeed < 0)
                {
                    scrollSpeed = 0;
                }
            }
        }

        // Calculate the new offset based on the scroll speed and time
        offset.x += scrollSpeed * Time.deltaTime;

        // Ensure the offset wraps around by using modulo operation
        if (offset.x > 1.0f)
        {
            offset.x -= 1.0f;
        }

        // Apply the offset to the material
        material.SetTextureOffset("_MainTex", offset);
    }

    // Public method to start scrolling
    public void StartScrolling()
    {
        isScrolling = true;
    }

    // Public method to stop scrolling
    public void StopScrolling()
    {
        isScrolling = false;
    }
}
