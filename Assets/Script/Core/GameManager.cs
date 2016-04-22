﻿using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int targetKilled = 0;

    bool enemyCleared = false;
    public GameStatu gameStatu = GameStatu.Init;

    public AudioClip successAC;

    public AudioClip failAC;

    #region 单例模式
    private static GameManager _instance;

    public static GameManager Instance
    {
        private set
        {
            _instance = value;
        }
        get
        {
            return _instance;
        }
    }

    #endregion

    public void Awake()
    {
        _instance = this;
        //监听死亡
        LeanTween.addListener((int)Events.ENEMYDIE, OnEnemyDie);
        LeanTween.addListener((int)Events.ENEMYCLEARED, OnEnemyCleared);
        LeanTween.addListener((int)Events.GAMEPAUSE, OnPause);
        LeanTween.addListener((int)Events.PREVIEWSTART, OnPreviewStart);
        Time.timeScale = 1;
        BehaviorDesigner.Runtime.GlobalVariables.Instance.SetVariableValue("Fired", false);
    }

    private void OnPreviewStart(LTEvent obj)
    {
        //throw new NotImplementedException();
        ChangeGameStatu(GameStatu.InGame);
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.ENEMYDIE, OnEnemyDie);
        LeanTween.removeListener((int)Events.ENEMYCLEARED, OnEnemyCleared);
        LeanTween.removeListener((int)Events.GAMEPAUSE, OnPause);
        LeanTween.removeListener((int)Events.PREVIEWSTART, OnPreviewStart);
        _instance = null;
    }

    void OnPause(LTEvent evt)
    {

        if (gameStatu == GameStatu.InGame)
        {
            ChangeGameStatu(GameStatu.Paused);
        }
        LeanTween.addListener((int)Events.GAMECONTINUE, OnContinue);
        Time.timeScale = 0;
    }

    void OnContinue(LTEvent evt)
    {
        LeanTween.removeListener((int)Events.GAMECONTINUE, OnContinue);
        ChangeGameStatu(GameStatu.InGame);
        Time.timeScale = 1;
    }

    public bool IsWillFinishGame(AS_Bullet bullet, AS_BulletHiter hiter)
    {
        if (!IsInGame())
            return false;
        if (!NearFinishTarget())
            return false;
        DamageManager dm = hiter.RootObject.GetComponent<DamageManager>();
        Animal animal = hiter.RootObject.GetComponent<Animal>();
        if (IsObjectiveTarget(animal, hiter.HitPos) && dm.hp - bullet.Damage <= 0)
            return true;
        return false;

    }


    public bool NearFinishTarget()
    {
        return targetKilled + 1 == GameValue.s_currentObjective.objectiveCount;
    }

    private void OnEnemyCleared(LTEvent obj)
    {
        //throw new NotImplementedException();
        enemyCleared = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInGame())
        {
            if (targetKilled >= GameValue.s_currentObjective.objectiveCount)
            {
                ChangeGameStatu(GameStatu.Completed);
                OnGameFinish(true);
            }
            else if (enemyCleared && targetKilled <= GameValue.s_currentObjective.objectiveCount)
            {
                ChangeGameStatu(GameStatu.Failed);
                OnGameFinish(false);
            }
        }
    }

    private void OnGameFinish(bool v)
    {
        //throw new NotImplementedException();
        //Debug.Log("Game Finish :" + v.ToString());
        if (v)
        {

            GameValue.s_currentObjective.IsFinished = true;
            if (GameValue.s_IsRandomObjective)
            {
                if (GameValue.s_LeveData)
                    GameValue.s_LeveData.SetRandom(-1);
            }
        }
        LeanTween.delayedCall(2f, () =>
        {
            if (v)
            {
                if (successAC)
                    LeanAudio.play(successAC);
            }
            else
            {
                if (failAC)
                {
                    LeanAudio.play(failAC);
                }
            }
            LeanTween.dispatchEvent((int)Events.GAMEFINISH, v);
        });

    }

    public bool IsInGame()
    {
        return gameStatu == GameStatu.InGame;
    }

    void ChangeGameStatu(GameStatu statu)
    {
        gameStatu = statu;
        GameValue.staus = statu;
       // Debug.Log("Game Statu : " + gameStatu.ToString());
    }

    public bool IsObjectiveTarget(Animal animal, HitPosition hitPos)
    {
        if (animal == null)
            return false;
        Animal target = GameValue.s_currentObjective.targetObjects.GetComponent<Animal>();
        if (target.Id != animal.Id)
        {
            return false;
        }
        else
        {
            if (GameValue.s_currentObjective.objectiveType == ObjectiveType.COUNT)
            {
                return true;
            }
            else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.HEADKILL)
            {
                if (hitPos == HitPosition.HEAD)
                    return true;
            }
            else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.HEARTKILL)
            {
                if (hitPos == HitPosition.HEART)
                    return true;
            }
            else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.LUNGKILL)
            {
                if (hitPos == HitPosition.LUNG)
                    return true;
            }
        }
        return false;
    }

    public void OnEnemyDie(LTEvent evt)
    {
        if (evt.data != null)
        {
            EnemyDeadInfo edi = evt.data as EnemyDeadInfo;
            if (edi != null && edi.animal != null)
            {
                Animal target = GameValue.s_currentObjective.targetObjects.GetComponent<Animal>();
                if (target)
                {
                    if (target.Id == edi.animal.Id)
                    {
                        if (GameValue.s_currentObjective.objectiveType == ObjectiveType.COUNT)
                        {
                            targetKilled += 1;
                        }
                        else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.HEADKILL)
                        {
                            if (edi.hitPos == HitPosition.HEAD)
                                targetKilled += 1;
                        }
                        else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.HEARTKILL)
                        {
                            if (edi.hitPos == HitPosition.HEART)
                                targetKilled += 1;
                        }
                        else if (GameValue.s_currentObjective.objectiveType == ObjectiveType.LUNGKILL)
                        {
                            if (edi.hitPos == HitPosition.LUNG)
                                targetKilled += 1;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Dont find type Animal at targetobjects!");
                }
            }
        }
    }
}
