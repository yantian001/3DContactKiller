using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Chapter
{
    public int Id = 0;
    public string Name = "";
    public string SceneName = "";
    public Texture2D BgTexture;
    public Texture2D ThumbTexture;
#if UNITY_EDITOR
    private SceneAsset _scene;
    public SceneAsset Scene
    {
        get
        {
            return _scene;
        }
        set
        {
            _scene = value;
            if (_scene != null)
                SceneName = _scene.name;
        }
    }
#endif
    public List<MissionObject> CommonMission = new List<MissionObject>();
    

    #region Menu UI Method
    /// <summary>
    /// 获取任务类型的总关卡数量
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetTotalByType(LevelType type)
    {
        int total = 0;
        switch (type)
        {
            case LevelType.MainTask:
                total = CommonMission.Count;
                break;
            default:
                break;
        }
        return total;
    }
    
  
    /// <summary>
    /// 获取指定的任务
    /// </summary>
    /// <param name="index"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public MissionObject GetMissionByIndex(int index, LevelType type)
    {
        MissionObject m = null;
        switch (type)
        {
            case LevelType.MainTask:
                if (index < CommonMission.Count)
                    m = CommonMission[index];
                break;
            default:
                break;
        }
        return m;
    }

    #endregion
}
