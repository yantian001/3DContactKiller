using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HoldBreath : MonoBehaviour
{

    public Button btn;
    public Image fillImage;
    /// <summary>
    /// 屏息时间
    /// </summary>
    public float holdBreathDuration = 10f;

    bool holdBreathClicked = false;
    //Coroutine coroutine = null;
    // Use this for initialization
    float timeLeft = 0;
    void Start()
    {
        if (btn == null)
        {
            btn = GetComponent<Button>();
        }
        if (!fillImage)
        {
            fillImage = CommonUtils.GetChildComponent<Image>(this.GetComponent<RectTransform>(), "Fill");
        }
        fillImage.fillAmount = 1;
        btn.onClick.AddListener(BtnClicked);
    }

    public void OnEnable()
    {
        //cor
        if(holdBreathClicked && timeLeft > 0)
        {
            StartCoroutine(HoldBreathRoute());
        }
    }

    public void OnDisable()
    {

    }

    private void BtnClicked()
    {
        btn.interactable = false;
        timeLeft = holdBreathDuration;
        StartCoroutine(HoldBreathRoute());
        holdBreathClicked = true;
        //throw new NotImplementedException();
    }

    private IEnumerator HoldBreathRoute()
    {
        //throw new NotImplementedException();
        GunHanddle.Instance.HoldBreath();
        //float c = holdBreathDuration;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            fillImage.fillAmount = timeLeft / holdBreathDuration;
        }
        GunHanddle.Instance.RecoveryBreath();
    }
}
