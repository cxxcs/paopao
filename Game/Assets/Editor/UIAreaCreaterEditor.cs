
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Codice.Client.BaseCommands.Import.Commit;


[CustomEditor(typeof(UIAreaCreater))]
public class UIAreaCreaterEditor : Editor
{
    private UIAreaCreater _creater;
    private void OnEnable()
    {
        _creater = (UIAreaCreater)target;
    }
    //private void OnDisable()
    //{
    //    SceneView.duringSceneGui -= AppOnSceneGUI;
    //}
    private int LevelID = 1;
    private int _keyIndex = 0;
    private void AppOnSceneGUI(SceneView view)
    {
        Event e = Event.current;
        if (e.keyCode == KeyCode.C || e.keyCode == KeyCode.Z) {
            var masks = new Dictionary<KeyValuePair<int, int>, Grid>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var go = Selection.gameObjects[i];
                var grid = go.gameObject.GetComponent<Grid>();
                if (grid != null)
                {
                    if (grid.Select == true)
                        masks[new KeyValuePair<int, int>(grid.RelevancyPosX, grid.RelevancyPosY)] = grid;
                    else
                        masks[new KeyValuePair<int, int>(grid.PosX, grid.PosY)] = grid;
                }
            }
            if (e.keyCode == KeyCode.C)
            {
                foreach (var g in masks)
                {
                    var xy = g.Key;
                    foreach (var grid in _creater.Maps.Values)
                    {
                        if (grid.Select == true)
                        {
                            if (xy.Key == grid.RelevancyPosX && xy.Value == grid.RelevancyPosY)
                            {
                                grid.ClearSelect();
                                grid.Neg = 0;
                                grid.SetEnable(false);
                            }
                        }
                        else
                        {

                            if (xy.Key == grid.PosX && xy.Value == grid.PosY)
                            {
                                grid.ClearSelect();
                                grid.Neg = 0;
                                grid.SetEnable(false);
                            }
                        }
                    }
                }
            }
            else if (e.keyCode == KeyCode.Z)
            {

                foreach (var g in masks)
                {
                    var xy = g.Key;
                    foreach (var grid in _creater.Maps.Values)
                    {
                        if (grid.Select == true)
                        {
                            if (xy.Key == grid.RelevancyPosX && xy.Value == grid.RelevancyPosY)
                            {
                                grid.Neg = 0;
                                grid.SetEnable(false);
                            }
                        }
                        else
                        {

                            if (xy.Key == grid.PosX && xy.Value == grid.PosY)
                            {
                                grid.Neg = 0;
                                grid.SetEnable(true);
                            }
                        }
                    }
                }

            }

        }
        if (e.keyCode >= KeyCode.Keypad1 && e.keyCode <= KeyCode.Keypad9)
        {
            var selects = new List<Grid>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var go = Selection.gameObjects[i];
                var grid = go.gameObject.GetComponent<Grid>();
                if (grid != null)
                {
                    selects.Add(grid);
                }
            }
            if (selects.Count > 0)
            {
                _keyIndex = e.keyCode - KeyCode.Keypad1;
                Grid negGrid = selects[_keyIndex % selects.Count];
                for (int i = 0; i < selects.Count; i++)
                {
                    var grid = selects[i];
                    grid.SetSelect(grid.NoramlColor);
                    grid.SetRelevancy(negGrid.PosX, negGrid.PosY);
                    grid.Neg = 0;
                }
                negGrid.Neg = selects.Count;
            }
        }
        if (e.isMouse == true && e.button == 0)
        {
            Grid negGrid = null;
            Color negColor = Color.gray;
            var count = 0;
            List<KeyValuePair<int,int>> masks = new List<KeyValuePair<int, int>>();
            var selects = new List<Grid>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var go = Selection.gameObjects[i];
                var grid = go.gameObject.GetComponent<Grid>();
                if (grid != null && grid.Enbale == true)
                {
                    selects.Add(grid);
                    if (grid.Select == true)
                    {
                        masks.Add(new KeyValuePair<int, int>(grid.RelevancyPosX, grid.RelevancyPosY));
                    } 
                }
            }
            for (int i = 0; i < masks.Count; i++) {
                var xy = masks[i];
                foreach (var grid in _creater.Maps.Values) {
                    if (xy.Key == grid.RelevancyPosX && xy.Value == grid.RelevancyPosY)
                    {
                        grid.ClearSelect();
                        grid.Neg = 0;
                    }
                }
            }
            if (selects.Count > 0) {
                if (_keyIndex == -1) {
                    _keyIndex = Random.Range(0, selects.Count - 1);
                }
                negGrid = selects[_keyIndex % selects.Count];
                negColor = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
                for (int i = 0; i < selects.Count; i++)
                {
                    var grid = selects[i];
                    grid.SetSelect(negColor);
                    grid.SetRelevancy(negGrid.PosX, negGrid.PosY);
                    negGrid.Neg = selects.Count;
                }
                _keyIndex = -1;
            }
            if (Selection.activeGameObject != _creater.gameObject)
            {
                Selection.activeGameObject = _creater.gameObject;
            }
        }
    }
    public override void OnInspectorGUI() {

        GUILayout.BeginHorizontal();
        _creater.SingeSize.x = EditorGUILayout.IntField("宽度（x）", (int)_creater.SingeSize.x);
        _creater.SingeSize.y = EditorGUILayout.IntField("高度（y）", (int)_creater.SingeSize.y);
        EditorGUILayout.Space(10);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        _creater.Complexity.x = EditorGUILayout.IntField("数量（x）", (int)_creater.Complexity.x);
        _creater.Complexity.y = EditorGUILayout.IntField("数量（y）", (int)_creater.Complexity.y);
        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);


        var count = 0;
        foreach (var grid in _creater.Maps.Values)
        {
            if (grid.gameObject.activeSelf) {
                count++;
            }
        }

        EditorGUILayout.IntField("格子总数：", count);
        GUILayout.Space(10);

        if (GUILayout.Button("创建关卡")) {
            _creater.Create();
            SceneView.duringSceneGui -= AppOnSceneGUI;
            SceneView.duringSceneGui += AppOnSceneGUI;
        }
        GUILayout.Space(50);

        LevelID = EditorGUILayout.IntField("关卡ID：", LevelID);
        GUILayout.Space(10);
        if (GUILayout.Button("加载关卡"))
        {
            SceneView.duringSceneGui -= AppOnSceneGUI;
            SceneView.duringSceneGui += AppOnSceneGUI;
            _creater.Load(LevelID);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("保存关卡"))
        {
            _creater.Save(LevelID);
        }

    }
}
