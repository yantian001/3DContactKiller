using UnityEngine;
using System.Collections;

public class WatchVedio : MonoBehaviour
{
    public RectTransform parent;
    public RectTransform before;
    public RectTransform noads;
    public RectTransform complete;
    Vector2 tempPos;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.COINADD, OnCoinAdd);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.COINADD, OnCoinAdd);

    }


    // Use this for initialization
    void Start()
    {
        tempPos = parent.anchoredPosition;
        Display(0);
    }

    void OnCoinAdd(LTEvent evt)
    {
        parent.anchoredPosition = Vector2.zero;
        Display(0);
    }

    public void Close()
    {
        parent.anchoredPosition = tempPos;
    }

    public void OnWatchClicked()
    {
        print("watch clicked!");
        FUGSDK.RewardVedioClosedEvent evt = OnWatchCallBack;
        LeanTween.dispatchEvent((int)Events.WATCHVEDIO, evt);
    }

    void OnWatchCallBack(bool b)
    {
        if (b)
        {
            Display(2);
            LeanTween.dispatchEvent((int)Events.MONEYUSED, -500);
        }
        else
        {
            Display(1);
        }
    }

    void Display(int index)
    {
        if (index == 0)
        {
            before.gameObject.SetActive(true);
            noads.gameObject.SetActive(false);
            complete.gameObject.SetActive(false);
        }
        else
        {
            if (index == 1)
            {
                before.gameObject.SetActive(false);
                noads.gameObject.SetActive(true);
                complete.gameObject.SetActive(false);
            }
            else if (index == 2)
            {
                before.gameObject.SetActive(false);
                noads.gameObject.SetActive(false);
                complete.gameObject.SetActive(true);
            }
        }
    }


}
