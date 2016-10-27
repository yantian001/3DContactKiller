using UnityEngine;
using System.Collections;

public class GameFinishTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DelayCall", 5f);
	}
	
    void DelayCall()
    {
        GameRecords gr = new GameRecords();
        gr.MissionReward = 320;
        gr.HeadShotCount = 3;
        gr.FinishType = GameFinishType.TimeUp;
        LeanTween.dispatchEvent((int)Events.GAMEFINISH, gr);
    }
}
