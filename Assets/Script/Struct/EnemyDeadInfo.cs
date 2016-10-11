using UnityEngine;
using System.Collections;

public class EnemyDeadInfo
{

    public int score = 0;

    public Transform transform;

    public bool headShot = false;
    /// <summary>
    /// 意外伤亡
    /// </summary>
    public bool accidentkill = false;

    public HitPosition hitPos;

    public Animal animal;

}
