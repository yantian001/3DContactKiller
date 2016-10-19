using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class TargetMission : IMission
{
    /// <summary>
    /// 任务目标
    /// </summary>
    public GameObject TargetObject;
    public Animal Target = null;
    public int TartgetCount = 0;
    public bool NeedHeadShot = false;
    int currentCount = 0;
    Animal animal;

    public TargetMission()
    {
        _type = MissionType.Target;
    }

    public override void OnInit()
    {
        base.OnInit();
        currentCount = 0;
        _statu = MissionStatu.Running;
        // if (TargetObject)
        //{
        //   animal = TargetObject.GetComponent<Animal>();
        if (!animal)
        {
            _statu = MissionStatu.Completed;
        }
        else
            LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
        // }
    }

    public override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
        if (_statu == MissionStatu.Running)
        {
            if (currentCount >= TartgetCount)
                _statu = MissionStatu.Completed;
        }
    }

    private void OnEnemyDie(LTEvent obj)
    {
        if (_statu == MissionStatu.Running)
        {
            var edi = obj.data as EnemyDeadInfo;
            if (edi != null)
            {
                if (edi.animal.Id == animal.Id)
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

    public override void OnDestory()
    {
        base.OnDestory();
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    public override string GetDescription()
    {
        string str = string.Format("Kill {0} {1}", TartgetCount, Target.Name);
        if (NeedHeadShot)
        {
            str += " with headshot";
        }
        return str;
    }

    public Texture2D GetAvatarTexture()
    {
        if (animal)
        {


            return animal.Avater;
        }
        return null;
    }
}
