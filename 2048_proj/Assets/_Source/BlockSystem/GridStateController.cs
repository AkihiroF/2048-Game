using System;
using System.Collections.Generic;
using BlockSystem.Data;
using Grid;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BlockSystem
{
    public class GridStateController
{
    private readonly Dictionary<Vector2, BlockData> _tileData = new();
    private Vector2[,] _gridPositions;
    
    private readonly HashSet<Vector2> _mergedThisTurn = new();

    public static event Action OnFinish; 

    [Inject]
    private void Construct(GridGenerator gridGenerator)
    {
        var positions = gridGenerator.GetGridData();
        GenerateGridState(positions);
    }

    // Генерація початкового стану сітки
    private void GenerateGridState(Dictionary<Vector2, BlockData> positions)
    {
        var minX = float.MaxValue;
        var maxX = float.MinValue;
        var minY = float.MaxValue;
        var maxY = float.MinValue;

        foreach (var position in positions.Keys)
        {
            minX = Mathf.Min(minX, position.x);
            maxX = Mathf.Max(maxX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxY = Mathf.Max(maxY, position.y);
        }

        int gridWidth = Mathf.CeilToInt(maxX - minX);
        int gridHeight = Mathf.CeilToInt(maxY - minY);

        _gridPositions = new Vector2[gridWidth, gridHeight];

        foreach (var pair in positions)
        {
            var worldPos = pair.Key;
            var cubeData = pair.Value;

            int xIndex = Mathf.FloorToInt(worldPos.x - minX);
            int yIndex = Mathf.FloorToInt(worldPos.y - minY);
            
            _gridPositions[xIndex, yIndex] = worldPos;
            _tileData.Add(worldPos, cubeData);
        }

        AddNewTile();
    }

    public void MoveElements(Vector2 direction)
    {
        if (direction.x != 0)
        {
            MoveHorizontal(direction.x < 0);
        }
        else if (direction.y != 0)
        {
            MoveVertical(direction.y < 0);
        }
    }
    
    // Горизонтальний рух блоків
    private void MoveHorizontal(bool isLeft)
    {
        Vector2 direction = isLeft ? Vector2.left : Vector2.right;

        _mergedThisTurn.Clear();
        for (int y = 0; y < _gridPositions.GetLength(1); y++)
        {
            for (int x = isLeft ? 0 : _gridPositions.GetLength(0) - 1; isLeft ? x < _gridPositions.GetLength(0) : x >= 0; x += isLeft ? 1 : -1)
            {
                Vector2 currentPos = _gridPositions[x, y];
                if (_tileData[currentPos].Score == 0) continue;

                Vector2 targetPos = FindTargetPosition(new Vector2Int(x, y), direction, out bool canMerge);
                MoveTile(currentPos, targetPos, canMerge);
            }
        }

        AddNewTile();
    }

    // Вертикальний рух блоків
    private void MoveVertical(bool isDown)
    {
        Vector2 direction = isDown ? Vector2.down : Vector2.up;

        _mergedThisTurn.Clear();
        for (int x = 0; x < _gridPositions.GetLength(0); x++)
        {
            for (int y = isDown ? 0 : _gridPositions.GetLength(1) - 1; isDown ? y < _gridPositions.GetLength(1) : y >= 0; y += isDown ? 1 : -1)
            {
                Vector2 currentPos = _gridPositions[x, y];
                if (_tileData[currentPos].Score == 0) continue;

                Vector2 targetPos = FindTargetPosition(new Vector2Int(x, y), direction, out bool canMerge);
                MoveTile(currentPos, targetPos, canMerge);
            }
        }

        AddNewTile();
    }
    
    // Додавання нового блоку на сітку
    private void AddNewTile()
    {
        List<Vector2> emptyPositions = new List<Vector2>();
        foreach (var kvp in _tileData)
        {
            if (kvp.Value.Score == 0)
            {
                emptyPositions.Add(kvp.Key);
            }
        }

        if (emptyPositions.Count > 0)
        {
            Vector2 newPosition = emptyPositions[Random.Range(0, emptyPositions.Count)];
            _tileData[newPosition].OnMerge();
        }
        else
        {
            OnFinish?.Invoke();
        }
    }

    // Переміщення блоку з однієї позиції на іншу
    private void MoveTile(Vector2 from, Vector2 to, bool canMerge)
    {
        if (to != from)
        {
            if (canMerge)
            {
                _tileData[to].OnMerge();
                _mergedThisTurn.Add(to);
            }
            else
            {
                _tileData[to].OnUpdateInfo(_tileData[from].Score);
            }

            _tileData[from].OnClear();
        }
    }

    // Знаходження цільової позиції для руху блоку
    private Vector2 FindTargetPosition(Vector2Int currentPos, Vector2 direction, out bool canMerge)
    {
        int x = currentPos.x;
        int y = currentPos.y;

        var startPos = _gridPositions[x, y];
        canMerge = false;

        while (x + (int)direction.x >= 0 && x + (int)direction.x < _gridPositions.GetLength(0) &&
               y + (int)direction.y >= 0 && y + (int)direction.y < _gridPositions.GetLength(1))
        {
            x += (int)direction.x;
            y += (int)direction.y;

            Vector2 nextPos = _gridPositions[x, y];
            if (_tileData.TryGetValue(nextPos, out BlockData nextTile))
            {
                if (_tileData[startPos].Score == nextTile.Score && !_mergedThisTurn.Contains(nextPos))
                {
                    canMerge = true;
                    break;
                }
                else if (nextTile.Score != 0)
                {
                    break;
                }
            }
        }

        return _gridPositions[x, y];
    }
}

}