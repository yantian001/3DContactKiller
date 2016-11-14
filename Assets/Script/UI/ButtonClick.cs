using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{

    public Events EventId;

    public AudioClip clickClip;

    public bool ingoreTimeScale = false;

    Button btn;

    public void OnEnable()
    {
        btn = GetComponent<Button>();
        if (btn)
        {
            btn.onClick.AddListener(OnButtonClick);
        }

        // StartCoroutine(CoroutineEffect());
    }


    //IEnumerator CoroutineEffect()
    //{
    //    if (effectTexture)
    //    {
    //        while (true)
    //        {
    //            yield return new WaitForSeconds(effectInterv);
    //            var effect = Instantiate(effectTexture);
    //            //effect.GetComponent<RectTransform>().SetParent(transform.GetComponent<RectTransform>());
    //          //  effect.GetComponent<ButtonLight>().StartEffect();
    //        }
    //    }

    //}


    void OnButtonClick()
    {
        if (clickClip)
        {
            LeanAudio.play(clickClip);
            //Invoke("DispatchEvent", clickClip.length);
            StartCoroutine(DispatchEventRoute(clickClip.length));
        }
        else
        {
            DispatchEvent();
        }


        //  Debug.Log("button Clicked");
    }

    IEnumerator DispatchEventRoute(float wt)
    {
        if (!ingoreTimeScale)
            yield return new WaitForSeconds(wt);
        //else
        //{
        //    float timeStart = Time.timeSinceLevelLoad;
        //    while (timeStart + wt > Time.timeSinceLevelLoad)
        //    {

        //    }
        //}
        DispatchEvent();
    }

    void DispatchEvent()
    {
        if (EventId != Events.NONE)
        {
            LeanTween.dispatchEvent((int)EventId);
        }
    }

    public void OnDisable()
    {
        if (btn)
        {
            btn.onClick.RemoveListener(OnButtonClick);
        }
    }
}
