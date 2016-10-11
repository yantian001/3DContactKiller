using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScoresMission : IMission
{

    public ScoresMission()
    {
        _type = MissionType.Score;
    }

    public float TargetScores = 0;
    public bool IsTimeLimit = false;
    public float LimitTime = 0f;

    public override void OnUpdate(float delta)
    {
        base.OnUpdate(delta);
        if (IsMissionRunning())
        {
            if (ScoresManager.instance != null)
            {
                if (ScoresManager.instance.currentScore >= TargetScores)
                {
                    _statu = MissionStatu.Completed;
                }
            }
        }
    }
}
