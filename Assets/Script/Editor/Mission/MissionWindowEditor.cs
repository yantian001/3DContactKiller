using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

public class MissionWindowEditor : EditorWindow
{

    private static MissionWindowEditor window;

    private static GUIContent cont;
    protected static float spaceX = 110;
    protected static float spaceY = 20;
    protected static float width = 150;
    protected static float height = 18;
    protected static float missionHeight = 300;

    public static void Init()
    {
        window = (MissionWindowEditor)EditorWindow.GetWindow(typeof(MissionWindowEditor));
        EditorDBManager.Init();
    }
    private static string newChapterName;
    private static int selectId = 0;
    private Vector2 scrollPos1;

    public bool minimiseCommonMission = false;
    void OnGUI()
    {
        if (window == null) Init();
        List<Chapter> chapterList = EditorDBManager.GetChapterList();
        //List<MissionObject> missionList = EditorDBManager.GetMissionList();
        if (GUI.Button(new Rect(window.position.width - 120, 5, 100, 25), "Save")) EditorDBManager.SetDirtyMission();
        //EditorGUI.LabelField(new Rect(5, 7, 150, 17), chapterList.Count.ToString());
        newChapterName = EditorGUI.TextField(new Rect(5, 7, 150, 25), newChapterName);
        if (GUI.Button(new Rect(160, 7, 100, 25), "Add")) EditorDBManager.AddNewChapter(newChapterName);

        float startX = 5;
        float startY = 50;
        Vector2 v2 = DrawChapterList(startX, startY, chapterList);
        startX = v2.x + 25;
        if (chapterList.Count == 0)
            return;
        //绘制第一行标题栏
        cont = new GUIContent("场景名称：", "该游戏场景的名称");
        EditorGUI.LabelField(new Rect(startX, startY, width, height), cont);
        chapterList[selectId].Name = EditorGUI.TextField(new Rect(startX + 90, startY, width, height), chapterList[selectId].Name);

        cont = new GUIContent("游戏场景", "该游戏章节使用的游戏场景");
        EditorGUI.LabelField(new Rect(startX + 295, startY, width, height), cont);
        //SceneAsset sa;
        chapterList[selectId].Scene = (SceneAsset)EditorGUI.ObjectField(new Rect(startX + 380, startY, width, height), chapterList[selectId].Scene, typeof(SceneAsset), false);

        //绘制普通任务
        startY += spaceY + 10;
        v2 = DrawCommonMission(startX, startY, chapterList[selectId].CommonMission);


        if (GUI.changed)
            EditorDBManager.SetDirtyMission();
    }

