using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMission : MonoBehaviour
{

    public RectTransform missionItemWithTime;
    public float missionItemHeight = 20f;

    public float missionItemWithTimeHeight = 40f;
    public RectTransform missionAvater;
    public float missionAvaterWidth = 60;


    public RectTransform avatarZone;
    public RectTransform listZone;

    MissionObject mission;

    // Use this for initialization
    void Start()
    {
        //mission = EditorDBManager.
        mission = MissionManager.Instance.lstChapters[0].CommonMission[0];
        DisplayMission(mission.missions);
    }




    public void DisplayMission(List<IMission> ms)
    {


        if (ms.Count > 0)
        {
            //显示的头像的数量
            int msAvatarCount = 0;
            Vector2 v2List = new Vector2(0, 0);
            Vector2 v2Avatar = new Vector2(0, 0);
            for (int i = 0; i < ms.Count; i++)
            {
                //显示任务文本
                RectTransform tmItemRect = RectTransform.Instantiate(missionItemWithTime) as RectTransform;
                tmItemRect.GetComponent<MissionItem>().mission = ms[i];
                tmItemRect.SetParent(listZone);
                tmItemRect.localScale = new Vector3(1, 1, 1);
                tmItemRect.anchoredPosition = v2List;
                if (ms[i]._type == MissionType.Target || ms[i]._type == MissionType.AlarmKill || ms[i]._type == MissionType.Alarm || ms[i]._type == MissionType.AccidentKill)
                {

                    v2List.y -= missionItemHeight;
                }
                else
                {
                    v2List.y -= missionItemWithTimeHeight;
                }
                //显示任务头像
                if (ms[i].Target && ms[i].Target.Avater != null)
                {
                    RectTransform rtAvatar = (RectTransform)RectTransform.Instantiate(missionAvater);
                    rtAvatar.GetComponent<AvatarItem>().mission = ms[i];
                    rtAvatar.SetParent(avatarZone);
                    rtAvatar.anchoredPosition = v2Avatar;
                    rtAvatar.localScale = Vector3.one;
                    msAvatarCount += 1;
                    v2Avatar.x += missionAvaterWidth;
                }
            }
            if (msAvatarCount <= 0)
            {
                listZone.anchoredPosition = new Vector2(0, -6);
            }
            else
            {
                listZone.anchoredPosition = new Vector2(0, -66);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DisplayMission(mission.missions);
    }
}
