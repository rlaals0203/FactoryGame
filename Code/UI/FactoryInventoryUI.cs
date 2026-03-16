using System;
using System.Collections.Generic;
using Code.Core.EventSystems;
using Code.Factory;
using Core.Code.Utill;
using Factory.Machine.MiscMachine;
using Players;
using UnityEngine;
using Works.Factory.Code.Core;
using Works.Factory.Code.UI;
using Works.Inventory.Code;
using Works.Items.Code;
using Works.Shop.Code;

public class FactoryInventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryDataListSO inventory;
    [SerializeField] private MineralDataUI mineralDataUI;
    [SerializeField] private Transform content;
    [SerializeField] private Transform background;

    private Dictionary<ItemDataSO, MineralDataUI> _mineralDict = new();

    private void Awake()
    {
        GameEventBus.AddListener<MineralAddEvent>(HandleMineralAdd);
        GameEventBus.AddListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
    }

    private void HandleMineralAdd(MineralAddEvent evt)
    {
        if (evt.data.AsResourceType() != ResourceType.Mineral) return;
        if (!_mineralDict.ContainsKey(evt.data))
        {
            MineralDataUI newUI = Instantiate(mineralDataUI, content);
            newUI.SetUpUI(evt.data, 1);
            _mineralDict.Add(evt.data, newUI);
        }
        else
        {
            _mineralDict[evt.data].RefreshUI();
        }
    }

    private void OnDestroy()
    {
        GameEventBus.RemoveListener<MineralAddEvent>(HandleMineralAdd);
        GameEventBus.RemoveListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
    }
    

    private void HandleChangeBuildMode(ChangeBuildModeEvent evt)
    {
        background.gameObject.SetActive(evt.canBuild);
    }
}
