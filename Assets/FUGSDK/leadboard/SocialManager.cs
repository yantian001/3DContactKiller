using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;

using System.IO;

namespace FUGSDK.Leadbord
{
    public class SocialManager : MonoBehaviour
    {
        /// <summary>
        /// 自动登陆
        /// </summary>
        [Tooltip("自动登陆,默认为true")]
        public bool autoLogin = true;
        /// <summary>
        /// 是否已经登陆
        /// </summary>
        bool isLogined = false;

        #region 单列
        static SocialManager _instance = null;

        public static SocialManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (SocialManager)GameObject.FindObjectOfType<SocialManager>();
                    if (_instance == null)
                    {
                        GameObject o = new GameObject("SocialManager");
                        _instance = o.AddComponent<SocialManager>();
                    }
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        #endregion

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            if (autoLogin && !ISAuthenticated())
            {
                Authenticate(b =>
                {
                    isLogined = b;
                });
            }

        }
        
        /// <summary>
        /// 是否能正常联网
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkOk()
        {
            return !(Application.internetReachability == NetworkReachability.NotReachable);

        }

        /// <summary>
        /// 显示排行榜
        /// </summary>
        public void ShowLeardBoardUI()
        {
            Social.ShowLeaderboardUI();
        }
        void Init()
        {
#if UNITY_ANDROID
            GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = false;
            GooglePlayGames.PlayGamesPlatform.Activate();
#endif
#if UNITY_IPHONE
    // by defaults when player gets achievement nothing happens, so we call this function to show standard iOS popup when achievement is completed by user
    UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
        }


        #region functions
        /// <summary>
        /// 登陆
        /// </summary>
        public void Authenticate(System.Action<bool> onAuthComplete)
        {
            //if(!Application.)
            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate(onAuthComplete);
            }
            else
            {
                if (onAuthComplete != null)
                    onAuthComplete(true);
            }
        }

        public bool ISAuthenticated()
        {
            return Social.localUser.authenticated;
        }

        /// <summary>
        /// 获得我的排名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onComplete"></param>
        public void GetMyScore(string id, System.Action<bool, IScore> onComplete)
        {
            if (!Social.localUser.authenticated)
            {
                return;
            }
            ILeaderboard lb = Social.CreateLeaderboard();
            lb.id = id;
            lb.SetUserFilter(new string[] { Social.localUser.id });
            lb.LoadScores(b =>
            {
                onComplete(b, lb.localUserScore);
            });
        }

        /// <summary>
        /// 提交分数
        /// </summary>
        /// <param name="score"></param>
        /// <param name="id"></param>
        /// <param name="onComplete"></param>
        public void ReportScore(long score, string id = default(string), System.Action<bool> onComplete = null)
        {
            //if(!CommonUtils.IsNetworkOk() || !Social.localUser.authenticated)
            if (!IsNetworkOk() || !Social.localUser.authenticated)
            {
                if (onComplete != null)
                {
                    onComplete(false);
                }
            }
            else
            {
                if (id == default(string))
                {
                    //id = GPGSIds.;
                    return;
                }

                Debug.Log("Report to:" + id + " with score :" + score.ToString());
                Social.ReportScore(score, id, onComplete);
            }

        }

        /// <summary>
        /// 获取排行榜的排名
        /// </summary>
        /// <param name="id">排行榜ID</param>
        /// <param name="range">区间</param>
        /// <param name="onComplete">完成回调</param>
        /// <param name="scope">时间区间</param>
        /// <param name="userScope">用户区间</param>
        public void GetTopByByLeaderboardID(string id, Range range, System.Action<bool, IScore[]> onComplete, TimeScope scope = TimeScope.AllTime, UserScope userScope = UserScope.Global)
        {
            if (!Social.localUser.authenticated)
                return;
            ILeaderboard lb = Social.CreateLeaderboard();
            Debug.Log(lb);
            if (lb == null)
                return;
            lb.id = id;
            lb.range = range;
            lb.timeScope = scope;
            lb.userScope = userScope;
            lb.LoadScores(ok =>
           {
               onComplete(ok, lb.scores);
           });
        }

        #endregion

    }
}
