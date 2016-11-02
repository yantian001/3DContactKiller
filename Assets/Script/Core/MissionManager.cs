using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MissionManager : MonoBehaviour
{

    private static MissionManager _instance = null;
    public static MissionManager Instance
    {
        private set
        {
            _instance = value;
        }
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MissionManager>();
                if (_instance == null)
                {
                    GameObject o = new GameObject("MissionManager");
                    _instance = o.AddComponent<MissionManager>();
                }

            }
            return _instance;
        }
    }

    public static MissionObject CurrentMission;
    public static int CurrentChapter = -1;
    public static int CurrentLevel = -1;
    public static LevelType CurrentLevelType = LevelType.None;
    public static string CurrentChapterSceneName = "";

    private int _lastPlayedChapter = -1;
    public int LastPlayedChapter
    {
        get
        {
            if (_lastPlayedChapter == -1)
            {
                if (!PlayerPrefs.HasKey("lpc"))
                {

                    PlayerPrefs.SetInt("lpc", 0);
                    PlayerPrefs.Save();
                }
                LastPlayedChapter = PlayerPrefs.GetInt("lpc");
            }
            return _lastPlayedChapter;
        }
        set
        {
            _lastPlayedChapter = value;
            PlayerPrefs.SetInt("lpc", _lastPlayedChapter);
            PlayerPrefs.Save();
        }
    }

    private LevelType _lastPlayedLevelType = LevelType.None;
    public LevelType LastPlayedLevelType
    {
        set
        {
            _lastPlayedLevelType = value;
            PlayerPrefs.SetInt("lplt", (int)_lastPlayedLevelType);
        }
        get
        {
            if (_lastPlayedLevelType == LevelType.None)
            {
                //if(!PlayerPrefs.HasKey("lplt"))
                //{
                //    LastPlayedLevelType = LevelType.MainTask;
                //}
                //else
                //{
                _lastPlayedLevelType = (LevelType)Enum.ToObject(typeof(LevelType), PlayerPrefs.GetInt("lplt", 0));
                //}
            }

            return _lastPlayedLevelType;
        }
    }


    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }



    public List<Chapter> lstChapters;
    public List<ChapterResult> lstChapterResults;


    void Init()
    {
        lstChapters = MissionDB.Load();
        LoadChapterResults();
        bool changed = false;
        for (int i = 0; i < lstChapters.Count; i++)
        {

            var rst = lstChapterResults.Find(p => { return p.Id == i; });
            if (rst == null)
            {
                lstChapterResults.Add(new ChapterResult(i));
                changed = true;
            }
        }
        if (changed)
        {
            SaveResult2File();
        }
        CurrentMission = lstChapters[0].CommonMission[0];
    }


    #region Chapter

    public Chapter _GetChapterByIndex(int index)
    {
        Chapter c = null;
        if (index < lstChapters.Count)
            c = lstChapters[index];
        return c;
    }

    public static Chapter GetChapterByIndex(int index)
    {
        return Instance._GetChapterByIndex(index);
    }

    #endregion

    #region chapter Results

    void LoadChapterResults()
    {
        string jsonStr = PlayerPrefs.GetString("cptRst", "");
        if (jsonStr == "")
            lstChapterResults = new List<ChapterResult>();
        else
        {
            lstChapterResults = DeserializeChapterResult(jsonStr);
        }
    }

    public static int GetCurrentLevelByType(LevelType _levelType)
    {
        int level = -1;
        switch (_levelType)
        {
            // case LevelType.MainTask:

        }
        return level;
        // throw new NotImplementedException();
    }

    /// <summary>
    /// 解析结果字符串
    /// </summary>
    /// <param name="jsonStr"></param>
    /// <returns></returns>
    private List<ChapterResult> DeserializeChapterResult(string jsonStr)
    {
        // throw new NotImplementedException();
        List<ChapterResult> results = new List<ChapterResult>();
        if (jsonStr != "")
        {
            string[] strs = jsonStr.Split(new char[] { ';' });
            for (int i = 0; i < strs.Length; ++i)
            {
                if (strs[i] != "")
                {
                    results.Add(new ChapterResult(strs[i]));
                }
            }
        }
        return results;
    }

    /// <summary>
    /// 获取章节指定任务类型的数量
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="_levelType"></param>
    /// <returns></returns>
    public static int GetTotalLevel(int chapter, LevelType _levelType)
    {
        int total = 0;
        if (chapter < Instance.lstChapters.Count)
        {
            switch (_levelType)
            {
                case LevelType.MainTask:
                    total = Instance.lstChapters[chapter].CommonMission.Count;
                    break;
                default:
                    break;
            }
        }
        return total;
        //throw new NotImplementedException();

    }

    /// <summary>
    /// 保存结果到文件
    /// </summary>
    private void SaveResult2File()
    {
        string jsonStr = SerializeChapterResult();
        PlayerPrefs.SetString("cptRst", jsonStr);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// 序列化结果
    /// </summary>
    private string SerializeChapterResult()
    {
        string strRst = "";
        if (lstChapterResults != null)
        {
            for (int i = 0; i < lstChapterResults.Count; i++)
            {
                if (lstChapterResults[i] != null)
                {
                    strRst += lstChapterResults[i].FormatString();
                    if (i != lstChapterResults.Count - 1)
                    {
                        strRst += ";";
                    }
                }
            }
        }
        return strRst;
        //throw new NotImplementedException();
    }

    public ChapterResult _GetResultByIndex(int index)
    {
        if (index < lstChapterResults.Count)
            return lstChapterResults[index];
        return null;
    }

    public static ChapterResult GetResultByIndex(int index)
    {
        return Instance._GetResultByIndex(index);
    }
    #endregion


}
