using UnityEngine;
using System.Collections;

public class TimeMission : IMission
{
    public float TimeCount = 0;

    public TimeMission()
    {
        _type = MissionType.Time;
    }
}
