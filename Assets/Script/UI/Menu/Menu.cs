﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using System;

public class Menu : MonoBehaviour
{

    int currentScene = -1;

    public Objective currentObjective;
    RectTransform parent;

    public Transform boosTransform = null;

    bool isLoopTask = false;

    LevelData ld = null;

    public void Awake()
    {
        LeanTween.addListener((int)Events.PLAYCLICKED, OnPlayClicked);
    }

    private void OnPlayClicked(LTEvent obj)
    {
        //throw new NotImplementedException();
        if (currentObjective == null || currentScene == -1 || ld == null)
            return;
        GameValue.s_currentObjective = currentObjective;
        GameValue.mapId = currentScene + 1;
        GameValue.s_CurrentSceneName = ld.sceneName;
        GameValue.s_IsRandomObjective = isLoopTask;
        GameValue.s_LeveData = ld;
        LeanTween.dispatchEvent((int)Events.GAMESTART);
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.PLAYCLICKED, OnPlayClicked);
    }



    // Use this for initialization
    void Start()
    {
        parent = GetComponent<RectTransform>();
        //currentScene = Player.CurrentUser.LastPlayedScene;
        UpdateSceneDisplay(Player.CurrentUser.LastPlayedScene);

        //OnMainTaskSelected();
    }

    void DefaultSelect()
    {
        CommonUtils.SetChildToggleOn(parent, "Middle/LoopTasks", false);
        CommonUtils.SetChildToggleOn(parent, "Middle/MainTasks", false);
        CommonUtils.SetChildToggleOn(parent, "Middle/Boss", false);
        if (currentObjective == null)
        {
            if (ld.IsMainCompleted())
            {
                CommonUtils.SetChildToggleOn(parent, "Middle/LoopTasks", true);
            }
            else
                CommonUtils.SetChildToggleOn(parent, "Middle/MainTasks", true);
        }
    }

    void UpdateSceneDisplay(int scene)
    {
        if (currentScene == scene)
            return;
        int len = ObjectiveManager.Instance.GetLevelLength();
        while (scene < 0 || scene >= len)
        {
            if (scene < 0)
                scene += len;
            if (scene >= len)
                scene -= len;
        }
        ld = ObjectiveManager.Instance.GetLevelData(scene);
        if (ld == null)
            return;
        currentScene = scene;

        CommonUtils.SetChildRawImage(parent, "Middle/MainTasks/backImage", ld.mainTexture);
        CommonUtils.SetChildRawImage(parent, "Middle/LoopTasks/backImage", ld.loopTexture);
        CommonUtils.SetChildText(parent, "Middle/MainTasks/Background/Count", ld.GetCurrentLevelString());

        if (ld.IsMainCompleted())
        {
            CommonUtils.SetChildToggleInteractable(parent, "Middle/MainTasks", false);
            if (ld.IsBossCompleted())
            {
                CommonUtils.SetChildToggleInteractable(parent, "Middle/Boss", false);

            }
            else
            {
                CommonUtils.SetChildToggleInteractable(parent, "Middle/Boss", true);

            }
        }
        else
        {
            CommonUtils.SetChildToggleInteractable(parent, "Middle/MainTasks", true);
            CommonUtils.SetChildToggleInteractable(parent, "Middle/Boss", false);

        }

        if (ld.currentLevel > 0)
        {
            CommonUtils.SetChildToggleInteractable(parent, "Middle/LoopTasks", true);
        }
        else
            CommonUtils.SetChildToggleInteractable(parent, "Middle/LoopTasks", false);

        if (boosTransform)
        {
            boosTransform.DetachChildren();
            if (ld.bossObjectives && ld.bossObjectives.targetObjects)
            {
                var createObj = (GameObject)GameObject.Instantiate(ld.bossObjectives.targetObjects, boosTransform.position, Quaternion.identity);

                createObj.transform.SetParent(boosTransform);
                // createObj.transform.localPosition = Vector3.zero;
                createObj.AddComponent<AutoDestroyByRemove>();
                if (ld.bossLocalPosition != Vector3.zero)
                {
                    createObj.transform.localPosition = ld.bossLocalPosition;

                }
                if (ld.bossScale != Vector3.zero)
                {
                    createObj.transform.localScale = ld.bossScale;

                }
                CommonUtils.SetChildComponentActive<BehaviorTree>(createObj.transform, false);

                // CommonUtils.SetChildComponentActive<Rigidbody>(createObj.transform, false);
            }
        }
        currentObjective = null;

        DefaultSelect();
    }


    public void OnMainTaskToggled(bool b)
    {
        if (b)
            OnMainTaskSelected();
    }

    public void OnLoopTaskToggled(bool b)
    {
        if (b)
            OnLoopTaskSelected();
    }

    public void OnBossTaskToggled(bool b)
    {
        if (b)
            onBossTaskSelected();
    }

    private void onBossTaskSelected()
    {
        // throw new NotImplementedException();
        isLoopTask = false;
        Objective obj = ObjectiveManager.Instance.GetBossObjective(currentScene);
        Display(obj);
    }

    private void OnLoopTaskSelected()
    {
        isLoopTask = true;
        Objective obj = ObjectiveManager.Instance.GetSceneLoopObjective(currentScene);
        Display(obj);
        //throw new NotImplementedException();
    }

    public void OnNextClicked()
    {
        UpdateSceneDisplay(currentScene + 1);
    }

    public void OnPrevClicked()
    {
        UpdateSceneDisplay(currentScene - 1);
    }

    public void OnMainTaskSelected()
    {
        Objective obj = ObjectiveManager.Instance.GetSceneCurrentObjective(currentScene);
        isLoopTask = false;
        Display(obj);

    }

    void Display(Objective obj)
    {
        if (currentObjective == obj)
            return;
        currentObjective = obj;
        Debug.Log(string.Format("{0} ~ {1}", Mathf.CeilToInt(currentObjective.reward * 0.8f), Mathf.CeilToInt(currentObjective.reward * 1.5f)));
        //显示target
        CommonUtils.SetChildText(parent, "Middle/Bg/TargetTitle/Text", currentObjective.GetObjString());
        CommonUtils.SetChildText(parent, "Middle/Bg/RewardTitle/RewardText", string.Format("{0} ~ {1}", Mathf.CeilToInt(currentObjective.reward * 0.8f), Mathf.CeilToInt(currentObjective.reward * 1.5f)));

        int recommandCount = 0;
        DisplayRecommand("Middle/Bg/Recommand/PowerItem", currentObjective.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, currentObjective.powerRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/MaxZoom", currentObjective.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, currentObjective.maxZoomRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/stability", currentObjective.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, currentObjective.stabilityRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/capacity", currentObjective.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, currentObjective.stabilityRequired), ref recommandCount);
    }

    public void DisplayRecommand(string name, bool show, bool isArrive, ref int displayCount)
    {
        if (show)
        {
            CommonUtils.SetChildActive(parent, name, show);
            RectTransform powerRect = CommonUtils.GetChildComponent<RectTransform>(parent, name);
            if (powerRect)
            {
                powerRect.anchoredPosition = new Vector2(displayCount * 40, 0);
                CommonUtils.SetChildActive(powerRect, "WarningImage", !isArrive);
            }
            displayCount += 1;
        }
        else
        {
            CommonUtils.SetChildActive(parent, name, show);
        }
    }
}
