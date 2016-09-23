using UnityEngine;
using BehaviorDesigner.Runtime;
using System;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    int currentScene = -1;

    public Objective currentObjective;
    RectTransform parent;

    public Transform boosTransform = null;

    public Color unArriveTextColor;

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
        Player.CurrentUser.LastPlayedScene = currentScene;
        GameValue.s_currentObjective = currentObjective;
        GameValue.mapId = currentScene + 1;
        GameValue.s_CurrentSceneName = ld.sceneName;
        //GameValue.s_IsRandomObjective = isLoopTask;
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
            if (Player.CurrentUser.IsMainTaskCompleted(ld.Id, ld.GetLevelsCount()))
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
        int levelCount = ld.GetLevelsCount();
        int curLevel = Player.CurrentUser.GetSceneCurrentLevel(ld.Id, levelCount);
        CommonUtils.SetChildText(parent, "Middle/MainTasks/Background/Count", string.Format("{0}/{1}", curLevel, levelCount));

        if (curLevel >= levelCount)
        {
            CommonUtils.SetChildToggleInteractable(parent, "Middle/MainTasks", false);
            if (Player.CurrentUser.GetSceneBossLevelFinished(ld.Id))
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

        if (curLevel > 1)
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
        //isLoopTask = false;
        GameValue.s_LevelType = LevelType.BossTask;
        Objective obj = ObjectiveManager.Instance.GetBossObjective(currentScene);
        Display(obj);
    }

    private void OnLoopTaskSelected()
    {
        //isLoopTask = true;
        GameValue.s_LevelType = LevelType.LoopTask;
        int loopLevel = Player.CurrentUser.GetSceneRandomLevel(currentScene);
        Objective obj = ObjectiveManager.Instance.GetSceneCurrentObjective(currentScene, loopLevel);
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
        GameValue.s_LevelType = LevelType.MainTask;
        int currentLevel = Player.CurrentUser.GetSceneCurrentLevel(currentScene);
        Objective obj = ObjectiveManager.Instance.GetSceneCurrentObjective(currentScene, currentLevel - 1);
        // isLoopTask = false;
        Display(obj);

    }

    int unArrivedCount = 0;

    void Display(Objective obj)
    {
        if (currentObjective == obj)
            return;
        currentObjective = obj;
        // Debug.Log(string.Format("{0} ~ {1}", Mathf.CeilToInt(currentObjective.reward * 0.8f), Mathf.CeilToInt(currentObjective.reward * 1.5f)));
        //显示target
        CommonUtils.SetChildText(parent, "Middle/Bg/TargetTitle/Text", currentObjective.GetObjString());
        CommonUtils.SetChildText(parent, "Middle/Bg/RewardTitle/RewardText", string.Format("{0} ~ {1}", Mathf.CeilToInt(currentObjective.reward * 0.8f), Mathf.CeilToInt(currentObjective.reward * 1.5f)));

        int recommandCount = 0;
        unArrivedCount = 0;
        DisplayRecommand("Middle/Bg/Recommand/PowerItem", currentObjective.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, currentObjective.powerRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/MaxZoom", currentObjective.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, currentObjective.maxZoomRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/stability", currentObjective.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, currentObjective.stabilityRequired), ref recommandCount);
        DisplayRecommand("Middle/Bg/Recommand/capacity", currentObjective.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, currentObjective.capacityRequired), ref recommandCount);
        //添加点击事件,有不符合要求的武器属性时,弹出提示框
        EventTrigger trigger = CommonUtils.GetChildComponent<EventTrigger>(parent, "Middle/Bg/Recommand");
        if (trigger)
        {
            trigger.triggers.Clear();
        }
        if (unArrivedCount > 0)
        {
            //添加点击事件
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(OnRecommandClick);
            trigger.triggers.Add(entry);
            recommandCount = 0;
            //更新弹出显示页面
            DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/PowerItem", currentObjective.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, currentObjective.powerRequired), currentObjective.powerRequired, ref recommandCount);
            DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/MaxZoom", currentObjective.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, currentObjective.maxZoomRequired), currentObjective.maxZoomRequired, ref recommandCount);
            DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/Stability", currentObjective.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, currentObjective.stabilityRequired), currentObjective.stabilityRequired, ref recommandCount);
            DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/capacity", currentObjective.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, currentObjective.capacityRequired), currentObjective.capacityRequired, ref recommandCount);
        }
    }



    public void DisplayRecommand(string name, bool show, bool isArrive, ref int displayCount)
    {
        if (!isArrive)
        {
            unArrivedCount += 1;
        }
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

    /// <summary>
    /// 显示武器属性弹出页面
    /// </summary>
    /// <param name="name"></param>
    /// <param name="show"></param>
    /// <param name="isArrive"></param>
    /// <param name="required"></param>
    /// <param name="displayCount"></param>
    public void DisplayRecommandPopup(string name, bool show, bool isArrive, float required, ref int displayCount)
    {
        if (show)
        {
            CommonUtils.SetChildActive(parent, name, show);
            RectTransform powerRect = CommonUtils.GetChildComponent<RectTransform>(parent, name);
            if (powerRect)
            {
                powerRect.anchoredPosition = new Vector2(displayCount * 50, 0);
                CommonUtils.SetChildActive(powerRect, "WarningImage", !isArrive);
                CommonUtils.SetChildTextAndColor(powerRect, "Text", required.ToString(), isArrive ? Color.white : unArriveTextColor);
            }
            displayCount += 1;
        }
        else
        {
            CommonUtils.SetChildActive(parent, name, show);
        }
    }

    private void OnRecommandClick(BaseEventData arg0)
    {
        // throw new NotImplementedException();
        //Debug.Log("Recommand Clicked");
        CommonUtils.SetChildActive(parent, "WeaponUpgrade", true);
    }
    /// <summary>
    /// 关闭弹出页面
    /// </summary>
    public void OnRecommandCancel()
    {
        CommonUtils.SetChildActive(parent, "WeaponUpgrade", false);
    }

}