    private Rect commonMissionVisiableRect, commonMissionContentRect;
    private Vector2 scrollPosCommonMission;
    /// <summary>
    /// 绘制普通任务
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="missionList"></param>
    /// <returns></returns>
    Vector2 DrawCommonMission(float startX, float startY, List<MissionObject> missionList)
    {
        //绘制普通任务
        minimiseCommonMission = EditorGUI.Foldout(new Rect(startX, startY, width, height), minimiseCommonMission, "普通任务");

        if (GUI.Button(new Rect(startX + width, startY, 100, 18), "+1")) { EditorDBManager.AddNewCommonMission(selectId); };
        if (GUI.Button(new Rect(startX + width + 110, startY, 100, 18), "折叠"))
        {
            for (int i = 0; i < missionList.Count; i++)
            {
                missionList[i].minimise = false;
            }
        };
        if (GUI.Button(new Rect(startX + width + 220, startY, 100, 18), "展开"))
        {
            for (int i = 0; i < missionList.Count; i++)
            {
                missionList[i].minimise = true;
            }
        };
        startY += spaceY + 5;
        if (missionList.Count != 0)
        {
            if (minimiseCommonMission)
            {
                float unitheight = 600;
                //每一个任务占用的高度
                commonMissionVisiableRect = new Rect(startX, startY, window.position.width - startX - 20, unitheight);
                float contentH = 0;
                for (int i = 0; i < missionList.Count; i++)
                {
                    if (!missionList[i].minimise) contentH += spaceY;
                    else contentH += missionHeight;
                    contentH += 10;
                }
                commonMissionContentRect = new Rect(startX, startY, window.position.width - startX - 20, contentH);

                GUI.color = new Color(.8f, .8f, .8f, 1f);

                GUI.Box(commonMissionVisiableRect, "");
                GUI.color = Color.white;

                scrollPosCommonMission = GUI.BeginScrollView(commonMissionVisiableRect, scrollPosCommonMission, commonMissionContentRect);
                Vector2 v2 = new Vector2(startX, startY);
                for (int i = 0; i < missionList.Count; i++)
                {
                    v2 = DrawMission(v2, missionList[i], i,
                        () => { missionList.RemoveAt(i); },
                        () =>
                        {
                            if (i > 0)
                            {
                                //up
                                MissionObject o = missionList[i];
                                missionList[i] = missionList[i - 1];
                                missionList[i - 1] = o;
                            }
                        },
                        () =>
                        {
                            //down
                            if (i < missionList.Count - 1)
                            {
                                MissionObject o = missionList[i];
                                missionList[i] = missionList[i + 1];
                                missionList[i + 1] = o;
                            }
                        });
                }
                GUI.EndScrollView();
                startY += unitheight + spaceY + 5;
            }
        }
        return new Vector2(startX, startY);
    }


    protected MissionType newMissionType = MissionType.None;
    protected Rect missionConditionVisableRect, missionConditionContentRect;
    Vector2 mcScorllPosition;
    Vector2 DrawMission(Vector2 v2, MissionObject m, int index, Action deleteAction, Action upAction, Action downAction)
    {
        float tempX = v2.x + 10;
        float tempY = v2.y;
        m.minimise = EditorGUI.Foldout(new Rect(tempX, tempY, width, height), m.minimise, (index + 1).ToString());

        if (GUI.Button(new Rect(window.position.width - width, tempY + 2, 20, 17), "x")) { if (deleteAction != null) deleteAction.Invoke(); };
        if (GUI.Button(new Rect(window.position.width - width + 25, tempY + 2, 20, 17), "↑"))
        {
            Debug.Log("↑ Click");
            upAction();
        }
        if (GUI.Button(new Rect(window.position.width - width + 50, tempY + 2, 20, 17), "↓"))
        {
            Debug.Log("↓ Click");
            downAction();
        }
        //v2.y += spaceY + 10;
        tempY += spaceY;
        if (m.minimise)
        {
            tempX += 10;
            GUI.Label(new Rect(tempX, tempY, 60, height), "时间:");
            m.totalTime = EditorGUI.FloatField(new Rect(tempX + 100, tempY, width, height), m.totalTime);
            tempY += spaceY;
            GUI.Label(new Rect(tempX, tempY, 60, height), "奖励:");
            m.reward = EditorGUI.FloatField(new Rect(tempX + 100, tempY, width, height), m.reward);
            tempY += spaceY;
            GUI.Label(new Rect(tempX, tempY, width, height), "任务条件:");
            tempY += spaceY;
            tempX += 10;
            GUI.Label(new Rect(tempX, tempY, 60, height), "条件类型:");
            newMissionType = (MissionType)EditorGUI.EnumPopup(new Rect(tempX + 100, tempY, width, height), newMissionType);
            if (GUI.Button(new Rect(tempX + 100 + width, tempY, 20, 17), "+"))
            {
                m.AddMission(newMissionType);
                EditorDBManager.SetDirtyMission();
            };
            tempY += spaceY;
            Vector2 vt = new Vector2(tempX + 5, tempY);
            //绘制条件集合
            if (m.missions.Count > 0)
            {
                //GUI.color=new Color(0.6f, 0.6f, 0.6f, 1);
                //GUI.Box(new Rect(tempX, tempY, width * 2, 200), "");
                //GUI.color = Color.white;
                missionConditionVisableRect = new Rect(tempX, tempY, window.position.width - tempX, 170);
                missionConditionContentRect = new Rect(tempX, tempY, m.missions.Count * 230, 170);
                mcScorllPosition = GUI.BeginScrollView(missionConditionVisableRect, mcScorllPosition, missionConditionContentRect);
                for (int i = 0; i < m.missions.Count; i++)
                {
                    vt = DrawMissionCondition(vt, m.missions[i], () => { m.DeleteMission(i); });
                }
                GUI.EndScrollView();
            }
            tempY += 180;
            tempX -= 10;
            //武器属性要求
            GUI.Label(new Rect(tempX, tempY, width, height), "武器要求:");
            tempX += 10;
            tempY += spaceY;
            GUI.Label(new Rect(tempX, tempY, 40, height), "火力:");
            m.powerRequired = EditorGUI.FloatField(new Rect(tempX + 50, tempY, 40, height), m.powerRequired);
            GUI.Label(new Rect(tempX + 100, tempY, 40, height), new GUIContent("倍数:", "武器的狙击镜最大放大倍数要求"));
            m.maxZoomRequired = EditorGUI.FloatField(new Rect(tempX + 150, tempY, 40, height), m.maxZoomRequired);
            GUI.Label(new Rect(tempX + 200, tempY, 40, height), "稳定性:");
            m.stabilityRequired = EditorGUI.FloatField(new Rect(tempX + 250, tempY, 40, height), m.stabilityRequired);
            GUI.Label(new Rect(tempX + 300, tempY, 40, height), new GUIContent("弹夹:", "武器弹夹数量要求"));
            m.capacityRequired = EditorGUI.FloatField(new Rect(tempX + 350, tempY, 40, height), m.capacityRequired);
            v2.y += missionHeight + 10;
        }
        else
        {
            v2.y += spaceY + 10;
        }
        return v2;
    }

