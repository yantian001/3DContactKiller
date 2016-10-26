using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClipText : MonoBehaviour
{

    public Text clipText;
    // Use this for initialization
    void Start()
    {
        if (!clipText)
        {
            clipText = GetComponent<Text>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(clipText)
        {
            clipText.text = GunHanddle.GetGunClip().ToString();
        }
    }
}
