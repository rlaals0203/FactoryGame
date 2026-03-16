using System.Collections.Generic;
using System.Linq;
using Code.Factory;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Factory.Machine.MiscMachine
{
    public class BaseConveyor : Machine
    {
        [field: SerializeField] public ConveyorType ConveyorType {get; private set; }
        public ConveyorSO ConveyorSO { get; private set; }
        public List<Mineral> MineralStorage { get; } = new();
        
        private List<ConveyorDirection> _outputDirections = new();
        [SerializeField] private List<BaseConveyor> _connectedConveyors = new();
        private ConveyorDirection _inputDirection;
        
        private readonly float yOffset = 0.6f;
        private int _idx = 0;
        
        protected override void InitializeMachine()
        {
            base.InitializeMachine();
            ConveyorSO = machineSO as ConveyorSO;
            
            CheckDirection();
            TryConnect();
        }

        private void Update()
        {
            MoveConveyor();
        }

        public virtual void ConveyorAction(Mineral mineral) { }

        private void MoveConveyor()
        {
            if(ConveyorSO == null)
                ConveyorSO = machineSO as ConveyorSO;

            foreach (var mineral in MineralStorage.ToList())
            {
                if (mineral.CurrentConveyor == null)
                {
                    MineralStorage.Remove(mineral);
                    continue;
                }
                if(mineral.IsConnecting) continue;
                
                Vector3 destination = mineral.CurrentConveyor.transform.position + Vector3.up * yOffset;
                mineral.transform.position = Vector3.MoveTowards(mineral.transform.position,
                    destination, ConveyorSO.conveyorSpeed * Time.deltaTime);
                
                if (Vector3.Distance(mineral.transform.position, destination) < 0.01f)
                {
                    mineral.transform.position = destination;
                    
                    RemoveMineral(mineral);
                    AddMineral(mineral);
                }
            }
        }

        public void AddMineral(Mineral mineral)
        {
            ConveyorAction(mineral);

            if (!GetNextConveyor(out BaseConveyor nextConveyor))
            {
                RemoveMineral(mineral);
                mineral.PushMineral();
                return;
            }

            mineral.ChangedTime = Time.time;
            mineral.CurrentConveyor = nextConveyor;
            nextConveyor.MineralStorage.Add(mineral);
        }

        protected void RemoveMineral(Mineral mineral)
        {
            MineralStorage.Remove(mineral);
        }

        public void ConnectMineral(Mineral mineral, bool hasDelay = true)
        {
            float duration = hasDelay ? 0.5f : 0f;
            Vector3 targetPos = transform.position + Vector3.up * yOffset;
            mineral.transform.DOKill();
            mineral.IsConnecting = true;
            mineral.transform.DOMove(targetPos, duration).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    AddMineral(mineral);
                    mineral.IsConnecting = false;
                });
        }

        public bool GetNextConveyor(out BaseConveyor nextConveyor)
        {
            nextConveyor = null;
            if (_connectedConveyors.Count == 0) return false;

            if (_idx >= _connectedConveyors.Count)
                _idx = 0;

            nextConveyor = _connectedConveyors[_idx++];
            return true;
        }

        #region ConnctLogic
        public void CheckDirection()
        {
            _outputDirections.Clear();
            
            int rot = Mathf.RoundToInt(transform.rotation.eulerAngles.y % 360);
            int outCount = ConveyorUtility.OutputCount[ConveyorType];
            _inputDirection = ConveyorUtility.GetOpposite((ConveyorDirection)(rot / 90));
            
            for (int i = 0; i < outCount; i++)
            {
                int newRot = (ConveyorUtility.Rd[(int)ConveyorType][i] * 90 + rot) % 360;
                _outputDirections.Add((ConveyorDirection)(newRot / 90));
            }
        }
        
        public void TryConnect()
        {
            Vector3Int selfPos = BuildManager.Instance.grid.WorldToCell(transform.position);
            _connectedConveyors.Clear();
            
            foreach (var dir in _outputDirections)
            {
                Vector3Int targetPos = selfPos + ConveyorUtility.GetDirection(dir);

                if (BuildManager.Instance.GetMachineFromCellPos(targetPos) is BaseConveyor conveyor)
                {
                    if (conveyor._inputDirection == ConveyorUtility.PairDirection[dir])
                        ConnectConveyor(conveyor);
                }
            }
        }

        private void ConnectConveyor(BaseConveyor baseConveyor)
        {
            if (_connectedConveyors.Contains(baseConveyor)) return;
            _connectedConveyors.Add(baseConveyor);
        }
        #endregion
    }
}