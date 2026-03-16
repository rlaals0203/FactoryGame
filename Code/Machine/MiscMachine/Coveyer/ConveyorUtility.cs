using System.Collections.Generic;
using UnityEngine;

namespace Factory.Machine.MiscMachine
{
    public static class ConveyorUtility
    {
        public static readonly Dictionary<ConveyorType, int> OutputCount = new()
            {
                { ConveyorType.Stright, 1 },
                { ConveyorType.RightTurn, 1 },
                { ConveyorType.LeftTurn, 1 },
                { ConveyorType.Triple, 2 }
            };
        
        public static readonly Dictionary<ConveyorDirection, ConveyorDirection> PairDirection = new()
            {
                { ConveyorDirection.Up, ConveyorDirection.Down },
                { ConveyorDirection.Down, ConveyorDirection.Up },
                { ConveyorDirection.Left, ConveyorDirection.Right },
                { ConveyorDirection.Right, ConveyorDirection.Left }
            };
        
        public static readonly int[][] Rd = {
            new[] { 0 },
            new[] { 1 },
            new[] { 3 },
            new[] { 1, 3 },
        };
        
        public static Vector3Int GetDirection(ConveyorDirection dir)
        {
            return dir switch {
                ConveyorDirection.Up => new Vector3Int(0, 0, 1),
                ConveyorDirection.Down => new Vector3Int(0, 0, -1),
                ConveyorDirection.Left => new Vector3Int(-1, 0, 0),
                ConveyorDirection.Right => new Vector3Int(1, 0, 0),
                _ => Vector3Int.zero
            };
        }
        
        public static ConveyorDirection GetOpposite(ConveyorDirection dir)
        {
            return dir switch {
                ConveyorDirection.Up => ConveyorDirection.Down,
                ConveyorDirection.Down => ConveyorDirection.Up,
                ConveyorDirection.Left => ConveyorDirection.Right,
                ConveyorDirection.Right => ConveyorDirection.Left,
                _ => ConveyorDirection.Down
            };
        }
        
        public static void ConnectLine(GameObject obj1, GameObject obj2) //테스트용
        {
            LineRenderer line = new GameObject().AddComponent<LineRenderer>();
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.positionCount = 2;
            Vector3 startPos = obj1.transform.position;
            Vector3 endPos = obj2.transform.position;
            startPos.y = 1f;
            endPos.y = 1f;
            
            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
        }
    }

    public enum ConveyorType
    {
        Stright,
        RightTurn,
        LeftTurn,
        Triple
    }

    public enum ConveyorDirection
    {
        Up = 0,
        Right,
        Down,
        Left,
    }
}