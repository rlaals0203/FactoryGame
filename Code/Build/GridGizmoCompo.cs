using System;
using Code.Core.EventSystems;
using UnityEngine;

namespace Code.Factory
{
    public class GridGizmoCompo : MonoBehaviour
    {
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private Material gridMaterial;
        [SerializeField] private Vector3 offset;

        [SerializeField] private GameObject parentObject;
        
        
        private void Start()
        {
            DrawGrid();
            GameEventBus.AddListener<ChangeDeployModeEvent>(HandleChangeDeployMode);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<ChangeDeployModeEvent>(HandleChangeDeployMode);
        }

        private void HandleChangeDeployMode(ChangeDeployModeEvent evt)
        {
            parentObject.gameObject.SetActive(evt.canDeploy);
        }

        private void DrawGrid()
        {
            GameObject gridParent = new GameObject("GridGizmo");

            for (int x = 0; x <= width; x++)
            {
                CreateLine(
                    new Vector3(x * cellSize, 0, 0) + offset,
                    new Vector3(x * cellSize, 0, height * cellSize) + offset,
                    gridParent.transform);
            }

            for (int z = 0; z <= height; z++)
            {
                CreateLine(
                    new Vector3(0, 0, z * cellSize) + offset,
                    new Vector3(width * cellSize, 0, z * cellSize) + offset,
                    gridParent.transform);
            }
            
            parentObject.SetActive(false);
        }

        private void CreateLine(Vector3 start, Vector3 end, Transform parent)
        {
            GameObject lineObj = new GameObject("GridLine");
            lineObj.transform.SetParent(parentObject.transform);
            
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.material = gridMaterial;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.useWorldSpace = true;
        }
    }
}
