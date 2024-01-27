using Grid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridGenerator))]
    public class GridCreatorEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridGenerator gridManager = (GridGenerator)target;
            if (GUILayout.Button("Generate Grid"))
            {
                gridManager.GenerateGrid();
            }
        }
    }
}