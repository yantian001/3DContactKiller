using UnityEngine;
using FUGSDK;

public class ChartboostUtil : MonoBehaviour
{

    static ChartboostUtil _instance = null;



    public static ChartboostUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ChartboostUtil>();
                if (_instance == null)
                {
                    GameObject signton = new GameObject();
                    signton.name = "Chartboost Container";
                    _instance = signton.AddComponent<ChartboostUtil>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
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

    private void OnGameMore(LTEvent obj)
    {
        //  throw new NotImplementedException();
        //this.ShowMoreAppOnDefault();
        Ads.Instance.ShowMoreApp();
    }

    private void OnGameMenu(LTEvent obj)
    {
        //throw new NotImplementedException();
        //this.ShowInterstitialOnHomescreen();
        Ads.Instance.ShowInterstitial();
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
}