    Vector2 DrawMissionCondition(Vector2 v2, IMission im, Action deleteAction)
    {
        GUI.color = new Color(0.6f, 0.6f, 0.6f, 1);
        GUI.Box(new Rect(v2.x, v2.y, 220, 150), "");
        GUI.color = Color.white;
        float tempx = v2.x;
        float tempy = v2.y;
        tempy += 5;
        if (GUI.Button(new Rect(tempx + 220 - 30, tempy, 20, 17), "x")) { if (deleteAction != null) deleteAction.Invoke(); };

        switch (im._type)
        {
            case MissionType.Target:
                //TargetMission tm = im as TargetMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), "目标任务");
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), "目标:");
                im.Target = (Animal)EditorGUI.ObjectField(new Rect(tempx + 50, tempy, 130, height), im.Target, typeof(Animal), false);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 40, height), "数量:");
                im.TargetCount = EditorGUI.IntField(new Rect(tempx + 50, tempy, 110, height), im.TargetCount);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 40, height), new GUIContent("爆头:", "是否需要爆头击杀"));
                im.NeedHeadShot = EditorGUI.Toggle(new Rect(tempx + 50, tempy, 30, height), im.NeedHeadShot);
                //绘制头像
                EditorGUI.DrawRect(new Rect(tempx + 100, tempy, 100, 80), Color.gray);
                GUI.Label(new Rect(tempx + 100 + 50 - 15, tempy + 40 - height / 2, 30, height), "头像");
                if (im.Target && im.Target.Avater)
                {
                    GUI.DrawTexture(new Rect(tempx + 100, tempy, 100, 80), im.Target.Avater, ScaleMode.ScaleToFit);
                }
                break;
            case MissionType.Score:
               // ScoresMission sm = im as ScoresMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), "积分任务");
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), "目标:");
                im.TargetScores = EditorGUI.FloatField(new Rect(tempx + 50, tempy, 130, height), im.TargetScores);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 40, height), new GUIContent("限时:", "是否需要在规定时间内达到分数"));
                im.IsTimeLimit = EditorGUI.Toggle(new Rect(tempx + 50, tempy, 30, height), im.IsTimeLimit);
                tempy += spaceY;
                if (im.IsTimeLimit)
                {
                    GUI.Label(new Rect(tempx + 10, tempy, 40, height), "时间:");
                    im.LimitTime = EditorGUI.FloatField(new Rect(tempx + 50 + 10, tempy, 110, height), im.LimitTime);
                }
                break;
            case MissionType.Combo:
                //开枪后多少秒内连续击杀目标
               // ComboMission cm = im as ComboMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), "连杀任务");
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), new GUIContent("时间:", "开枪后多少秒内"));
                im.LimitTime = EditorGUI.FloatField(new Rect(tempx + 50, tempy, 40, height), im.LimitTime);
                GUI.Label(new Rect(tempx + 100, tempy, 40, height), new GUIContent("s,击杀", "击杀敌人的人数"));
                im.TargetCount = EditorGUI.IntField(new Rect(tempx + 150, tempy, 40, height), im.TargetCount);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 60, height), "限定目标:");
                im.IsLimitTarget = EditorGUI.Toggle(new Rect(tempx + 70, tempy, 30, height), im.IsLimitTarget);
                tempy += spaceY;
                if (im.IsLimitTarget)
                {
                    GUI.Label(new Rect(tempx, tempy, 40, height), "目标:");

                    im.Target = (Animal)EditorGUI.ObjectField(new Rect(tempx + 10, tempy + spaceY, 80, height), im.Target, typeof(Animal), false);
                    //绘制头像
                    EditorGUI.DrawRect(new Rect(tempx + 100, tempy, 100, 80), Color.gray);
                    GUI.Label(new Rect(tempx + 100 + 50 - 15, tempy + 40 - height / 2, 30, height), "头像");
                    if (im.Target && im.Target.Avater)
                    {
                        GUI.DrawTexture(new Rect(tempx + 100, tempy, 100, 80), im.Target.Avater, ScaleMode.ScaleToFit);
                    }
                }
                break;
            case MissionType.Alarm:
                //AlarmMission am = im as AlarmMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), "警报触发");
                tempy += spaceY;
                tempx += 10;
                GUI.TextArea(new Rect(tempx, tempy, 180, 100), "在不触发警报的情况下完成任务");
                break;
            case MissionType.Time:
               // TimeMission tim = im as TimeMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), new GUIContent("限时任务", "在指定时间内完成所有任务"));
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), "时间:");
                im.LimitTime = EditorGUI.FloatField(new Rect(tempx + 50, tempy, 80, height), im.LimitTime);
                GUI.Label(new Rect(tempx + 130, tempy, 40, height), "秒");
                tempy += spaceY;
                GUI.TextArea(new Rect(tempx, tempy, 180, 100), "在指定时间内完成所有任务!");
                break;
            case MissionType.AlarmKill:
               // AlarmKillMission akm = im as AlarmKillMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), new GUIContent("警报击杀", "击杀指定数量处于警报状态下的敌人"));
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), "数量:");
                im.TargetCount = EditorGUI.IntField(new Rect(tempx + 50, tempy, 130, height), im.TargetCount);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 60, height), "限定目标:");
                im.IsLimitTarget = EditorGUI.Toggle(new Rect(tempx + 70, tempy, 30, height), im.IsLimitTarget);
                tempy += spaceY;
                if (im.IsLimitTarget)
                {
                    GUI.Label(new Rect(tempx, tempy, 40, height), "目标:");

                    im.Target = (Animal)EditorGUI.ObjectField(new Rect(tempx + 10, tempy + spaceY, 80, height), im.Target, typeof(Animal), false);
                    //绘制头像
                    EditorGUI.DrawRect(new Rect(tempx + 100, tempy, 100, 80), Color.gray);
                    GUI.Label(new Rect(tempx + 100 + 50 - 15, tempy + 40 - height / 2, 30, height), "头像");
                    if (im.Target && im.Target.Avater)
                    {
                        GUI.DrawTexture(new Rect(tempx + 100, tempy, 100, 80), im.Target.Avater, ScaleMode.ScaleToFit);
                    }
                }
                break;

            case MissionType.AccidentKill:
               // AccidentKillMission acm = im as AccidentKillMission;
                GUI.Label(new Rect(tempx, tempy, 80, height), new GUIContent("意外击杀", "攻击爆炸体引发意外击杀"));
                tempy += spaceY;
                tempx += 10;
                GUI.Label(new Rect(tempx, tempy, 40, height), "数量:");
                im.TargetCount = EditorGUI.IntField(new Rect(tempx + 50, tempy, 130, height), im.TargetCount);
                tempy += spaceY;
                GUI.Label(new Rect(tempx, tempy, 60, height), "限定目标:");
                im.IsLimitTarget = EditorGUI.Toggle(new Rect(tempx + 70, tempy, 30, height), im.IsLimitTarget);
                tempy += spaceY;
                if (im.IsLimitTarget)
                {
                    GUI.Label(new Rect(tempx, tempy, 40, height), "目标:");

                    im.Target = (Animal)EditorGUI.ObjectField(new Rect(tempx + 10, tempy + spaceY, 80, height), im.Target, typeof(Animal), false);
                    //绘制头像
                    EditorGUI.DrawRect(new Rect(tempx + 100, tempy, 100, 80), Color.gray);
                    GUI.Label(new Rect(tempx + 100 + 50 - 15, tempy + 40 - height / 2, 30, height), "头像");
                    if (im.Target && im.Target.Avater)
                    {
                        GUI.DrawTexture(new Rect(tempx + 100, tempy, 100, 80), im.Target.Avater, ScaleMode.ScaleToFit);
                    }
                }
                break;
        }
        v2.x += 220 + 10;
        return v2;
    }

    private Rect listVisibleRect, listContentRect;
    private int deleteID = -1;
    Vector2 DrawChapterList(float x, float y, List<Chapter> chapterList)
    {
        float width = 260;
        listVisibleRect = new Rect(x, y, width + 15, window.position.height - y - 5);
        listContentRect = new Rect(x, y, width, chapterList.Count * 35 + 5);
        GUI.color = new Color(.8f, .8f, .8f, 1f);
        GUI.Box(listVisibleRect, "");
        GUI.color = Color.white;

        scrollPos1 = GUI.BeginScrollView(listVisibleRect, scrollPos1, listContentRect);

        x += 5; y += 5;
        for (int i = 0; i < chapterList.Count; i++)
        {
            if (selectId == i) GUI.color = new Color(0, 1, 1, 1);
            if (GUI.Button(new Rect(x + 35, y + i * 35, 150, 30), chapterList[i].Name)) SelectChapter(i);
            GUI.color = Color.white;
            if (deleteID == i)
            {
                if (GUI.Button(new Rect(x + 190, y + (i * 35), 60, 15), "cancel")) deleteID = -1;

                GUI.color = Color.red;
                if (GUI.Button(new Rect(x + 190, y + (i * 35) + 15, 60, 15), "confirm"))
                {
                    if (selectId >= deleteID) SelectChapter(Mathf.Max(0, selectId - 1));
                    EditorDBManager.RemoveChapter(deleteID);
                    deleteID = -1;
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUI.Button(new Rect(x + 190, y + (i * 35), 60, 30), "remove")) deleteID = i;
            }
        }
        GUI.EndScrollView();
        return new Vector2(x + width, y);
    }

    private void SelectChapter(int i)
    {
        // throw new NotImplementedException();
        selectId = i;
        GUI.FocusControl("");
    }
}
