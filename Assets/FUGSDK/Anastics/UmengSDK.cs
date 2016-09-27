using UnityEngine;
using System.Collections;
using Umeng;
using System;

public class UmengSDK : MonoBehaviour
{

    public string appid_android = "";
    public string appScret_android = "";

    public string appid_ios = "";
    public string appscret_ios = "";

    private static UmengSDK _instance = null;

    public static UmengSDK Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UmengSDK>();
                if (_instance == null)
                {
                    GameObject o = new GameObject("Umeng Container");
                    _instance = o.AddComponent<UmengSDK>();
                }
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitUmeng();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void InitUmeng()
    {
#if UNITY_ANDROID
        //throw new NotImplementedException();
        //UMeng统计
        GA.StartWithAppKeyAndChannelId(appid_android, "Google Play");

        //Umeng 推送
        //    UMPushAndroid.enable();
        //  UMPushAndroid.onAppStart();

#elif UNITY_IOS
#endif
    }
    
}
