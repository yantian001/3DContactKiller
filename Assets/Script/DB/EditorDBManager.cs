using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorDBManager : EditorWindow
{

    public static bool init = false;
    public static void Init()
    {
        if (init) return;
        LoadMission();
        //init = true;
    }

    private static MissionDB missionPrefabDB;
    private static List<Chapter> chapterList = new List<Chapter>();
    private static List<int> chapterIdList = new List<int>();
    private static void LoadMission()
    {
        missionPrefabDB = MissionDB.LoadDB();
        chapterList = missionPrefabDB.chapterList;
        for (int i = 0; i < chapterList.Count; i++)
        {
            chapterIdList.Add(chapterList[i].Id);
        }
    }

    public static List<Chapter> GetChapterList() { return chapterList; }
    public static void SetDirtyMission() { EditorUtility.SetDirty(missionPrefabDB); }

    public static void AddNewChapter(string name)
    {
        Chapter newMission = new Chapter();
        newMission.Id = GenerateNewID(chapterIdList);
        newMission.Name = name;
        chapterList.Add(newMission);
        SetDirtyMission();
    }

    public static void RemoveChapter(int id)
    {
        chapterIdList.Remove(chapterList[id].Id);
        chapterList.RemoveAt(id);
        SetDirtyMission();
    }

    public static void AddNewCommonMission(int chapterId)
    {
        Chapter c = chapterList[chapterId];
        MissionObject mo = new MissionObject();
        c.CommonMission.Add(mo);
        SetDirtyMission();
    }

    public static int GenerateNewID(List<int> list)
    {
        int ID = 0;
        while (list.Contains(ID)) ID += 1;
        return ID;
    }
}
