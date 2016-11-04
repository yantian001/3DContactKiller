using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    public RectTransform ingame;
    public RectTransform finish;
    public RectTransform success;
    public RectTransform fail;
    public RectTransform screen;

    public float scaleInTime = 0.5f;

    Vector3 localScale;
    Vector2 tempPosition;

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);
    }

    public void Start()
    {
        if (finish)
        {
            // finish.transform.position = finish.transform.position + new Vector3(Screen.width, 0f, 0f);
            tempPosition = finish.anchoredPosition;
            //localScale = finish.
        }
    }

    void OnGameFinish(LTEvent evt)
    {
        if (evt.data == null || finish == null)
            return;

        GameRecords record = evt.data as GameRecords;
        if (record == null)
            return;
        if (ingame)
            ingame.gameObject.SetActive(false);
        finish.anchoredPosition = Vector2.zero;
        LeanTween.moveX(screen, -140, scaleInTime).setOnComplete(() =>
        {
            SetDisplay(record);

        });
        //var transformComplete = finish.FindChild("Complete");
        //localScale = transformComplete.localScale;
        //transformComplete.localScale = Vector3.zero;
        //LeanTween.scale(transformComplete.gameObject, localScale, scaleInTime);
    }

    IEnumerator DynamicDisplayMoeny(Text text, int to, System.Action onFinish, int from = 0, int time = 2)
    {
        if (text != null)
        {
            int diff = to - from;
            //int normal = Mathf.CeilToInt((float)diff / time);
            while (from != to)
            {
                if (Mathf.Abs(to - from) >= time)
                {
                    from += time;
                }
                else
                    from = to;

                text.text = from.ToString();

                //yield return new  WaitForSeconds(0.05f);
                yield return null;
            }
        }
        if (onFinish != null)
            onFinish();
    }


    void SetDisplay(GameRecords record)
    {

        if (record.FinishType == GameFinishType.Completed)
        {
            success.gameObject.SetActive(true);
            fail.gameObject.SetActive(false);
            int reward = (int)record.MissionReward;
            int heatshot = record.HeadShotCount * 10;
            int total = reward + heatshot;

            //显示任务奖励
            CommonUtils.SetChildActive(success, "MissionRewards", true);
            Text txtReward = CommonUtils.GetChildComponent<Text>(success, "MissionRewards/Value");
            StartCoroutine(DynamicDisplayMoeny(txtReward, reward, () =>
            {
                //显示爆头奖励
                CommonUtils.SetChildActive(success, "Headshots", true);
                Text txtHeadshot = CommonUtils.GetChildComponent<Text>(success, "Headshots/Value");
                StartCoroutine(DynamicDisplayMoeny(txtHeadshot, heatshot, () =>
                {
                    //显示总计
                    CommonUtils.SetChildActive(success, "Total", true);
                    Text txtTotal = CommonUtils.GetChildComponent<Text>(success, "Total/Value");
                    StartCoroutine(DynamicDisplayMoeny(txtTotal, total, () =>
                    {
                        //按钮启用
                        CommonUtils.SetChildButtonActive(success, "ButtonContinue", true);
                    }));
                }));

            }));

            //CommonUtils.SetChildText(finish, "Complete/Title", "YOU WIN");
            //CommonUtils.SetChildActive(finish, "Complete/ButtonReplay", false);
        }
        else
        {
            success.gameObject.SetActive(false);
            fail.gameObject.SetActive(true);
            CommonUtils.SetChildText(fail, "MissionDes", MissionManager.CurrentMission.GetDescription());
            if (record.FinishType == GameFinishType.Failed)
            {
                CommonUtils.SetChildActive(fail, "Lost", true);
                CommonUtils.SetChildActive(fail, "Timeout", false);
            }
            else if (record.FinishType == GameFinishType.TimeUp)
            {
                CommonUtils.SetChildActive(fail, "Lost", false);
                CommonUtils.SetChildActive(fail, "Timeout", true);
            }
        }
        //else if (record.FinishType == GameFinishType.TimeUp)
        //{
        //    CommonUtils.SetChildText(finish, "Complete/Title", "TIME UP");
        //    CommonUtils.SetChildActive(finish, "Complete/ButtonNext", false);
        //    //  CommonUtils.SetChildActive(finish, "Complete/ButtonShare", false);
        //}

        //CommonUtils.SetChildText(finish, "Complete/Scores/HeadShotTitle/Count", record.HeadShotCount.ToString());
        //CommonUtils.SetChildText(finish, "Complete/Scores/KillEnemy/Count", record.EnemyKills.ToString());
        //CommonUtils.SetChildText(finish, "Complete/Scores/TimeLeft/Count", record.TimeLeft.ToString());

    }
}
