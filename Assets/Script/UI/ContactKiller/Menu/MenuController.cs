using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    #region Public Variable
    public RawImage BackGround;
    public MenuButton mbPrimary;
    public MenuButton mbCommon;
    public MenuButton mbDaily;
    public MenuButton mbBoss;
    public Text txtLevelIndex;
    public Button btnNextLevel;
    public Button btnPreLevel;
    public Text txtRewards;
    public Text txtDescription;
    public RawImage imgPhoto;
    public Button btnNextChapter;
    public Button btnPreChapter;
    public Text txtChapterName;

    public RectTransform MissionPanel;
    public RectTransform LockedPanle;
    public RawImage Thumb;

    public Color unArriveTextColor;
    #endregion

    #region Private Variable
    private int _selectChapter = -1;
    private Chapter _currentChapter = null;
    private ChapterResult _currentChapterResult = null;
    private LevelType _levelType = LevelType.None;
    private int _currentLevel;
    private MissionObject _currentMission;
    private RectTransform parent;
    private int unArrivedCount;


    #endregion

    #region Monobehavior Method

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.RECOMMENDCONTINUE, OnRecommandCotinue);
        LeanTween.addListener((int)Events.PLAYCLICKED, OnPlayClicked);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.PLAYCLICKED, OnPlayClicked);
        LeanTween.removeListener((int)Events.RECOMMENDCONTINUE, OnRecommandCotinue);
    }



    // Use this for initialization
    void Start()
    {
        int lstChapter = MissionManager.Instance.LastPlayedChapter;
        parent = GetComponent<RectTransform>();
        if (mbPrimary)
        {
            mbPrimary.OnClick.AddListener(OnMenuButtonClicked);
        }
        if (mbDaily)
        {
            mbDaily.OnClick.AddListener(OnMenuButtonClicked);
        }
        if (mbCommon)
        {
            mbCommon.OnClick.AddListener(OnMenuButtonClicked);
        }
        if (mbBoss)
        {
            mbBoss.OnClick.AddListener(OnMenuButtonClicked);
        }
        DisplayChapter(lstChapter);

        btnNextChapter.onClick.AddListener(OnNextChapterClicked);
        btnPreChapter.onClick.AddListener(OnPreChapterClicked);
        btnNextLevel.onClick.AddListener(OnNextLevelClicked);
        btnPreLevel.onClick.AddListener(OnPreLevelClicked);
    }

    #endregion

    #region Play Game Logic


    private void OnPlayClicked(LTEvent obj)
    {
        //throw new NotImplementedException();
        if (unArrivedCount > 0)
        {
            OnRecommandClick(null);
        }
        else
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        if (_currentChapter == null || _currentMission == null || _currentLevel < 0)
            return;
        MissionManager.Instance.LastPlayedChapter = _selectChapter;
        MissionManager.Instance.LastPlayedLevelType = _levelType;
        MissionManager.CurrentMission = _currentMission;
        MissionManager.CurrentChapter = _selectChapter;
        MissionManager.CurrentLevel = _currentLevel;
        MissionManager.CurrentLevelType = _levelType;
        MissionManager.CurrentChapterSceneName = _currentChapter.SceneName;
        //GameValue.s_currentObjective = currentObjective;
        //GameValue.mapId = currentScene + 1;
        GameValue.s_CurrentSceneName = _currentChapter.SceneName;
        ////GameValue.s_IsRandomObjective = isLoopTask;
        //GameValue.s_LeveData = ld;
        LeanTween.dispatchEvent((int)Events.GAMESTART);
    }


    #endregion

    #region Display Mission Info.
    private void DisplayChapter(int select)
    {
        //throw new NotImplementedException();
        if (select == _selectChapter)
            return;
        int len = MissionManager.Instance.lstChapters.Count;
        while (select < 0 || select >= len)
        {
            if (select < 0)
                select += len;
            if (select >= len)
                select -= len;
        }
        _selectChapter = select;
        _currentChapter = MissionManager.GetChapterByIndex(_selectChapter);
        _currentChapterResult = MissionManager.GetResultByIndex(_selectChapter);
        txtChapterName.text = _currentChapter.Name;
        BackGround.texture = _currentChapter.BgTexture;
        if (_currentChapterResult.unlocked)
        {
            ChangeDisplay(false);
            mbPrimary.Interactable = true;
            mbCommon.Interactable = _currentChapterResult.commonUnlocked;
            mbDaily.Interactable = _currentChapterResult.dailyUnlocked;
            mbBoss.Interactable = _currentChapterResult.boosUnlocked;
            _levelType = LevelType.None;
            _currentLevel = 0;
            _currentMission = null;
            ChangeLevelType(MissionManager.Instance.LastPlayedLevelType);

        }
        else
        {
            //显示锁死的界面
            ChangeDisplay(true);
            Thumb.texture = _currentChapter.ThumbTexture;
            print("Chapter Locked");
        }


    }

    void ChangeDisplay(bool locked)
    {
        MissionPanel.gameObject.SetActive(!locked);
        LockedPanle.gameObject.SetActive(locked);
        mbPrimary.gameObject.SetActive(!locked);
        mbCommon.gameObject.SetActive(!locked);
        mbDaily.gameObject.SetActive(!locked);
        mbBoss.gameObject.SetActive(!locked);
    }

    void ChangeLevelType(LevelType type)
    {
        if (type == _levelType)
            return;
        _levelType = type;
        int level = 0;
        if (_levelType == LevelType.MainTask)
        {
            level = _currentChapterResult.primaryLevel;
        }
        else if (_levelType == LevelType.DailyTask)
        {
            level = _currentChapterResult.dailyLevel;
        }
        else if (_levelType == LevelType.CommonTask)
        {
            level = _currentChapterResult.commonLevel;
        }
        else if (_levelType == LevelType.BossTask)
        {
            level = _currentChapterResult.boosLevel;
        }
        DisplayLevel(level);
    }

    private void DisplayLevel(int level)
    {
        //throw new NotImplementedException();
        if (_currentLevel != level && level >= 0)
        {
            _currentLevel = level;
        }

        int total = _currentChapter.GetTotalByType(_levelType);
        _currentLevel = Mathf.Min(_currentLevel, total - 1);
        int totalUnlocked = MissionManager.GetUnlockedTotalByType(_selectChapter, _levelType);
        txtLevelIndex.text = string.Format("{0}/{1}", _currentLevel + 1, total);
        if (_currentLevel == 0 || _currentLevel == totalUnlocked)
        {
            if (_currentLevel <= 0)
            {
                btnPreLevel.interactable = false;
                // btnNextLevel.interactable = true;
            }
            else
            {
                btnPreLevel.interactable = true;
            }
            if (_currentLevel >= totalUnlocked)
            {
                btnNextLevel.interactable = false;
                //btnPreLevel.interactable = true;
            }
            else
            {
                btnNextLevel.interactable = true;
            }
        }
        else
        {
            btnNextLevel.interactable = true;
            btnPreLevel.interactable = true;
        }

        _currentMission = _currentChapter.GetMissionByIndex(_currentLevel, _levelType);
        DisplayMission(_currentMission);
    }

    private void DisplayMission(MissionObject m)
    {
        if (m != null)
        {
            txtRewards.text = m.reward.ToString();
            txtDescription.text = m.GetDescription();
            Texture2D txture2d = m.GetPhotoTexture();
            if (txture2d)
            {
                txtDescription.rectTransform.sizeDelta = new Vector2(165, txtDescription.rectTransform.sizeDelta.y);
                imgPhoto.texture = txture2d;
                imgPhoto.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                txtDescription.rectTransform.sizeDelta = new Vector2(270, txtDescription.rectTransform.sizeDelta.y);
                imgPhoto.transform.parent.gameObject.SetActive(false);
            }

            int recommandCount = 0;
            DisplayRecommand("Panel/Mission/Recommand/PowerItem", m.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, m.powerRequired), ref recommandCount);
            DisplayRecommand("Panel/Mission/Recommand/MaxZoom", m.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, m.maxZoomRequired), ref recommandCount);
            DisplayRecommand("Panel/Mission/Recommand/stability", m.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, m.stabilityRequired), ref recommandCount);
            DisplayRecommand("Panel/Mission/Recommand/capacity", m.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, m.capacityRequired), ref recommandCount);
            //throw new NotImplementedException();

            EventTrigger trigger = CommonUtils.GetChildComponent<EventTrigger>(parent, "Panel/Mission/Recommand");
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
                DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/PowerItem", m.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, m.powerRequired), m.powerRequired, ref recommandCount);
                DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/MaxZoom", m.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, m.maxZoomRequired), m.maxZoomRequired, ref recommandCount);
                DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/Stability", m.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, m.stabilityRequired), m.stabilityRequired, ref recommandCount);
                DisplayRecommandPopup("WeaponUpgrade/Background/Recommand/capacity", m.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, m.capacityRequired), m.capacityRequired, ref recommandCount);
            }
        }
    }
    #endregion

    #region Weapon Recommend
    private void OnRecommandClick(BaseEventData arg0)
    {
        CommonUtils.SetChildActive(parent, "WeaponUpgrade", true);
    }

    /// <summary>
    /// 关闭弹出页面
    /// </summary>
    public void OnRecommandCancel()
    {
        CommonUtils.SetChildActive(parent, "WeaponUpgrade", false);
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
                powerRect.anchoredPosition = new Vector2(displayCount * 65, 0);
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
                powerRect.anchoredPosition = new Vector2(displayCount * 70, 0);
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

    private void OnRecommandCotinue(LTEvent obj)
    {
        //   throw new NotImplementedException();
        GameStart();
    }

    #endregion

    #region Button Click Event

    private void OnMenuButtonClicked(LevelType type)
    {
        //throw new NotImplementedException();
        // _levelType = type;
        ChangeLevelType(type);
    }
    /// <summary>
    /// 下一关
    /// </summary>
    void OnNextLevelClicked()
    {
        // print("Next Level");
        DisplayLevel(_currentLevel + 1);
    }
    /// <summary>
    /// 上一关
    /// </summary>
    void OnPreLevelClicked()
    {
        //print("Prev Level");
        DisplayLevel(_currentLevel - 1);
    }
    /// <summary>
    /// 下一章
    /// </summary>
    void OnNextChapterClicked()
    {
        print("Next Chapter");
        DisplayChapter(_selectChapter + 1);

    }
    /// <summary>
    /// 上一章
    /// </summary>
    void OnPreChapterClicked()
    {
        print("Prev Chapter");
        DisplayChapter(_selectChapter - 1);
    }
    #endregion
}
