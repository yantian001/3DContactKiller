using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[System.Serializable]
public class MenuButtonClicked : UnityEvent<LevelType>
{
    public MenuButtonClicked()
    { }
}

[RequireComponent(typeof(RawImage))]
public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // public EventTrigger trigger;
    /// <summary>
    /// 是否启用
    /// </summary>
    [SerializeField]
    private bool _interactable = true;
    public bool Interactable
    {
        get
        {
            return _interactable;
        }
        set
        {
            _interactable = value;
            if (!_interactable)
            {
                Selected = false;
                if (graphic && disableImage)
                {
                    graphic.texture = disableImage;
                }
            }
        }
    }
    /// <summary>
    /// 是否选中
    /// </summary>
    [SerializeField]
    private bool _selected = false;

    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            if (_selected == value)
                return;
            _selected = value;
            if (graphic)
            {
                if (_selected && selectImage)
                {
                    graphic.texture = selectImage;
                    graphic.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
                else
                {
                    if (normalImage)
                    {
                        graphic.texture = normalImage;
                        graphic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }
            }
        }
    }

    public RawImage graphic;
    public Texture2D normalImage;
    public Texture2D disableImage;
    public Texture2D selectImage;
    public LevelType type;
    public MenuButtonClicked OnClick;



    // Use this for initialization
    void Start()
    {

        if (!graphic)
        {
            graphic = GetComponent<RawImage>();
        }
        if (!_interactable)
        {
            graphic.texture = disableImage;

        }
        else
        {
            if (Selected)
            {
                graphic.texture = selectImage;
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                graphic.texture = normalImage;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // print("click");
        //throw new NotImplementedException();
        if (!_interactable)
        {
            return;
        }

        if (!Selected)
        {
            Selected = true;
            OnClick.Invoke(type);
            var mbs = transform.parent.GetComponentsInChildren<MenuButton>();
            for (int i = 0; i < mbs.Length; i++)
            {
                if (mbs[i].transform != transform)
                {
                    if (mbs[i].transform.parent == transform.parent)
                    {
                        mbs[i].Selected = false;
                    }
                }
            }
        }
    }

    Vector3 tempScale;
    public void OnPointerDown(PointerEventData eventData)
    {
        //  throw new NotImplementedException();
        // print("down");
        if (!_interactable)
            return;
        tempScale = transform.localScale;
        transform.localScale = tempScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        //   print("up");
        if (!_interactable)
            return;
        transform.localScale = tempScale;
    }

    public void OnValidate()
    {
        if (!_interactable)
            Selected = false;
    }
}
