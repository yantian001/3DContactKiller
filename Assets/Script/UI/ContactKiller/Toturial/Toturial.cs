using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Toturial : MonoBehaviour
{

    public GameObject[] toturials;

    private bool isToturialed = false;
    private int currentIndex = 0;

    public void Awake()
    {
        isToturialed = PlayerPrefs.GetInt("toturial-ingame", 0) == 1;
    }

    // Use this for initialization
    void Start()
    {
        for(int i =0;i<toturials.Length;i++)
        {
            toturials[i].SetActive(false);
        }

        if (isToturialed)
        {
            CompletedToturial();
        }
        else
        {
            PopupToturial(currentIndex);
        }
    }

    void PopupToturial(int index)
    {
        for (int i = 0; i < toturials.Length; i++)
        {
            if (i != index)
                toturials[i].SetActive(false);
            else
                toturials[i].SetActive(true);
        }
    }



    void CompletedToturial()
    {
        isToturialed = true;
        PlayerPrefs.SetInt("toturial-ingame", 1);
        LeanTween.dispatchEvent((int)Events.TOTURIALED);
        gameObject.SetActive(false);
    }

    public static void OnPointerDown1(PointerEventData eventData)
    {
        // throw new NotImplementedException();
        print("dpwm");
    }


   public void OnPUp(BaseEventData bed)
    {
        currentIndex++;
        if (currentIndex < toturials.Length)
        {
            PopupToturial(currentIndex);
        }
        else
        {
            CompletedToturial();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        print("pointer Click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
    // Update is called once per frame

}
