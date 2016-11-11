using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using FUGSDK;

public class UnityAdsUtil : MonoBehaviour
{
    private static UnityAdsUtil _instance = null;
    public static UnityAdsUtil Instance
    {
        private set
        {
            _instance = value;
        }
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UnityAdsUtil>();
                if (_instance == null)
                {
                    GameObject o = new GameObject("UnityAdsContainer");
                    _instance = o.AddComponent<UnityAdsUtil>();
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        if (Advertisement.isInitialized)
        {

        }
        else
        {
            Debug.Log("isInitialized = false");
        }
    }
    /// <summary>
    /// 是否有插页广告
    /// </summary>
    /// <returns></returns>
    public bool HasInterstital()
    {
        if (Advertisement.IsReady())
            return true;
        return false;
    }
    /// <summary>
    /// 显示插页广告
    /// </summary>
    public void ShowInterstital()
    {
        if (HasInterstital())
        {
            Advertisement.Show();
        }
    }
    /// <summary>
    /// 是否有视频奖励广告
    /// </summary>
    /// <returns></returns>
    public bool HasRewardVedio()
    {
        if (Advertisement.IsReady("rewardedVideo"))
            return true;
        return false;
    }

    /// <summary>
    /// 显示视屏奖励广告
    /// </summary>
    /// <param name="ev"></param>
    public bool ShowRewardVedio(RewardVedioClosedEvent ev)
    {
        if (HasRewardVedio())
        {
            var opt = new ShowOptions()
            {
                resultCallback = (rst) =>
                {
                    switch (rst)
                    {
                        case ShowResult.Failed:
                        case ShowResult.Skipped:
                            if (ev != null) ev(false);
                            break;
                        case ShowResult.Finished:
                            if (ev != null) ev(true);
                            break;
                    }
                }
            };
            Advertisement.Show("rewardedVideo", opt);
            return true;
        }
        else
            return false;
    }
}
