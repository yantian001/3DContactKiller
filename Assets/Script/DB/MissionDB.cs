using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionDB : MonoBehaviour
{
    
    public List<Chapter> chapterList = new List<Chapter>();


    public static MissionDB LoadDB()
    {
        GameObject obj = Resources.Load("Missions/MissionDB") as GameObject;

        return obj.GetComponent<MissionDB>();
    }

    public static List<Chapter> Load()
    {
        return LoadDB().chapterList;
    }
}
