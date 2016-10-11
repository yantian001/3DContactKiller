using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class ComboMission : IMission
{

    /// <summary>
    /// 在多少秒内杀死特定人数的目标
    /// </summary>
    public float LimitTime = 0f;
    /// <summary>
    /// 连杀数量
    /// </summary>
    public int TargetCount = 0;
    /// <summary>
    /// 是否限定目标
    /// </summary>
    public bool IsLimitTarget = false;
    /// <summary>
    /// 限定的目标
    /// </summary>
    public Animal Target;

    public bool timeStarted = false;

    float timeLeft = 0f;

    public float currentCount = 0;

    public ComboMission()
    {
        _type = MissionType.Combo;
    }

    public override void OnInit()
    {
        base.OnInit();
        LeanTween.addListener((int)Events.FIRED, OnFired);
        LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        LeanTween.removeListener((int)Events.FIRED, OnFired);
        // LeanTween.
    }

    public override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
        if( _statu == MissionStatu.Running)
        {
            if (timeStarted)
            {
                timeLeft -= delta;
                if (currentCount >= TargetCount)
                {
                    _statu = MissionStatu.Completed;
                }
                else
                {
                    if(timeLeft <= 0)
                    {
                        _statu = MissionStatu.Failed;
                    }
                }
            }
        }
       
    }

    private void OnFired(LTEvent obj)
    {
        //throw new NotImplementedException();
        timeStarted = true;
        timeLeft = LimitTime;

    }

    private void OnEnemyDie(LTEvent obj)
    {
        // throw new NotImplementedException();
        var edi = obj.data as EnemyDeadInfo;
        if (edi != null)
        {
            if (edi.animal.Id == Target.Id)
            {
                currentCount += 1;
            }
        }
    }
}
