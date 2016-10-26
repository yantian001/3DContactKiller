using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SniperText : MonoBehaviour
{

    public Text sniperText;
    // Use this for initialization
    void Start()
    {
        if (!sniperText)
        {
            sniperText = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sniperText)
        {
            sniperText.text = string.Format("x {0:##0.0}", GunHanddle.GetZoom());
        }
    }
}
