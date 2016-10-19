using UnityEngine;
using System.Collections;

public class MissionControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MissionManager.CurrentMission.OnInit();
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
}
