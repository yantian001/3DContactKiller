using UnityEngine;
using System.Collections;
using System;

public class MissionControl : MonoBehaviour
{
    public static MissionControl Instance = null;

    public Animal[] animals;
    float timeLeft = 0;
    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.ENEMYAWAY, OnEnemyAway);
    }

    private void OnEnemyAway(LTEvent obj)
    {
        var am = obj.data as Animal;
        for (int i = 0; i < animals.Length; i++)
        {
            if (animals[i] == am)
            {
                animals[i] = null;
            }
        }
        //throw new NotImplementedException();
    }



    // Use this for initialization
    void Start()
    {
        if (animals.Length <= 0)
        {
            animals = FindObjectsOfType<Animal>() as Animal[];
            //for (int i = 0; i < animals.Length; i++)
            //{
            //    var dm = animals[i].gameObject.GetComponent<DamageManager>();
            //    //if (dm)
            //    //{
            //    //    dm.hp = (int)MissionManager.CurrentMission.powerRequired;
            //    //}
            //    //else
            //    //{
            //    //    Debug.LogError("Animal对象缺少DamageManager组件");
            //    //}
            //}
        }
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
            if (GameValue.staus == GameStatu.InGame)
            {
               
                timeLeft -= 1;
            }
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
        if (MissionManager.CurrentMission._statu == MissionStatu.Running)
        {
            MissionManager.CurrentMission.OnCheckFailure(animals);
            MissionManager.CurrentMission.OnUpdate(Time.deltaTime);
            if (MissionManager.CurrentMission._statu == MissionStatu.Completed)
            {
                GameManager.Instance.OnGameFinish(true);
            }
            else if (MissionManager.CurrentMission._statu == MissionStatu.Failed)
            {
                GameManager.Instance.OnGameFinish(false);
            }
        }

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
