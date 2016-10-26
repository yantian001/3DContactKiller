using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Chapter
{
    public int Id = 0;
    public string Name = "";
    public string SceneName = "";
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

}
