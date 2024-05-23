using UnityEngine;

public class StartSceneBgmChanger : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private BgmManager bgmManager;
    private void Update()
    {
        if (obj.gameObject.activeSelf == false)
        {
            bgmManager.BgmSet = true;
        }
    }
}
