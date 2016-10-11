using UnityEngine;
using System.Collections;
using System;

public class ScoresManager : MonoBehaviour
{

    public static ScoresManager instance = null;

    public float currentScore = 0;

    public void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    private void OnEnemyDie(LTEvent obj)
    {
        var edi = obj.data as EnemyDeadInfo;
        if (edi != null)
        {
            currentScore += edi.score;
        }
        UpdateScoreDisplay();
        //throw new NotImplementedException();
    }

    /// <summary>
    /// 更新分数显示
    /// </summary>
    void UpdateScoreDisplay()
    {

    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
    }

    public void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
