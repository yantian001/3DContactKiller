﻿using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour
{
    public int Id;
    public string Name;

    public bool isTarget = true;

    public void OnDestroy()
    {
        LeanTween.dispatchEvent((int)Events.ENEMYAWAY);
    }
}
