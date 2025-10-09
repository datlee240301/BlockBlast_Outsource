using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();
        
        DrawColumnsInputFields();
        EditorGUILayout.Space();
        
        if (ShapeDataInstance.Board != null && ShapeDataInstance.Columns > 0 && ShapeDataInstance.Rows > 0)
        {
            DrawBoardTable();
        }
        
        serializedObject.ApplyModifiedProperties();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }
    
    private void DrawColumnsInputFields()
    {
        var columnsTemp = ShapeDataInstance.Columns;
        var rowsTemp = ShapeDataInstance.Rows;
        
        ShapeDataInstance.Columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.Columns);
        ShapeDataInstance.Rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.Rows);
        
        if ((ShapeDataInstance.Columns != columnsTemp || ShapeDataInstance.Rows != rowsTemp) && ShapeDataInstance.Columns > 0 && ShapeDataInstance.Rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }
    
    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box")
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin =
            {
                left = 32
            }
        };

        var headerColumnStyle = new GUIStyle
        {
            fixedWidth = 65,
            alignment = TextAnchor.MiddleCenter
        };

        var rowStyle = new GUIStyle
        {
            fixedHeight = 25,
            alignment = TextAnchor.MiddleCenter
        };

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid)
        {
            normal =
            {
                background = Texture2D.grayTexture
            },
            onNormal =
            {
                background = Texture2D.whiteTexture
            }
        };

        for (var row = 0; row < ShapeDataInstance.Rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for (var column = 0; column < ShapeDataInstance.Columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.Board[row].Column[column], dataFieldStyle);
                ShapeDataInstance.Board[row].Column[column] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
