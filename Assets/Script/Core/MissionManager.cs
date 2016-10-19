using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    void Init()
    {
        lstChapters = MissionDB.Load();
        CurrentMission = lstChapters[0].CommonMission[0];
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
