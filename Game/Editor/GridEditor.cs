using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CanEditMultipleObjects]
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    private Grid _grid;
    private void OnEnable()
    {
        _grid = (Grid)target;
    }
    //public override void OnInspectorGUI()
    //{
    //}

}
