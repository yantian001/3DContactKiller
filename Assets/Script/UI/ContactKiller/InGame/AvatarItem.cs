using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour
{

    public IMission mission;
    public Material greyMaterial;
    public Text txtName;
    public RawImage image;
    public RectTransform cross;
    bool lastStau = false;
    // Use this for initialization
    void Start()
    {
        if (image == null)
        {
            image = GetComponent<RawImage>();
        }
        if (txtName == null)
        {
            txtName = GetComponent<Text>();
        }
        if (!cross)
        {
            cross = CommonUtils.GetChildComponent<RectTransform>(GetComponent<RectTransform>(), "Cross");
        }
        DisplayAvater();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayAvater();
    }

    void DisplayAvater()
    {
        if (mission != null && mission.Target.Avater)
        {
            image.texture = mission.Target.Avater;
            txtName.text = mission.Target.Name;

            if (mission.IsMissionComplete())
            {
                image.material = greyMaterial;
                if (lastStau == false)
                {
                    cross.gameObject.SetActive(true);// = true;
                    cross.localScale = new Vector3(2, 2, 2);
                    LeanTween.scale(cross, Vector3.one, .2f);
                    lastStau = true;
                }
            }
            else
            {
                image.material = null;
            }
        }
    }
}
