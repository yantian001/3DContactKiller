using UnityEngine;
using System.Collections;


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

    /// <summary>
    /// 初始化方法
    /// </summary>
    public virtual void OnInit()
    {
        _statu = MissionStatu.Running;
    }
    /// <summary>
    /// 类似于每帧执行
    /// </summary>
    /// <param name="delta"></param>
    public virtual void OnUpdate(float delta) { }
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
}
