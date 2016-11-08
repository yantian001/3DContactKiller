﻿using UnityEngine;
using System.Collections;
using System;

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
        currentCount = 0;
        currentScores = 0;
        if (IsTimeLimit)
        {
            timeLeft = LimitTime;
        }
        AddEventListener();
    }

    public void AddEventListener()
    {
        if (_type == MissionType.Target)
        {
            LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
        }
    }

    public void RemoveEventListener()
    {
        if (_type == MissionType.Target)
        {
            LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
        }
    }

    private void OnEnemyDie(LTEvent obj)
    {
        if (_statu == MissionStatu.Running)
        {
            var edi = obj.data as EnemyDeadInfo;
            if (edi != null)
            {
                if (edi.animal.Id == Target.Id)
                {
                    if (NeedHeadShot)
                    {
                        if (edi.headShot)
                        {
                            currentCount += 1;
                        }
                    }
                    else
                        currentCount += 1;
                }
            }
        }
    }

    /// <summary>
    /// 类似于每帧执行
    /// </summary>
    /// <param name="delta"></param>
    public virtual void OnUpdate(float delta)
    {
        if (IsMissionRunning())
        {
            if (IsTimeLimit)
            {
                timeLeft -= delta;
            }
            if (_type == MissionType.Target)
            {
                if (currentCount >= TargetCount)
                    _statu = MissionStatu.Completed;
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
                TimeUpdate();
            }
        }
    }

    void TimeUpdate()
    {
        if (IsMissionRunning())
        {
            if (timeLeft <= 0)
            {
                _statu = MissionStatu.Failed;
            }
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


    public void OnCheckFailure(Animal[] animals)
    {
        if (_statu == MissionStatu.Running && IsLimitTarget)
        {
            int count = 0;
            for (int i = 0; i < animals.Length; i++)
            {
                if (animals[i] != null)
                {
                    if (animals[i].Id == Target.Id)
                        count += 1;
                }
            }
            if (count < TargetCount - currentCount)
            {
                _statu = MissionStatu.Failed;
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
            return "Kill targets in " + LimitTime.ToString() + "s";
        }
        return str;
        // return "";
    }

    public IMission Clone()
    {
        //throw new NotImplementedException();
        IMission m = new IMission();
        m.currentCount = this.currentCount;
        m.currentScores = this.currentScores;
        m.IsLimitTarget = this.IsLimitTarget;
        m.IsTimeLimit = this.IsTimeLimit;
        m.LimitTime = this.LimitTime;
        m.NeedHeadShot = this.NeedHeadShot;
        m.Target = this.Target;
        m.TargetCount = this.TargetCount;
        m.TargetScores = this.TargetScores;
        m.timeLeft = this.timeLeft;
        m._statu = this._statu;
        m._type = this._type;
        return m;
    }

    public string GetTimeString()
    {
        int m = (int)timeLeft / 60;
        int s = (int)timeLeft % 60;
        string str =
            string.Format("{0:00}:{1:00}", m, s);
        return str;
    }

    /// <summary>
    /// 获取头像
    /// </summary>
    /// <returns></returns>
    public Texture2D GetPhotoTexture()
    {
        Texture2D t = null;
        if (IsLimitTarget && Target)
        {
            t = Target.Avater;
        }
        return t;
    }
}
