using UnityEngine;
using System.Collections;


public enum MissionStatu
{
    None,
    Running,
    Completed,
    Failed
}

public enum MissionType
{
    None,
    /// <summary>
    /// 目标任务
    /// </summary>
    Target,
    /// <summary>
    /// 时间任务
    /// </summary>
    Time,
    /// <summary>
    /// 积分任务
    /// </summary>
    Score,
    /// <summary>
    /// 连杀任务
    /// </summary>
    Combo,
    /// <summary>
    /// 警报
    /// </summary>
    Alarm,
    /// <summary>
    /// 击杀正在警报的敌人
    /// </summary>
    AlarmKill,
    /// <summary>
    /// 制造意外击杀
    /// </summary>
    AccidentKill,
}

[System.Serializable]
public class IMission
{
    public MissionStatu _statu = MissionStatu.None;
    public MissionType _type = MissionType.None;

    #region 公用
    /// <summary>
    /// 是否限制时间
    /// </summary>
    public bool IsTimeLimit = false;
    /// <summary>
    /// 时间
    /// </summary>
    public float LimitTime = 0f;
    /// <summary>
    /// 是否限制目标对象
    /// </summary>
    public bool IsLimitTarget = false;
    #endregion

    #region 目标击杀任务
    public Animal Target = null;
    public int TargetCount = 0;
    public bool NeedHeadShot = false;
    #endregion

    #region 积分任务
    public float TargetScores = 0;
    #endregion


    public int currentCount = 0;
    public float currentScores = 0;
    public float timeLeft = 0;
    /// <summary>
    /// 初始化方法
    /// </summary>
    public virtual void OnInit()
    {
        _statu = MissionStatu.Running;
        if (IsTimeLimit)
        {
            timeLeft = LimitTime;
        }
    }
    /// <summary>
    /// 类似于每帧执行
    /// </summary>
    /// <param name="delta"></param>
    public virtual void OnUpdate(float delta)
    {
        if (IsTimeLimit)
        {
            timeLeft -= delta;
        }
        if (_type == MissionType.Target)
        {

        }
        else if (_type == MissionType.Alarm)
        {

        }
        else if (_type == MissionType.AlarmKill)
        {

        }
        else if (_type == MissionType.AccidentKill)
        {

        }
        else if (_type == MissionType.Combo)
        {
            ScoreUpdate();
        }
        else if (_type == MissionType.Score)
        {
            ScoreUpdate();
        }
        else if (_type == MissionType.Time)
        {

        }
    }

    void ScoreUpdate()
    {
        if (IsMissionRunning())
        {
            if (ScoresManager.instance != null)
            {
                currentScores = ScoresManager.instance.currentScore;
                if (currentScores >= TargetScores)
                {
                    _statu = MissionStatu.Completed;
                }
                else
                {
                    if (IsTimeLimit)
                    {
                        if (timeLeft <= 0)
                        {
                            _statu = MissionStatu.Failed;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 销毁方法
    /// </summary>
    public virtual void OnDestory()
    {
        _statu = MissionStatu.None;
    }
    /// <summary>
    /// 任务是否完成
    /// </summary>
    /// <returns></returns>
    public virtual bool IsMissionComplete()
    {
        return _statu == MissionStatu.Completed;
    }
    /// <summary>
    /// 任务是否失败
    /// </summary>
    /// <returns></returns>
    public virtual bool IsMissionFailed()
    {
        return _statu == MissionStatu.Failed;
    }
    /// <summary>
    /// 任务是否在进行中
    /// </summary>
    /// <returns></returns>
    public virtual bool IsMissionRunning()
    {
        //if(_statu == MissionStatu.Running)
        return _statu == MissionStatu.Running;
    }

    public virtual string GetDescription()
    {
        string str = "";
        if (_type == MissionType.Target)
        {
            str = string.Format("Kill {0}/{1} {2}", currentCount, TargetCount, Target.Name);
            if (NeedHeadShot)
            {
                str += " with headshot";
            }
        }
        else if (_type == MissionType.Alarm)
        {

        }
        else if (_type == MissionType.AlarmKill)
        {

        }
        else if (_type == MissionType.AccidentKill)
        {

        }
        else if (_type == MissionType.Combo)
        {
            return string.Format("Continue kill {0} enemy in {1}s", TargetCount, LimitTime);
        }
        else if (_type == MissionType.Score)
        {
            str = string.Format("Get {0}/{1} point", currentScores, TargetScores);
            if (IsTimeLimit)
            {
                str += string.Format(" in {0}s", LimitTime);
            }
        }
        else if (_type == MissionType.Time)
        {
            return "Kill more than 10 enemy with 10s and 10000 points";
        }
        return str;
        // return "";
    }

    public string GetTimeString()
    {
        int m = (int)timeLeft / 60;
        int s = (int)timeLeft % 60;
        string str =
            string.Format("{0:00}:{1:00}", m, s);
        return str;
    }
}
