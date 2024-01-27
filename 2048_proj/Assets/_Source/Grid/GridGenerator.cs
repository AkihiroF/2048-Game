using System.Collections.Generic;
using BlockSystem.Data;
using BlockSystem.View;
using UnityEngine;

namespace Grid
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private int gridWidth = 4;
        [SerializeField] private int gridHeight = 4;
        [SerializeField] private float tileSpacing;
        [SerializeField] private GameObject tilePrefab;

        // Словарь для хранения данных о плитках
        private Dictionary<Vector2, BlockData> _gridData;
        

        public void GenerateGrid()
        {
            _gridData = new Dictionary<Vector2, BlockData>();
            ClearGrid();
            GenerateGridPositions();
        }

        private void ClearGrid()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        private void GenerateGridPositions()
        {
            Vector3 startPosition = CalculateStartPosition();

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 position = startPosition + new Vector3(x * tileSpacing, y * tileSpacing, 0);
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                    // Создаем и добавляем CubeData для каждой плитки
                    Vector2 pos2D = new Vector2(position.x, position.y);
                    if(tile.TryGetComponent(out BlockView view))
                    {
                        _gridData[pos2D] = new BlockData(view);
                        view.SetNumber(0);
                    } // Здесь можно инициализировать CubeData
                }
            }
        }

        private Vector3 CalculateStartPosition()
        {
            float offsetX = (gridWidth - 1) * tileSpacing / 2;
            float offsetY = (gridHeight - 1) * tileSpacing / 2;
            return transform.position - new Vector3(offsetX, offsetY, 0);
        }

        public Dictionary<Vector2, BlockData> GetGridData()
        {
            GenerateGrid();
            return _gridData;
        }
    }
}
