using UnityEngine;
using System.Collections;

public class TweenAlpha : MonoBehaviour
{
    public float to = 0f;
    public float duration = 1f;

    public void OnEnable()
    {
        LeanTween.alphaText(this.GetComponent<RectTransform>(), to, duration).setLoopPingPong();
    }
}
