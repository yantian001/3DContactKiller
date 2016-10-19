using UnityEngine;
using System.Collections;

public class IngameControl : MonoBehaviour
{

    public GameObject Screen;
    public GameObject btnHoldBreath;

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
