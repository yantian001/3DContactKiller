using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Prediction : MonoBehaviour
{

    public Image slider;
    public Text txtName;
    public float interval = 0.1f;
    public float timeCount = 5f;

    float currentTimeCount = 0;
    float lastTimeStamb = -1;
    /// <summary>
    /// 上一次瞄准的对象
    /// </summary>
    Transform lastT, currentT;

    public RectTransform parent;

    public GunHanddle gunHanddle;
    // Use this for initialization
    void Start()
    {
        if (!gunHanddle)
        {
            gunHanddle = GameObject.FindGameObjectWithTag("Player").GetComponent<GunHanddle>();
        }
        //parent = GetComponent<RectTransform>();
        if (!slider)
        {
            slider = CommonUtils.GetChildComponent<Image>(parent, "Image/fill");
            slider.fillAmount = 0;
        }
        if (!txtName)
            txtName = CommonUtils.GetChildComponent<Text>(parent, "Name");
        PredictionTrajectory.ptEvent += OnPrediction;
        parent.gameObject.SetActive(false);

    }

    void OnPrediction(Transform t)
    {
        // Debug.Log("Aim at " + t.name);
        currentT = t;
        lastTimeStamb = Time.time;
    }



    public void Update()
    {
        if (lastT == null || lastT != currentT)
        {
            currentTimeCount = 0;
            slider.fillAmount = 0;
            lastT = currentT;
        }
        if (lastT == null || lastTimeStamb + interval < Time.time || !gunHanddle.Zoomed())
        {
            currentTimeCount = 0;
            slider.fillAmount = 0;
            parent.gameObject.SetActive(false);
        }
        else
        {
            var pto = lastT.GetComponent<PredictionObject>();
            if (pto)
            {
                parent.gameObject.SetActive(true);
                if (pto.marked)
                {
                    
                    txtName.gameObject.SetActive(true);
                    txtName.text = lastT.name;
                    CommonUtils.SetChildActive(parent, "Image", false);
                }
                else
                {
                    txtName.gameObject.SetActive(false);
                    CommonUtils.SetChildActive(parent, "Image", true);
                    currentTimeCount += Time.deltaTime;
                    float fillAmout = currentTimeCount / timeCount;
                    if(fillAmout >= 1) { pto.marked = true; }
                    slider.fillAmount = fillAmout;
                }
                
            }
        }
    }
}
