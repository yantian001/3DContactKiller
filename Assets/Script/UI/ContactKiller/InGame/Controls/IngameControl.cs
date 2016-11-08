using UnityEngine;
using System.Collections;
using System;

public class IngameControl : MonoBehaviour
{

    public GameObject Screen;
    public GameObject btnHoldBreath;

    public void Awake()
    {
        LeanTween.addListener((int)Events.TOTURIALED, OnTuto);
        gameObject.SetActive(false);
    }

    private void OnTuto(LTEvent obj)
    {
        // throw new NotImplementedException();
        gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.TOTURIALED, OnTuto);
    }




    // Use this for initialization
    void Start()
    {
        if (!Screen)
        {
            Screen = transform.FindChild("Screen").gameObject;
            btnHoldBreath = transform.FindChild("BtnHold").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GunHanddle.Instance.Zoomed())
        {
            Screen.SetActive(true);
            btnHoldBreath.SetActive(true);
        }
        else
        {
            Screen.SetActive(false);
            btnHoldBreath.SetActive(false);
        }
    }
}
