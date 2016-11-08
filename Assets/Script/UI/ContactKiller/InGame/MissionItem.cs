using UnityEngine;
using System.Collections;

public class MissionItem : MonoBehaviour
{

    public IMission mission;

    RectTransform rt;
    public Color missionTxtColor = Color.white;
    public Color missionCompleteTxtColor = Color.green;
    public Color missionFailTxtColor = Color.red;
    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayMission();
    }

    void DisplayMission()
    {
        if (mission != null)
        {
            CommonUtils.SetToggleOn(rt, mission.IsMissionComplete());
            if (mission.IsMissionComplete())
            {
                CommonUtils.SetChildTextAndColor(rt, "Label", mission.GetDescription(), missionCompleteTxtColor);
            }
            else if (mission.IsMissionFailed())
            {
                CommonUtils.SetChildTextAndColor(rt, "Label", mission.GetDescription(), missionFailTxtColor);
            }
            else
            {
                CommonUtils.SetChildTextAndColor(rt, "Label", mission.GetDescription(), missionTxtColor);
            }

            if ((mission._type == MissionType.Target || mission._type == MissionType.AlarmKill || mission._type == MissionType.Alarm || mission._type == MissionType.AccidentKill))
            {
                CommonUtils.SetChildActive(rt, "Time", false);
            }
            else
            {
                CommonUtils.SetChildActive(rt, "Time", true);
                CommonUtils.SetChildText(rt, "Time", mission.GetTimeString());
                RectTransform rtLabel = CommonUtils.GetChildComponent<RectTransform>(rt, "Label");
                RectTransform rtTime = CommonUtils.GetChildComponent<RectTransform>(rt, "Time");
                if (rtLabel && rtTime)
                {
                    rtTime.anchoredPosition = new Vector2(rtTime.anchoredPosition.x, -(rtLabel.sizeDelta.y - 2));
                }
            }
        }
    }
}
