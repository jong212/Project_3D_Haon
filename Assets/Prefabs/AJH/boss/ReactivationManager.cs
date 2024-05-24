using System.Collections;
using UnityEngine;

public class ReactivationManager : MonoBehaviour
{
    public void ReactivateAfterDelay(GameObject obj, float delay)
    {
        StartCoroutine(ReactivationCoroutine(obj, delay));
    }

    private IEnumerator ReactivationCoroutine(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
        lazer_top_hp hpScript = obj.GetComponent<lazer_top_hp>();
        if (hpScript != null)
        {
            hpScript.ResetHP();
        }
    }
}
