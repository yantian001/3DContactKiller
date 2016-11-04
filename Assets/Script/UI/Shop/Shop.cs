using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameDataEditor;
using System.Collections.Generic;
public class Shop : MonoBehaviour
{


    public Toggle tgPower;
    public Toggle tgStability;
    public Toggle tgMaxZoom;
    public Toggle tgCapacity;
    public Button buyBtn;
    public Text txtBuyPrice;
    public Button upgradeBtn;
    public Text txtUpgradePrice;
    public Button eqBtn;
    public GameObject markCrt;


    public AudioClip succedAudio;
    RectTransform rect;




    Button leftBtn;
    Button rightBtn;



    GDEWeaponData weapon;
    GDEWeaponAttributeData powerAttr;
    GDEWeaponAttributeData stabilityAttr;
    GDEWeaponAttributeData maxZoomAttr;
    GDEWeaponAttributeData capacityAttr;
    GDEWeaponAttributeData infraredAttr;



    List<string> gunlist;

    int currentAttr = 0;

    int currentGun = 0;

    int MaxGun = 0;


    GameObject[] weapons;

    Text costTxt;

    Vector3 selectTmpPosition;
    // Use this for initialization
    void Start()
    {


        GDEDataManager.Init("gde_data");
        rect = GetComponent<RectTransform>();
        FunctionBtnsetUp();

        weapons = new GameObject[3];

        weapons[0] = GameObject.Find("Q-0").gameObject;

        weapons[1] = GameObject.Find("Q-1").gameObject;

        weapons[2] = GameObject.Find("Q-2").gameObject;



        GDEDataManager.GetAllDataKeysBySchema("Weapon", out gunlist);


        MaxGun = gunlist.Count;

        updateWeapon(WeaponManager.Instance.GetCurrentWeaponId());

    }

