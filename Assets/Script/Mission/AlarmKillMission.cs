using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 击杀指定数量处于警报下的敌人
/// </summary>
public class AlarmKillMission : IMission
{

    public int TargetCount = 0;

    public bool IsLimitTarget = false;

    public Animal Target;

    int currentCount = 0;

    public AlarmKillMission()
    {
        _type = MissionType.AlarmKill;
    }

    public override void OnInit()
    {
        base.OnInit();
        if (currentCount >= TargetCount || (IsLimitTarget && Target == null))
        {
            _statu = MissionStatu.Completed;
        }
        else
        {
            LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
        }
    }

    public override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
        if(_statu == MissionStatu.Running)
        {
            if(currentCount >= TargetCount)
            {
                _statu = MissionStatu.Completed;
            }
        }
    }

    public override void OnDestory()
    {
        base.OnDestory();
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    private void OnEnemyDie(LTEvent obj)
    {
        //throw new NotImplementedException();
        var edi = obj.data as EnemyDeadInfo;
        if (edi != null)
        {
            if (edi.animal.statu == AnimalStatu.Warning)
            {
                if (IsLimitTarget)
                {
                    if (edi.animal.Id == Target.Id) currentCount += 1;
                }

                else
                {
                    currentCount += 1;
                }
            }
        }
    }
}
