using UnityEngine;
using System.Collections;

public class PredictionObject : MonoBehaviour
{

    public bool marked = false;

    public Texture2D targetTexture;
    public Texture2D normalTexture;
    public float hight = 0;
    public void OnGUI()
    {
        if (marked)
        {
            Texture2D texture = GameValue.IsTarget(transform) ? targetTexture : normalTexture;
          
            Vector3 v3 = Camera.main.WorldToScreenPoint(transform.position);
            //float hight = Screen.height - v3.y;
           
            Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
            //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
            Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
            //得到真实NPC头顶的2D坐标
            position = new Vector2(position.x, Screen.height - position.y);
            Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(texture));
           // Debug.Log(bloodSize);
            //通过血值计算红色血条显示区域
            int blood_width = texture.width;
            //在绘制红色血条
            GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, blood_width, bloodSize.y), texture);
        }
    }
}
