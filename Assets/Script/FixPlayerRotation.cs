using UnityEngine;
using System.Collections;

public class FixPlayerRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(FixDirection());
	}
	
    IEnumerator FixDirection()
    {
        yield return new WaitForSeconds(.1f);
        transform.rotation = Quaternion.Euler(0, 180, 0); ;
    }

}
