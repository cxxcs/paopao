
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[ExecuteInEditMode]

public class UIAreaCreater : MonoBehaviour
{
#if UNITY_EDITOR
    public Vector2 SingeSize = new Vector2(50, 50);
    public Vector2 Complexity = new Vector2(10,10);
    public Transform Plane;

    public Dictionary<KeyValuePair<int,int>, Grid> Maps = new Dictionary<KeyValuePair<int, int>, Grid>();

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) {
        //    Debug.LogError(2);
        //    if (Selection.gameObjects.Length > 0)
        //    {
        //        Grid negGrid = null;
        //        for (int i = 0; i < Selection.gameObjects.Length; i++)
        //        {
        //            var go = Selection.gameObjects[i];
        //            var grid = go.gameObject.GetComponent<Grid>();
        //            if (grid != null)
        //            {
        //                if (negGrid == null) {
        //                    negGrid = grid;
        //                }
        //                Debug.LogError(1);
        //                grid.SetSelect(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
        //                grid.SetRelevancy(negGrid.PosX, negGrid.PosY);
        //            }
        //        }
        //    }

        //}


        if (Input.GetMouseButton(0))
        {
            if (Selection.gameObjects.Length > 0)
            {
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var go = Selection.gameObjects[i];
                    var grid = go.gameObject.GetComponent<Grid>();
                    if (grid != null)
                    {
                        return;
                    }
                }
            }
            if (Selection.activeGameObject != this.gameObject)
            {
                Selection.activeGameObject = this.gameObject;
            }
        }

    }
  

    public void Save(int id) 
    {
        var game = GameObject.Find("Game");
        if (game != null) {
            PrefabUtility.SaveAsPrefabAssetAndConnect(game, "Assets/LevelPrefabs/" + id + ".prefab", InteractionMode.AutomatedAction);
        }
        var jsonData = new JsonData();

        jsonData.Canvas.PosX = (int)game.transform.localPosition.x;
        jsonData.Canvas.PosY = (int)game.transform.localPosition.y;

        for (int i = 0; i < game.transform.childCount; i++)
        {
            var grid = game.transform.GetChild(i).gameObject.GetComponent<Grid>();
            var size = grid.gameObject.GetComponent<RectTransform>().sizeDelta;
            jsonData.Grids.Add(new GridData() {
                PosX = grid.PosX, 
                PosY = grid.PosY,
                DisplayX = (int)grid.gameObject.transform.localPosition.x,
                DisplayY = (int)grid.gameObject.transform.localPosition.y,
                SizeX = (int)size.x,
                SizeY = (int)size.y,
                RelevancyPosX = grid.RelevancyPosX,
                RelevancyPosY = grid.RelevancyPosY, 
                Enbale = grid.Enbale,
                Select = grid.Select,
                Neg = grid.Neg,
            });
        }
        var json = JsonUtility.ToJson(jsonData);
        System.IO.File.WriteAllText("Assets/StreamingAssets/Levels/" + id + ".json", json);
    }
    public void Load(int id) 
    {
        Maps.Clear();
        Vector2 offset = Vector2.zero;
        Vector2 border = new Vector2(0.02f, 0.02f);
        Vector2 spacing = new Vector2(SingeSize.x / Complexity.x, SingeSize.y / Complexity.y);
        var game = GameObject.Find("Game");
        if (game != null)
        {

            UnityEngine.Object.DestroyImmediate(game);
        }
        var res = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelPrefabs/" + id + ".prefab");
        if (res != null)
        {
            game = GameObject.Instantiate(res);
            game.name = "Game";
            game.transform.SetParent(this.transform);

            game.transform.localPosition = Vector3.zero;
            game.transform.localScale = Vector3.one;
            game.transform.localPosition = new Vector2(-(SingeSize.x * Complexity.x / 2), -(SingeSize.y * Complexity.y / 2));
            for (int i = 0; i < game.transform.childCount; i++) {
                var grid = game.transform.GetChild(i).gameObject.GetComponent<Grid>();
                if (grid) {
                    grid.HideChild();
                    Maps.Add(new KeyValuePair<int, int>(grid.PosX, grid.PosY), grid);
                }
            }
        }
        else {

            Debug.LogError(string.Format("没有找到关卡 {0}", id));
        }
    }

    [ContextMenu("Create")]
    public void Create() {
        Maps.Clear();
        Vector2 offset = Vector2.zero;
        Vector2 border = new Vector2(0.02f,0.02f);
        Vector2 spacing = new Vector2(SingeSize.x / Complexity.x, SingeSize.y / Complexity.y);
        var game = GameObject.Find("Game");
        if (game != null)
        {
            UnityEngine.Object.DestroyImmediate(game);
        }
        game = new GameObject("Game");
        game.transform.SetParent(this.transform);
        game.transform.localPosition = Vector3.zero;
        game.transform.localScale = Vector3.one;

        game.transform.localPosition = new Vector2(-(SingeSize.x * Complexity.x / 2), -(SingeSize.y * Complexity.y / 2));
        Maps.Clear();

        int index = 0;
        for (int x = 0; x < Complexity.x; x++)
        {
            for (int y = 0; y < Complexity.y; y++) {
                GameObject plane = GameObject.Instantiate(Plane.gameObject, game.transform);
                plane.gameObject.SetActive(true);
                var grid = plane.GetComponent<Grid>();
                grid.HideChild();
                grid.SetPos(x, y);
                plane.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(SingeSize.x - 4, SingeSize.y - 4);
                plane.transform.localPosition = new Vector3(x * SingeSize.x + SingeSize.x / 2, y * SingeSize.y + SingeSize.y / 2, 0);
                Maps.Add(new KeyValuePair<int, int>(x,y), grid);
                index++;
    
            }     
        }
        game.AddComponent<GameInput>();
    }
#endif
}
