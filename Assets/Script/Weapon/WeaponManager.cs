using UnityEngine;
using GameDataEditor;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{


    #region Staic Variable

    public static float MaxPower = 400;
    public static float MaxStability = 100;
    public static float MaxZoom = 20;
    public static float MaxCapacity = 10;

    #endregion

    #region 单例
    private static WeaponManager _instance;
    public static WeaponManager Instance
    {
        private set
        {
            _instance = value;
        }
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("WeaponManagerContainer");
                    _instance = obj.AddComponent<WeaponManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    List<GDEWeaponData> weapons;

    GDEWeaponData currentWeapon;

    #region Monobehavior
    void Awake()
    {

        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
            Init();
            //BuyWeapon(0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Method

    void Init()
    {
        weapons = new List<GDEWeaponData>();
        if (GDEDataManager.Init("gde_data"))
        {
            List<string> wsKeys;
            
            GDEDataManager.GetAllDataKeysBySchema("Weapon", out wsKeys);

            for (int i = 0; i < wsKeys.Count; i++)
            {
                GDEWeaponData wd = null;
                GDEDataManager.DataDictionary.TryGetCustom(wsKeys[i], out wd);

                if (wd != null)
                {
                   // wd.ResetAll();
                    weapons.Add(wd);
                    if (wd.Equipped)
                        currentWeapon = wd;
                }
            }
        }
    }

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="id"></param>
    public void EqWeaon(int id)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Id != id)
            {
                weapons[i].Equipped = false;
            }
            else
            {
                weapons[i].Equipped = true;
                currentWeapon = weapons[i];
            }
        }
    }

    /// <summary>
    /// 购买武器
    /// </summary>
    /// <param name="id">武器id</param>
    /// <returns>返回是否购买成功</returns>
    public bool BuyWeapon(int id)
    {

        GDEWeaponData w = weapons.Find((p) => { return p.Id == id; });
        if (w != null)
        {
            if (w.Owned)
            {
                return true;
            }
            if (Player.CurrentUser.IsMoneyEnough(ConvertUtil.ToInt32(w.Price, 1000000)))
            {
                Player.CurrentUser.UseMoney(ConvertUtil.ToInt32(w.Price, 1000000));
                w.Owned = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region 判断是否达到要求
    /// <summary>
    /// 判断武器属性是否达到要求 
    /// </summary>
    /// <param name="attrid">属性值id</param>
    /// <param name="val">要求属性</param>
    /// <returns></returns>
    public bool IsWeaponMeetReq(int attrid, float val)
    {
        if (currentWeapon == null)
        {
            return false;
        }
        return currentWeapon.GetAttributeCurrentVal(attrid) >= val;
    }

    #endregion

    /// <summary>
    /// 获取当前武器ID
    /// </summary>
    /// <returns></returns>
    public int GetCurrentWeaponId()
    {
        if (currentWeapon != null)
        {
            return currentWeapon.Id;
        }
        else
            return -1;
    }

    /// <summary>
    /// 通过ID 获取武器
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GDEWeaponData GetWeaponById(int id)
    {
        GDEWeaponData wd = weapons.Find(p => { return p.Id == id; });
        return wd;
    }
}
