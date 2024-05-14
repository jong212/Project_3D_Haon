using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillControl : MonoBehaviour
{
    //비활성화 연출에 사용될 이미지를 타나내거나 숨겨주기 위해 게임오브젝트 만듬
    public GameObject[] hideSkillButtons;

    //TextPro가 처음에는 비활성화 되어 있어서 바로 컴포넌트를 가져올 수 없어서 게임오브젝트를 만들고 textpros작성
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;


    //스킬 사용중인지 
    private bool[] isHideSkills = { false, false, false, false };

    private float[] skillTimers = { 3, 3, 9, 12 };
    private float[] getSkillTimes = { 0, 0, 0, 0, };

    void Start()
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HideSkillChk();
    }
    public void HideSkillSetting(int skillNum)
    {
        hideSkillButtons[skillNum].SetActive(true);
        getSkillTimes[skillNum] = skillTimers[skillNum];
        isHideSkills[skillNum] = true;
    }

    private void HideSkillChk()
    {
        if (isHideSkills[0])
        {
            StartCoroutine(SkillTimeChk(0));
        }
        if (isHideSkills[1])
        {
            StartCoroutine(SkillTimeChk(1));
        }
        if (isHideSkills[2])
        {
            StartCoroutine(SkillTimeChk(2));
        }
        if (isHideSkills[3])
        {
            StartCoroutine(SkillTimeChk(3));
        }
        if (isHideSkills[4])
        {
            StartCoroutine(SkillTimeChk(4));
        }
    }

    IEnumerator SkillTimeChk(int skillNum)
    {
        yield return null;

        if (getSkillTimes[skillNum] == 0)
        {
            getSkillTimes[skillNum] -= Time.deltaTime;
            if (getSkillTimes[skillNum] < 0)
            {
                getSkillTimes[skillNum] = 0;
                isHideSkills[skillNum] = false;
                hideSkillButtons[skillNum].SetActive(false);
            }

            hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");
            float time = getSkillTimes[skillNum] / skillTimers[skillNum];
            hideSkillImages[skillNum].fillAmount = time * Time.deltaTime;
        }
    }
}
