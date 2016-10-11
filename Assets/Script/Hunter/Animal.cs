using UnityEngine;
using System.Collections;

public enum AnimalStatu
{
    /// <summary>
    /// 休闲
    /// </summary>
    Idle,
    /// <summary>
    /// 发现异常,警觉中
    /// </summary>
    Warning,
    /// <summary>
    /// 已经警觉
    /// </summary>
    Warned 
}

public class Animal : MonoBehaviour
{
    public int Id;
    public string Name;
    public Texture2D Avater;

    public AnimalStatu statu = AnimalStatu.Idle;

    public bool isTarget = true;

    public void OnDestroy()
    {
        LeanTween.dispatchEvent((int)Events.ENEMYAWAY);
    }
}
