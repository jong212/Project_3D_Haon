using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[RequireComponent(typeof(LineRenderer))]
public class Gimick2 : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerMask;
    public float defaultLength = 50f;
    public int numOfReflections = 2;
    public GameObject boss;
    public Boss realboss;
    public GameObject LazerLoc;

    //private materialRocate materialRocateComponent; // Reference to the materialRocate component
    ///public Boss bossObject;
    private LineRenderer _lineRenderer;
    private Camera _myCam;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
       // materialRocateComponent = LazerLoc.GetComponent<materialRocate>();

       _lineRenderer = GetComponent<LineRenderer>();
        if (realboss == null)
        {
            GameObject bossObject = GameObject.FindWithTag("Boss");
            if (bossObject != null)
            {
                realboss = bossObject.GetComponent<Boss>();
            }
        }
        _myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ReflectLaser();
    }

    void ReflectLaser()
    {
        ray = new Ray(transform.position, transform.forward);

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        float remainLength = defaultLength;

        for (int i = 0; i < numOfReflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);

                remainLength -= Vector3.Distance(ray.origin, hit.point);
                if (hit.collider.gameObject.tag == "Wall2") break;

                if (hit.collider.gameObject == boss)
                {
                   // materialRocateComponent.StartScrolling();

                    realboss.LazerStartFiveMin = true;
                    break;
                }

                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
            else
            {
               // materialRocateComponent.StopScrolling();
                realboss.LazerStartFiveMin = false;
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength));
                break;
            }
        }
    }

    void NormalLaser()
    {
        _lineRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
        {
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * defaultLength));
        }
    }
}
