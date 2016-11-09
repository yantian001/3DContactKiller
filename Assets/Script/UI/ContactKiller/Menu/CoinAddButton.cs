using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CoinAddButton : MonoBehaviour {

    public Button addButton;

	// Use this for initialization
	void Start () {
        if (!addButton)
            addButton = GetComponent<Button>();
        addButton.onClick.AddListener(OnClicked);
	}

    private void OnClicked()
    {
        // throw new NotImplementedException();
        LeanTween.dispatchEvent((int)Events.MONEYUSED, -1000);
    }

    
}
