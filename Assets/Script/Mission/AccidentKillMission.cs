using UnityEngine;
using System.Collections;
using System;

public class AccidentKillMission : IMission
{
    public int TargetCount = 0;

    public bool IsLimitTarget = false;

    public Animal Target;

    private int currentCount;

    public AccidentKillMission()
    {
        _type = MissionType.AccidentKill;
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
        if (_statu == MissionStatu.Running)
        {
            if (currentCount >= TargetCount)
            {
                _statu = MissionStatu.Completed;
            }
        }
    }
    private void OnEnemyDie(LTEvent obj)
    {
        //throw new NotImplementedException();
        var edi = obj.data as EnemyDeadInfo;
        if (edi != null)
        {
            if (edi.accidentkill)
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
