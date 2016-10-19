using UnityEngine;
using System.Collections;

public class SniperScreen : MonoBehaviour {

    public GameObject screen;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
	    if(GunHanddle.Instance.Zoomed())
        {
            screen.SetActive(true);
        }
        else
        {
            screen.SetActive(false);
        }
    }
}
