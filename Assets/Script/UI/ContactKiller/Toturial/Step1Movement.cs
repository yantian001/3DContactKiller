using UnityEngine;
using System.Collections;

public class Step1Movement : MonoBehaviour {

    public RectTransform rect;
    public Vector2 to;
    public float t;
    public LeanTweenType type;
	// Use this for initialization
	void Start () {
       // Time.timeScale = 0;
        LeanTween.move(rect, to, t).setEase(type).setLoopPingPong();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
