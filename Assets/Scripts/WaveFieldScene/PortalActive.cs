using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortalActive : MonoBehaviour
{
    [SerializeField] private GameObject portal;
    [SerializeField] private float portalActiveDelay = 2.0f;
    
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private float fadeActiveDelay = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivePortal());            
        }
    }

    IEnumerator ActivePortal()
    {
        yield return new WaitForSeconds(portalActiveDelay);
        portal.gameObject.SetActive(true);

        

        yield return new WaitForSeconds(fadeActiveDelay);
        fadeImage.SetActive(true);

        //yield return new WaitForSeconds(1.0f);
        //gameObject.SetActive(false);
    }
}
