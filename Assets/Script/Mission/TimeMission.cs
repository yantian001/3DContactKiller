using UnityEngine;
using System.Collections;

public class TimeMission : IMission
{
    /// <summary>
    /// 总任务时长
    /// </summary>
    public float TimeCount = 0;
    /// <summary>
    /// 现在时长
    /// </summary>
    public float TimeLeft = 0;
    public TimeMission()
    {
        _type = MissionType.Time;
    }

    public override void OnInit()
    {
        base.OnInit();
        TimeLeft = TimeCount;
        
    }

    public override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
        if(_statu == MissionStatu.Running)
        {
            TimeLeft -= delta;
            if(TimeLeft <= 0)
            {
                _statu = MissionStatu.Failed;
            }
        }
    }
}
