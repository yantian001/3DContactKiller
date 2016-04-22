// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by the Game Data Editor.
//
//      Changes to this file will be lost if the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

using GameDataEditor;

namespace GameDataEditor
{
    public class GDEWeaponAttributeData : IGDEData
    {
        private static string CanUpgradeKey = "CanUpgrade";
        private bool _CanUpgrade;
        public bool CanUpgrade
        {
            get { return _CanUpgrade; }
            set
            {
                if (_CanUpgrade != value)
                {
                    _CanUpgrade = value;
                    GDEDataManager.SetBool(_key + "_" + CanUpgradeKey, _CanUpgrade);
                }
            }
        }

        private static string IdKey = "Id";
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    GDEDataManager.SetInt(_key + "_" + IdKey, _Id);
                }
            }
        }

        private static string CurrentLevelKey = "CurrentLevel";
        private int _CurrentLevel;
        public int CurrentLevel
        {
            get { return _CurrentLevel; }
            set
            {
                if (_CurrentLevel != value)
                {
                    _CurrentLevel = value;
                    GDEDataManager.SetInt(_key + "_" + CurrentLevelKey, _CurrentLevel);
                }
            }
        }

        private static string InitialValueKey = "InitialValue";
        private float _InitialValue;
        public float InitialValue
        {
            get { return _InitialValue; }
            set
            {
                if (_InitialValue != value)
                {
                    _InitialValue = value;
                    GDEDataManager.SetFloat(_key + "_" + InitialValueKey, _InitialValue);
                }
            }
        }

        private static string CurrentValueKey = "CurrentValue";
        private float _CurrentValue;
        public float CurrentValue
        {
            get
            {
                float ret = InitialValue;
                if (_CurrentLevel > 0)
                {
                    if(LevelsInfo != null && LevelsInfo.Count > 0)
                    {
                        for(int i=0;i<_CurrentLevel && i< LevelsInfo.Count;i++)
                        {
                            ret += LevelsInfo[i].IncreaseValue;
                        }
                    }
                }
                return ret;
            }
            set
            {
                if (_CurrentValue != value)
                {
                    _CurrentValue = value;
                    GDEDataManager.SetFloat(_key + "_" + CurrentValueKey, _CurrentValue);
                }
            }
        }

        private static string NameKey = "Name";
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    GDEDataManager.SetString(_key + "_" + NameKey, _Name);
                }
            }
        }

        private static string IconKey = "Icon";
        private Texture2D _Icon;
        public Texture2D Icon
        {
            get { return _Icon; }
            set
            {
                if (_Icon != value)
                {
                    _Icon = value;
                    GDEDataManager.SetTexture2D(_key + "_" + IconKey, _Icon);
                }
            }
        }

        private static string LevelsInfoKey = "LevelsInfo";
        public List<GDEWeaponLevelPropertyData> LevelsInfo;
        public void Set_LevelsInfo()
        {
            GDEDataManager.SetCustomList(_key + "_" + LevelsInfoKey, LevelsInfo);
        }


        public GDEWeaponAttributeData()
        {
            _key = string.Empty;
        }

        public GDEWeaponAttributeData(string key)
        {
            _key = key;
        }

        public override void LoadFromDict(string dataKey, Dictionary<string, object> dict)
        {
            _key = dataKey;

            if (dict == null)
                LoadFromSavedData(dataKey);
            else
            {
                dict.TryGetBool(CanUpgradeKey, out _CanUpgrade);
                dict.TryGetInt(IdKey, out _Id);
                dict.TryGetInt(CurrentLevelKey, out _CurrentLevel);
                dict.TryGetFloat(InitialValueKey, out _InitialValue);
                dict.TryGetFloat(CurrentValueKey, out _CurrentValue);
                dict.TryGetString(NameKey, out _Name);
                dict.TryGetTexture2D(IconKey, out _Icon);

                dict.TryGetCustomList(LevelsInfoKey, out LevelsInfo);
                LoadFromSavedData(dataKey);
            }
        }

        public override void LoadFromSavedData(string dataKey)
        {
            _key = dataKey;

            _CanUpgrade = GDEDataManager.GetBool(_key + "_" + CanUpgradeKey, _CanUpgrade);
            _Id = GDEDataManager.GetInt(_key + "_" + IdKey, _Id);
            _CurrentLevel = GDEDataManager.GetInt(_key + "_" + CurrentLevelKey, _CurrentLevel);
            _InitialValue = GDEDataManager.GetFloat(_key + "_" + InitialValueKey, _InitialValue);
            _CurrentValue = GDEDataManager.GetFloat(_key + "_" + CurrentValueKey, _CurrentValue);
            _Name = GDEDataManager.GetString(_key + "_" + NameKey, _Name);
            _Icon = GDEDataManager.GetTexture2D(_key + "_" + IconKey, _Icon);

            LevelsInfo = GDEDataManager.GetCustomList(_key + "_" + LevelsInfoKey, LevelsInfo);
        }

        public void Reset_CanUpgrade()
        {
            GDEDataManager.ResetToDefault(_key, CanUpgradeKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetBool(CanUpgradeKey, out _CanUpgrade);
        }

        public void Reset_Id()
        {
            GDEDataManager.ResetToDefault(_key, IdKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(IdKey, out _Id);
        }

        public void Reset_CurrentLevel()
        {
            GDEDataManager.ResetToDefault(_key, CurrentLevelKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetInt(CurrentLevelKey, out _CurrentLevel);
        }

        public void Reset_InitialValue()
        {
            GDEDataManager.ResetToDefault(_key, InitialValueKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetFloat(InitialValueKey, out _InitialValue);
        }

        public void Reset_CurrentValue()
        {
            GDEDataManager.ResetToDefault(_key, CurrentValueKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetFloat(CurrentValueKey, out _CurrentValue);
        }

        public void Reset_Name()
        {
            GDEDataManager.ResetToDefault(_key, NameKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetString(NameKey, out _Name);
        }

        public void Reset_Icon()
        {
            GDEDataManager.ResetToDefault(_key, IconKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            dict.TryGetTexture2D(IconKey, out _Icon);
        }

        public void Reset_LevelsInfo()
        {
            GDEDataManager.ResetToDefault(_key, LevelsInfoKey);

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);

            dict.TryGetCustomList(LevelsInfoKey, out LevelsInfo);
            LevelsInfo = GDEDataManager.GetCustomList(_key + "_" + LevelsInfoKey, LevelsInfo);

            LevelsInfo.ForEach(x => x.ResetAll());
        }

        public void ResetAll()
        {
            GDEDataManager.ResetToDefault(_key, IdKey);
            GDEDataManager.ResetToDefault(_key, NameKey);
            GDEDataManager.ResetToDefault(_key, InitialValueKey);
            GDEDataManager.ResetToDefault(_key, CurrentValueKey);
            GDEDataManager.ResetToDefault(_key, CurrentLevelKey);
            GDEDataManager.ResetToDefault(_key, CanUpgradeKey);
            GDEDataManager.ResetToDefault(_key, LevelsInfoKey);
            GDEDataManager.ResetToDefault(_key, IconKey);

            Reset_LevelsInfo();

            Dictionary<string, object> dict;
            GDEDataManager.Get(_key, out dict);
            LoadFromDict(_key, dict);
        }

        #region
        /// <summary>
        /// 获取当前升级花费
        /// </summary>
        /// <returns></returns>
        public int GetUpgradeCost()
        {
            if(IsMaxLevel())
            {
                return -1;
            }
            else
            {
                return LevelsInfo[CurrentLevel].Cost;
            }
        }

        /// <summary>
        /// 是否已经满级
        /// </summary>
        /// <returns></returns>
        public bool IsMaxLevel()
        {
            return CurrentLevel >= LevelsInfo.Count;
        }
        #endregion
    }
}
