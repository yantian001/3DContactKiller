using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour
{

    public IMission mission;
    public Material greyMaterial;
    public Text txtName;
    public RawImage image;
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
            }
            else
            {
                image.material = null;
            }
        }
    }
}
