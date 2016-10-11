using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class Chapter
{
    public int Id = 0;
    public string Name = "";
    public string SceneName = "";
    public SceneAsset Scene;
    public List<MissionObject> CommonMission = new List<MissionObject>();

}
