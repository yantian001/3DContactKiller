using UnityEngine;
using System.Collections;

public class AlarmMission : IMission
{

    public AlarmMission()
    {
        _type = MissionType.Alarm;
    }

    public override void OnInit()
    {
        base.OnInit();
        LeanTween.addListener((int)Events.ANIMALWARNED, OnAnimalWarned);
        _statu = MissionStatu.Completed;
    }

    void OnAnimalWarned(LTEvent ent)
    {
        _statu = MissionStatu.Failed;
    }

    public override void OnDestory()
    {
        base.OnDestory();
        LeanTween.removeListener((int)Events.ANIMALWARNED, OnAnimalWarned);
    }
}
