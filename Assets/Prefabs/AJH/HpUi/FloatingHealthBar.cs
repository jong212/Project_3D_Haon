using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    Transform target;
    [SerializeField] private Slider slider;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    public void SetTarget(Transform transform)
    {
        target = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
    private void LateUpdate()

    {
        if (target != null)
        {
            // 캐릭터의 월드 좌표를 가져옴
            Vector3 worldPos = target.position;

            // 카메라의 월드 좌표를 기준으로 캐릭터의 스크린 좌표를 계산
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // 스크린 좌표를 기준으로 HP 바의 위치를 설정
            transform.position = screenPos + Vector3.up * 100;

            // HP 바의 너비를 설정 (옵션)
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, rect.sizeDelta.y);
        }
    }*/
    private void LateUpdate()
    {
        if (target != null)
        {
            // 캐릭터의 월드 좌표를 가져옴
            Vector3 worldPos = target.position;

            // 캐릭터의 월드 좌표에서 상대적인 높이를 설정하여 HP 바의 위치를 계산
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos + Vector3.up * 2f);

            // 스크린 좌표를 기준으로 HP 바의 위치를 설정
            transform.position = screenPos + Vector3.up * 0;

            // HP 바의 너비를 설정 (옵션)
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, rect.sizeDelta.y);
        }
    }


}
