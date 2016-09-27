using UnityEngine;
using System.Collections;
using System;

public class GunHanddle : MonoBehaviour
{
    public Camera FPScamera;
    public Gun[] Guns;
    public int GunIndex = -1;
    [HideInInspector]
    public Gun CurrentGun;

    bool isFirstFire = false;

    #region 为了预测瞄准位置
    /// <summary>
    /// 当前武器的子弹
    /// </summary>
    public AS_Bullet currentBullet;
    /// <summary>
    /// 当前武器的rigidbody
    /// </summary>
    public Rigidbody currentBulletRgbody;

    public Camera currentCamera;
    #endregion

    void OnEnable()
    {
        if (Guns.Length < 1)
        {
            Guns = this.gameObject.GetComponentsInChildren<Gun>();
        }
        for (int i = 0; i < Guns.Length; i++)
        {
            if (FPScamera)
                Guns[i].NormalCamera = FPScamera;
            Guns[i].fovTemp = FPScamera.fieldOfView;
            if (Guns[i].positionTemp == Vector3.zero)
            {
                Guns[i].positionTemp = Guns[i].transform.localPosition;
            }
        }
        //SwitchGun();

    }

    private void OnPreviewStart(LTEvent obj)
    {
        // throw new NotImplementedException();
        SwitchGun();
    }

    public void Awake()
    {
        LeanTween.addListener((int)Events.GAMEFINISH, OnGameFinish);
        LeanTween.addListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.GAMEFINISH, OnGameFinish);
        LeanTween.removeListener((int)Events.PREVIEWSTART, OnPreviewStart);
    }


    void OnGameFinish(LTEvent evt)
    {
        //if (evt.data != null)
        //{
        //    GameRecords record = evt.data as GameRecords;
        //    if (record != null)
        //    {
        //        if (record.FinishType == GameFinishType.Failed)
        //        {
        //            //hide gun
        //            for(int i= 0;i<Guns.Length;i++)
        //            {
        //                Guns[GunIndex].SetActive(false);
        //            }
        //        }
        //    }
        //}
        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[GunIndex].SetActive(false);
        }
    }

    void Hide(GameObject gameObject, bool show)
    {
        /*Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
    	foreach (Renderer r in renderers) {
        	r.enabled = show;
    	}*/
    }

    public void Zoom()
    {
        if (CurrentGun)
        {
            CurrentGun.Zoom();
        }
    }
    public void ZoomToggle()
    {
        if (CurrentGun)
            CurrentGun.ZoomToggle();
    }

    public void Reload()
    {
        if (CurrentGun)
            CurrentGun.Reload();
    }

    public void ZoomAdjust(int delta)
    {
        if (CurrentGun)
            CurrentGun.ZoomDelta(delta);
    }
    public void OffsetAdjust(Vector2 delta)
    {
        if (CurrentGun)
            CurrentGun.OffsetAdjust(delta);
    }
    public void SwitchGun(int index)
    {
        //if (FPScamera.enabled)
        //  {
        for (int i = 0; i < Guns.Length; i++)
        {
            Hide(Guns[i].gameObject, false);
            Guns[i].SetActive(false);
        }
        if (Guns.Length > 0 && index < Guns.Length && index >= 0)
        {
            GunIndex = index;
            CurrentGun = Guns[GunIndex].gameObject.GetComponent<Gun>();
            currentBullet = CurrentGun.Bullets.GetComponent<AS_Bullet>();
            currentBulletRgbody = CurrentGun.Bullets.GetComponent<Rigidbody>();
            Hide(Guns[GunIndex].gameObject, true);
            Guns[GunIndex].SetActive(true);
        }
        //    }
    }

    public void SwitchGun()
    {

        int currentId = WeaponManager.Instance.GetCurrentWeaponId();
        if (currentId == -1)
        {
            SwitchGun(0);

        }
        else
        {
            int select = -1;
            for (int i = 0; i < Guns.Length; i++)
            {
                if (Guns[i].id == currentId)
                {
                    select = i;
                    break;
                }
            }
            if (select == -1)
                SwitchGun(0);
            else
                SwitchGun(select);
        }
    }

    public void Shoot()
    {
        if (CurrentGun)
        {
            CurrentGun.Shoot();
            isFirstFire = true;
            if (isFirstFire)
            {
                BehaviorDesigner.Runtime.GlobalVariables.Instance.SetVariableValue("Fired", true);
                LeanTween.dispatchEvent((int)Events.FIRED);
            }
        }
    }

    public void HoldBreath(int noiseMult)
    {
        if (CurrentGun)
            CurrentGun.FPSmotor.Holdbreath(noiseMult);
    }
    /// <summary>
    /// 是否打开狙击镜
    /// </summary>
    /// <returns></returns>
    public bool Zoomed()
    {
        if (CurrentGun && CurrentGun.Zooming)
            return true;
        return false;
    }
}
