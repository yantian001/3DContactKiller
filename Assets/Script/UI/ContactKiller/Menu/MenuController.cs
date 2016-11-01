using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuController : MonoBehaviour
{

    public MenuButton mbPrimary;
    public MenuButton mbCommon;
    public MenuButton mbDaily;
    public MenuButton mbBoss;
    public Text txtLevelIndex;
    public Button btnNextLevel;
    public Button btnPreLevel;

    public Button btnNextChapter;
    public Button btnPreChapter;

    public Text txtChapterName;

    public Text txtRewards;


    private int _selectChapter = -1;

    private Chapter _currentChapter = null;
    private ChapterResult _currentChapterResult = null;

    private LevelType _levelType = LevelType.None;
    private int _currentLevel;
    private MissionObject _currentMission;

    private RectTransform parent;

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

        if (_currentChapterResult.unlocked)
        {
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

            print("Chapter Locked");
        }


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
        txtLevelIndex.text = string.Format("{0}/{1}", _currentLevel + 1, total);
        if (_currentLevel == 0)
        {
            btnPreLevel.interactable = false;
            btnNextLevel.interactable = true;
        }
        else if (_currentLevel == total - 1)
        {
            btnNextLevel.interactable = false;
            btnPreLevel.interactable = true;
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
        if(m != null)
        {
            txtRewards.text = m.reward.ToString();
        }

        int recommandCount = 0;
        DisplayRecommand("Panel/Mission/Recommand/PowerItem", m.powerRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(0, m.powerRequired), ref recommandCount);
        DisplayRecommand("Panel/Mission/Recommand/MaxZoom", m.maxZoomRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(1, m.maxZoomRequired), ref recommandCount);
        DisplayRecommand("Panel/Mission/Recommand/stability", m.stabilityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(2, m.stabilityRequired), ref recommandCount);
        DisplayRecommand("Panel/Mission/Recommand/capacity", m.capacityRequired != -1, WeaponManager.Instance.IsWeaponMeetReq(3, m.capacityRequired), ref recommandCount);
        //throw new NotImplementedException();
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
