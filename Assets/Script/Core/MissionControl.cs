using UnityEngine;
using System.Collections;
using System;

public class MissionControl : MonoBehaviour
{
    public static MissionControl Instance = null;

    float timeLeft = 0;
    public void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        MissionManager.CurrentMission.OnInit();
        timeLeft = MissionManager.CurrentMission.totalTime;
        StartCoroutine(TimeRoute());
    }

    IEnumerator TimeRoute()
    {
        //throw new NotImplementedException();
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft -= 1;
        }

        OnTimeOut();
    }

    public float GetTime()
    {
        return timeLeft;
    }

    private void OnTimeOut()
    {
        //throw new NotImplementedException();
        LeanTween.dispatchEvent((int)Events.TIMEUP);
    }

    // Update is called once per frame
    void Update()
    {
        MissionManager.CurrentMission.OnUpdate(Time.deltaTime);
    }

    public void OnDisable()
    {
        MissionManager.CurrentMission.OnDestory();
    }

    public void OnDestroy()
    {
        Instance = null;
    }
}
