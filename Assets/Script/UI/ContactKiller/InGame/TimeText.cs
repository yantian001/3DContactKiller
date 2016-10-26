using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{

    public Text timeText;
    // Use this for initialization
    void Start()
    {
        if (!timeText)
        {
            timeText = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeText)
        {
            float time = MissionControl.Instance.GetTime();
            int m = (int)time / 60;
            int s = (int)time % 60;
            string str =
                string.Format("{0:00}:{1:00}", m, s);
            timeText.text = str;
        }
    }
}
