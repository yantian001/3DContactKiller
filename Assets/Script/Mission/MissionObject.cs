using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MissionObject
{
    //public float level;
    /// <summary>
    /// 总游戏时长
    /// </summary>
    public float totalTime = 60;
    /// <summary>
    /// 奖励
    /// </summary>
    public float reward = 0f;


    [Tooltip(" 武器火力要求")]
    public float powerRequired = -1f;
    /// <summary>
    /// 武器最大视距要求
    /// </summary>
    [Tooltip("武器最大视距要求")]
    public float maxZoomRequired = -1f;
    /// <summary>
    /// 武器稳定性要求
    /// </summary>
    [Tooltip("武器稳定性要求")]
    public float stabilityRequired = -1f;
    /// <summary>
    /// 武器弹夹数量要求
    /// </summary>
    [Tooltip("武器弹夹数量要求")]
    public float capacityRequired = -1f;

    [SerializeField]
    public List<IMission> missions = new List<IMission>();
    /// <summary>
    /// 任务是否已完成
    /// </summary>
    public bool finished = false;

    #region 运行时变量
    /// <summary>
    /// 任务转台
    /// </summary>
    public MissionStatu _statu = MissionStatu.None;



    #endregion

    public void OnInit()
    {
        _statu = MissionStatu.Running;
        if (missions.Count > 0)
        {
            for (int i = 0; i < missions.Count; i++)
            {
                missions[i].OnInit();
            }
        }
        else
        {
            _statu = MissionStatu.Completed;
        }
    }

    public void OnUpdate(float delta)
    {
        bool allSubCompleted = true;
        if (_statu == MissionStatu.Running)
        {
            for (int i = 0; i < missions.Count; i++)
            {
                missions[i].OnUpdate(delta);
                allSubCompleted = allSubCompleted && missions[i].IsMissionComplete();
                if (missions[i].IsMissionFailed())
                {
                    _statu = MissionStatu.Failed;
                }
            }
            if (allSubCompleted)
                _statu = MissionStatu.Completed;
        }
    }

    public void OnDestory()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            missions[i].OnDestory();
        }
    }

    #region Editor
#if UNITY_EDITOR
    //#if UnityEditor
    /// <summary>
    /// 是否展开
    /// </summary>
    public bool minimise = false;
    //#endif
    /// <summary>
    /// 新增任务
    /// </summary>
    /// <param name="type"></param>
    public void AddMission(MissionType type)
    {
        IMission newMission = null;
        switch (type)
        {
            case MissionType.Score:
                newMission = new ScoresMission();
                break;
            case MissionType.Target:
                newMission = new TargetMission();
                break;
            case MissionType.Combo:
                newMission = new ComboMission();
                break;
            case MissionType.Alarm:
                newMission = new AlarmMission();
                break;
            case MissionType.AlarmKill:
                newMission = new AlarmKillMission();
                break;
            case MissionType.AccidentKill:
                newMission = new AccidentKillMission();
                break;
            case MissionType.Time:
                newMission = new TimeMission();
                break;
        }
        if (newMission != null)
            missions.Add(newMission);
    }

    public void DeleteMission(int i)
    {
        missions.RemoveAt(i);
    }
#endif
    #endregion
}