    void updateWeapon(int value)
    {


        //Debug.Log(value);
        while (value < 0 || value >= MaxGun)
        {
            if (value < 0)
                value += MaxGun;
            if (value >= MaxGun)
                value -= MaxGun;
        }
        currentAttr = -1;
        tgPower.isOn = false;
        tgMaxZoom.isOn = false;
        tgCapacity.isOn = false;
        tgStability.isOn = false;
        getWeapon(value);
        readGunData();
        updateBuyBtn();
        for (int i = 0; i < 3; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[value].SetActive(true);
    }

    GDEWeaponData getWeapon(int value)
    {
        //if (!GDEDataManager.DataDictionary.TryGetCustom(gunlist[value], out weapon))
        //{
        //    weapon = null;
        //}
        weapon = WeaponManager.Instance.GetWeaponById(value);
        return weapon;
    }


    void FunctionBtnsetUp()
    {
        // return;

        if (!leftBtn)
        {
            leftBtn = CommonUtils.GetChildComponent<Button>(rect, "Bottom/bg/ButtonPrev");

            leftBtn.onClick.AddListener(delegate () { this.OnClickBtnFunction("leftBtn"); });
        }

        if (!rightBtn)
        {
            rightBtn = CommonUtils.GetChildComponent<Button>(rect, "Bottom/bg/ButtonNext");

            rightBtn.onClick.AddListener(delegate () { this.OnClickBtnFunction("rightBtn"); });
        }

        tgPower.onValueChanged.AddListener((b) => { OnToggleFunction(b, 0); });
        tgMaxZoom.onValueChanged.AddListener((b) => { OnToggleFunction(b, 1); });
        tgStability.onValueChanged.AddListener((b) => { OnToggleFunction(b, 2); });
        tgCapacity.onValueChanged.AddListener((b) => { OnToggleFunction(b, 3); });
        upgradeBtn.onClick.AddListener(delegate () { this.OnClickBtnFunction("upgradeBtn"); });
        buyBtn.onClick.AddListener(delegate () { this.OnClickBtnFunction("BuyBtn"); });
        eqBtn.onClick.AddListener(delegate () { this.OnClickBtnFunction("BuyBtn"); });
    }


    void OnToggleFunction(bool isOn, int i)
    {
        if (isOn)
        {
            currentAttr = i;
        }
        else
        {
            currentAttr = -1;
        }
        readGunData();
    }

    void updateBuyBtn()
    {
        if (!buyBtn) return;

        if (!weapon.Owned)
        {
            ChangeButtonDisplay(1);
            txtBuyPrice.text = weapon.Price.ToString();
        }
        else
        {
            if (!weapon.Equipped)
            {
                ChangeButtonDisplay(2);
                markCrt.SetActive(false);
            }
            else
            {
                //if()
                ChangeButtonDisplay(0);
                markCrt.SetActive(true);

            }
        }

        //if (!weapon.Owned)
        //{
        //    upgradeBtn.gameObject.SetActive(false);
        //    costTxt.transform.parent.gameObject.SetActive(true);
        //    costTxt.text = weapon.Price.ToString();
        //    Text t = buyBtn.transform.FindChild("Text").GetComponent<Text>();
        //    t.text = "Buy";
        //    buyBtn.gameObject.SetActive(true);

        //}
        //else if (weapon.Owned && !weapon.Equipped)
        //{
        //    Text t = buyBtn.transform.FindChild("Text").GetComponent<Text>();
        //    t.text = "Equipment";
        //    // BuyBtn.interactable = true;
        //    buyBtn.gameObject.SetActive(true);
        //    costTxt.transform.parent.gameObject.SetActive(false);
        //    upgradeBtn.gameObject.SetActive(false);
        //}
        //else if (weapon.Equipped)
        //{
        //    buyBtn = CommonUtils.GetChildComponent<Button>(rect, "middle/task/BuyBtn");
        //    Text t = buyBtn.transform.FindChild("Text").GetComponent<Text>();
        //    t.text = "Equipment";
        //    buyBtn.gameObject.SetActive(false);
        //    //costTxt.transform.parent.gameObject.SetActive(false);
        //    costTxt.transform.parent.gameObject.SetActive(false);
        //    upgradeBtn.gameObject.SetActive(false);
        //}
    }

    /// <summary>
    /// 更新按钮显示
    /// </summary>
    /// <param name="i">按钮显示的序列
    /// 1:显示购买按钮
    /// 2:显示装备按钮
    /// 3:显示升级按钮
    /// 0:都不显示
    /// </param>
    void ChangeButtonDisplay(int i)
    {
        if (i == 0)
        {
            buyBtn.gameObject.SetActive(false);
            eqBtn.gameObject.SetActive(false);
            upgradeBtn.gameObject.SetActive(false);
        }
        else if (i == 1)
        {
            buyBtn.gameObject.SetActive(true);
            eqBtn.gameObject.SetActive(false);
            upgradeBtn.gameObject.SetActive(false);
        }
        else if (i == 2)
        {
            buyBtn.gameObject.SetActive(false);
            eqBtn.gameObject.SetActive(true);
            upgradeBtn.gameObject.SetActive(false);
        }
        else if (i == 3)
        {
            buyBtn.gameObject.SetActive(false);
            eqBtn.gameObject.SetActive(false);
            upgradeBtn.gameObject.SetActive(true);
        }
    }

    void readGunData()
    {

        //设置枪的名字
        CommonUtils.SetChildText(rect, "PropertyPanel/txtTitle", weapon.Name.ToUpper());
        CommonUtils.SetChildText(rect, "Bottom/bg/ChapterName", weapon.Name.ToUpper());
        //设置power

        powerAttr = weapon.GetAttributeById(0);
        CommonUtils.SetChildText(rect, "PropertyPanel/Damage/Background/txtPropertyValue", powerAttr.CurrentValue.ToString());
        CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Damage/Background/SliderBg/Fill", powerAttr.CurrentValue / WeaponManager.MaxPower);

        if (currentAttr == 0 && !powerAttr.IsMaxLevel())
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Damage/Background/txtPropertyValueAdd", true);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Damage/Background/SliderBg/FillUpgrade", true);
            CommonUtils.SetChildText(rect, "PropertyPanel/Damage/Background/txtPropertyValueAdd", "+" + powerAttr.GetNextIncreaseValue().ToString());
            CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Damage/Background/SliderBg/FillUpgrade", (powerAttr.CurrentValue + powerAttr.GetNextIncreaseValue()) / WeaponManager.MaxPower);

        }
        else
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Damage/Background/txtPropertyValueAdd", false);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Damage/Background/SliderBg/FillUpgrade", false);
        }
        //设置 Stability

        stabilityAttr = weapon.GetAttributeById(2);
        CommonUtils.SetChildText(rect, "PropertyPanel/Stability/Background/txtPropertyValue", stabilityAttr.CurrentValue.ToString() + "%");
        CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Stability/Background/SliderBg/Fill", stabilityAttr.CurrentValue / WeaponManager.MaxStability);
        if (currentAttr == 2 && !stabilityAttr.IsMaxLevel())
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Stability/Background/txtPropertyValueAdd", true);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Stability/Background/SliderBg/FillUpgrade", true);
            CommonUtils.SetChildText(rect, "PropertyPanel/Stability/Background/txtPropertyValueAdd", "+" + stabilityAttr.GetNextIncreaseValue().ToString() + "%");
            CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Stability/Background/SliderBg/FillUpgrade", (stabilityAttr.CurrentValue + stabilityAttr.GetNextIncreaseValue()) / WeaponManager.MaxStability);

        }
        else
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Stability/Background/txtPropertyValueAdd", false);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Stability/Background/SliderBg/FillUpgrade", false);
        }




        ////设置 Infrared

        //CommonUtils.GetChildComponent<Text>(rect, "middle/task/attr3/value").color = Color.white;

        //GDEWeaponAttributeData infraredAttr = weapon.GetAttributeById(4);

        //CommonUtils.SetChildText(rect, "middle/task/attr3/value", infraredAttr.CurrentValue.ToString());

        //设置 MaxZoom

        maxZoomAttr = weapon.GetAttributeById(1);

        CommonUtils.SetChildText(rect, "PropertyPanel/MaxZoom/Background/txtPropertyValue", maxZoomAttr.CurrentValue.ToString());
        CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/MaxZoom/Background/SliderBg/Fill", maxZoomAttr.CurrentValue / WeaponManager.MaxZoom);
        if (currentAttr == 1 && !maxZoomAttr.IsMaxLevel())
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/MaxZoom/Background/txtPropertyValueAdd", true);
            CommonUtils.SetChildActive(rect, "PropertyPanel/MaxZoom/Background/SliderBg/FillUpgrade", true);
            CommonUtils.SetChildText(rect, "PropertyPanel/MaxZoom/Background/txtPropertyValueAdd", "+" + maxZoomAttr.GetNextIncreaseValue().ToString());
            CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/MaxZoom/Background/SliderBg/FillUpgrade", (maxZoomAttr.CurrentValue + maxZoomAttr.GetNextIncreaseValue()) / WeaponManager.MaxZoom);

        }
        else
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/MaxZoom/Background/txtPropertyValueAdd", false);
            CommonUtils.SetChildActive(rect, "PropertyPanel/MaxZoom/Background/SliderBg/FillUpgrade", false);
        }

        //设置 Capacity	
        capacityAttr = weapon.GetAttributeById(3);
        CommonUtils.SetChildText(rect, "PropertyPanel/Capacity/Background/txtPropertyValue", capacityAttr.CurrentValue.ToString());
        CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Capacity/Background/SliderBg/Fill", capacityAttr.CurrentValue / WeaponManager.MaxCapacity);
        if (currentAttr == 3 && !capacityAttr.IsMaxLevel())
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Capacity/Background/txtPropertyValueAdd", true);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Capacity/Background/SliderBg/FillUpgrade", true);
            CommonUtils.SetChildText(rect, "PropertyPanel/Capacity/Background/txtPropertyValueAdd", "+" + capacityAttr.GetNextIncreaseValue().ToString());
            CommonUtils.SetChildImageSliderValue(rect, "PropertyPanel/Capacity/Background/SliderBg/FillUpgrade", (capacityAttr.CurrentValue + capacityAttr.GetNextIncreaseValue()) / WeaponManager.MaxCapacity);

        }
        else
        {
            CommonUtils.SetChildActive(rect, "PropertyPanel/Capacity/Background/txtPropertyValueAdd", false);
            CommonUtils.SetChildActive(rect, "PropertyPanel/Capacity/Background/SliderBg/FillUpgrade", false);
        }


        if (currentAttr != -1)
        {
            if (weapon.Equipped)
            {
                //markCrt.SetActive(true);
                GDEWeaponAttributeData currentAttrdata = weapon.GetAttributeById(currentAttr);
                if (currentAttrdata != null)
                {
                    if (currentAttrdata.IsMaxLevel())
                    {
                        //upgradeBtn.gameObject.SetActive(false);
                        //costTxt.transform.parent.gameObject.SetActive(false);
                        ChangeButtonDisplay(0);
                    }
                    else
                    {
                        //upgradeBtn.gameObject.SetActive(true);
                        //costTxt.transform.parent.gameObject.SetActive(true);
                        //costTxt.text = currentAttrdata.GetUpgradeCost().ToString();
                        ChangeButtonDisplay(3);
                        txtUpgradePrice.text = currentAttrdata.GetUpgradeCost().ToString();
                    }
                }
            }
            //else
            //{
            //    markCrt.SetActive(false);
            //}

        }



    }


    public void OnClickBtnFunction(string obj)
    {
        switch (obj)
        {
            case "upgradeBtn":
                {
                    if (weapon.WeaponAttributes[currentAttr].CanUpgrade)
                    {
                        if (Player.CurrentUser.Money >= weapon.GetAttributeById(currentAttr).GetUpgradeCost())
                        {
                            Player.CurrentUser.UseMoney(weapon.GetAttributeById(currentAttr).GetUpgradeCost());
                            //weapon.WeaponAttributes[currentAttr].CurrentValue += weapon.WeaponAttributes[currentAttr].LevelsInfo[0].IncreaseValue;
                            weapon.GetAttributeById(currentAttr).CurrentLevel += 1;
                            readGunData();
                            if (succedAudio)
                            {
                                LeanAudio.play(succedAudio);
                            }
                        }
                    }
                }
                break;
            case "BuyBtn":
                {
                    onBuy();
                }
                break;

            case "leftBtn":
                Debug.Log("leftBtn");
                currentGun = currentGun - 1;
                updateWeapon(currentGun);
                break;
            case "rightBtn":
                currentGun = currentGun + 1;
                updateWeapon(currentGun);
                break;
        }

    }
    public void onBuy()
    {
        if (!weapon.Owned)
        {
            //买
            if (Player.CurrentUser.Money >= ConvertUtil.ToInt32(weapon.Price))
            {
                Player.CurrentUser.UseMoney(ConvertUtil.ToInt32(weapon.Price));
                weapon.Owned = true;
                if (succedAudio)
                {
                    LeanAudio.play(succedAudio);
                }
            }
        }
        else
        {
            //装备
            WeaponManager.Instance.EqWeaon(weapon.Id);
            //weapon.Equipped = true;

        }
        updateBuyBtn();
    }


}
