﻿using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace FUGSDK
{
    public delegate void InterstitialClosedEvent();
    public delegate void RewardVedioClosedEvent(bool completed);



    public class Ads : MonoBehaviour
    {
        public string gpBannerId = "ca-app-pub-4204987182299137/5342806003";
        public string gpIntersititialId = "ca-app-pub-4204987182299137/4167124006";
        public string iosBannerId = "ca-app-pub-4204987182299137/2550790009";
        public string iosIntersititialId = "ca-app-pub-4204987182299137/4027523206";
        private static Ads _instance = null;
        public static Ads Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Ads>();
                    if (_instance == null)
                    {
                        GameObject o = new GameObject("AdsContainer");
                        _instance = o.AddComponent<Ads>();
                    }
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
                LeanTween.addListener((int)Events.GAMEPAUSE, OnGamePause);
                LeanTween.addListener((int)Events.MENULOADED, OnGameMenu);
                LeanTween.addListener((int)Events.GAMEMORE, OnGameMore);
                LeanTween.addListener((int)Events.WATCHVEDIO, OnWatchVedio);
            }
            else
            {
                Destroy(gameObject);
            }
        }



        // Use this for initialization
        void Start()
        {
            ChartboostUtil.Instance.Initialize();
            UnityAdsUtil.Instance.Initialize();
#if UNITY_ANDROID
            GoogleAdsUtil.Instance.Initialize(gpBannerId, gpIntersititialId);
#elif UNITY_IPHONE
            GoogleAdsUtil.Instance.Initialize(iosBannerId, iosIntersititialId);
#endif

        }
        /// <summary>
        /// 在位置p上显示横幅广告
        /// </summary>
        /// <param name="p">显示横幅的位置，默认是在中下</param>
        public void ShowBanner(AdPosition p = AdPosition.Bottom)
        {
            GoogleAdsUtil.Instance.ShowBanner(p, AdSize.SmartBanner);
        }


        #region 插页广告
        /// <summary>
        /// 显示插页广告，并在插页广告关闭后执行closeEvent方法，不需要判断是否有广告
        /// </summary>
        /// <param name="closeEvent">广告关闭时执行的方法，默认为null</param>
        public void ShowInterstitial(InterstitialClosedEvent closeEvent = null)
        {

            if (GoogleAdsUtil.Instance.HasInterstital())
            {
                GoogleAdsUtil.Instance.ShowInterstital(closeEvent);
            }
            else if (ChartboostUtil.Instance.HasInterstitialOnDefault())
            {
                ChartboostUtil.Instance.ShowInterstitialOnDefault(closeEvent);
            }
        }

        /// <summary>
        /// 是否有插页广告
        /// </summary>
        /// <returns></returns>
        public bool HasIntersititial()
        {
            return GoogleAdsUtil.Instance.HasInterstital() || ChartboostUtil.Instance.HasInterstitialOnDefault();
        }
        #endregion

        #region 视频奖励广告
        /// <summary>
        /// 是否有奖励广告
        /// </summary>
        /// <returns></returns>
        public bool HasRewardVedio()
        {
            return ChartboostUtil.Instance.HasGameOverVideo() || UnityAdsUtil.Instance.HasRewardVedio();
        }
        /// <summary>
        /// 播放视频奖励广告，并在广告播放完后执行ev方法，
        /// 
        /// </summary>
        /// <param name="ev"></param>
        public void ShowRewardVedio(RewardVedioClosedEvent ev)
        {

            if(!ChartboostUtil.Instance.ShowGameOverVideo(ev))
            {
                if(!UnityAdsUtil.Instance.ShowRewardVedio(ev))
                {
                    ev(false);
                }
            }
        }
        #endregion

        #region More Game

        /// <summary>
        /// 是否有“更多游戏”广告
        /// </summary>
        /// <returns></returns>
        public bool HasMoreApp()
        {
            return ChartboostUtil.Instance.HasMoreAppOnDefault();
        }


        /// <summary>
        /// 显示更多游戏
        /// </summary>
        public void ShowMoreApp()
        {
            ChartboostUtil.Instance.ShowMoreAppOnDefault();
        }
        #endregion

        private void OnGameMore(LTEvent obj)
        {
            //  throw new NotImplementedException();
            //this.ShowMoreAppOnDefault();
            Ads.Instance.ShowMoreApp();
            print("more");
        }

        private void OnGameMenu(LTEvent obj)
        {
            //throw new NotImplementedException();
            //this.ShowInterstitialOnHomescreen();
            //  Ads.Instance.ShowInterstitial();
        }

        private void OnGamePause(LTEvent obj)
        {
            //throw new NotImplementedException();
            // this.ShowInterstitialOnDefault();
            Ads.Instance.ShowInterstitial();
        }

        private void OnGameFinish(LTEvent obj)
        {
            //throw new NotImplementedException();
            // this.ShowInterstitialOnDefault();
            Ads.Instance.ShowInterstitial();
        }

        private void OnWatchVedio(LTEvent obj)
        {
            //throw new NotImplementedException();
            var evt = obj.data as RewardVedioClosedEvent;
            if (HasRewardVedio())
            {
                ShowRewardVedio(evt);
            }
            else
            {
                evt(false);
            }
        }

    }


}