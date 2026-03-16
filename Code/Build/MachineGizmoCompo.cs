using Code.Core.EventSystems;
using Code.Factory;
using Core.GameSystem;
using DG.Tweening;
using Factory.Machine;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Works.Factory.Code.Core;

public class MachineGizmoCompo : MonoBehaviour, IBuildComponent
{
    [Inject] private PoolManagerMono _poolManager;
    [SerializeField] private InputSO playerInput;

    private BuildingSystem _buildingSystem;
    private GameObject _gizmoObject;
    private IBuildable _selectBuilding;
    private PoolingGizmo _gizmoPool;
    private Vector3Int _prevCellPos;

    private float _rotation;

    private readonly MachineSelectEvent _machineSelectEvent = BuildEventChannel.MachineSelectEvent;
    private readonly MachineDeselectEvent _machineDeselect = BuildEventChannel.MachineDeselectEvent;

    public void Initialize(BuildingSystem buildSystem)
    {
        _buildingSystem = buildSystem;
        _buildingSystem.OnRotateEvent += HandleChangeRotate;
        playerInput.OnClickEvent += HandleClick;
        playerInput.OnRightClickEvent += HandleCancel;

        GameEventBus.AddListener<ChangeDeployModeEvent>(HandleChangeDeployMode);
        GameEventBus.AddListener<MachineUISelectEvent>(HandleMachineSelect);
    }
    
    private void OnDestroy()
    {
        _buildingSystem.OnRotateEvent -= HandleChangeRotate;
        playerInput.OnClickEvent -= HandleClick;
        playerInput.OnRightClickEvent -= HandleCancel;

        GameEventBus.RemoveListener<ChangeDeployModeEvent>(HandleChangeDeployMode);
        GameEventBus.RemoveListener<MachineUISelectEvent>(HandleMachineSelect);
    }
    
    private void Update()
    {
        Vector3Int cellPoint = _buildingSystem.GetCellPoint();

        if (_buildingSystem.CanDeploy)
            DrawMachineGizmo(cellPoint);
        else
            SetOutline(cellPoint);
    }
    
    private void HandleMachineSelect(MachineUISelectEvent evt)
    {
        if (_gizmoObject == null) { PopGizmo(evt.machine.machineGizmo); return; }
        _gizmoObject.SetActive(false);
        _poolManager.Push(_gizmoPool);
        
        Vector3 prevPos = _gizmoObject.transform.position;
        Quaternion prevRot = _gizmoObject.transform.rotation;
        
        PopGizmo(evt.machine.machineGizmo);
        _gizmoObject.transform.position = prevPos;
        _gizmoObject.transform.rotation = prevRot;
    }


    private void HandleClick(bool isClick)
    {
        if (EventSystem.current.IsPointerOverGameObject()
            || _buildingSystem.CanDeploy || !isClick) return;
        
        GameEvent evt = _selectBuilding == null ? 
            _machineDeselect : _machineSelectEvent.Iniailizer(_selectBuilding as Machine);

        GameEventBus.RaiseEvent(evt);
    }

    private void HandleChangeRotate(Vector3 rot)
        => _gizmoObject?.transform.DORotate(rot, 0.15f).SetEase(Ease.OutBack);

    private void HandleCancel() => _gizmoObject?.gameObject.SetActive(false);

    private void HandleChangeDeployMode(ChangeDeployModeEvent channel)
        => _gizmoObject?.gameObject.SetActive(channel.canDeploy);

    private void PopGizmo(PoolItemSO next)
    {
        _gizmoPool = _poolManager.Pop<PoolingGizmo>(next);
        _gizmoObject = _gizmoPool.gameObject;
    }

    private void DrawMachineGizmo(Vector3Int cellPoint)
    {
        Vector3 center = _buildingSystem.GetCellCenterWorld(cellPoint);

        if (cellPoint != _prevCellPos)
        {
            _gizmoObject.transform.DOMove(center, 0.1f).SetEase(Ease.OutSine);
            _prevCellPos = cellPoint;
        }
    }

    private void SetOutline(Vector3Int cellPoint)
    {
        if (cellPoint == _prevCellPos) return;
        _prevCellPos = cellPoint;
        
        IBuildable building = BuildManager.Instance.GetMachineFromCellPos(cellPoint);
        
        if (_selectBuilding != null && _selectBuilding != building)
            _selectBuilding.Outline.enabled = false;

        _selectBuilding = building;
        if (building == null) return;
        
        building.HighlightBuilding();
    }
}
