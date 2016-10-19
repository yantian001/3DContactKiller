using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SniperSlider : MonoBehaviour
{

    public Slider slider;

    // Use this for initialization
    void Start()
    {
        if (!slider)
            slider = GetComponent<Slider>();
        slider.value = 0;
        slider.onValueChanged.AddListener(OnSlider);
    }

    private void OnSlider(float f)
    {
        //throw new NotImplementedException();
        GunHanddle.Instance.ZoomAdjust(f);
    }
    
}
