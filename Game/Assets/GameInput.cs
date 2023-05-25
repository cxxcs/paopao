using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public enum InputMode {
        None,
        Add,
        Clear
    }
    public Dictionary<KeyValuePair<int, int>, Grid> Grids = new Dictionary<KeyValuePair<int, int>, Grid>();
    [ContextMenu("Ready")]
    public void Ready() {
        var grids = GetComponentsInChildren<Grid>(true);
        foreach (var grid in grids) {
            Grids.Add(new KeyValuePair<int, int>(grid.PosX, grid.PosY), grid);
        }
    }
    private void Start()
    {
        Ready();
    }
    private InputMode inputMode = InputMode.None;
    private Dictionary<KeyValuePair<int, int>, Grid> grids = new Dictionary<KeyValuePair<int, int>, Grid>();
    private Color _randomColor = Color.red;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            _randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1); 
            var casts = IsPointerOverGameObject(Input.mousePosition);
            grids.Clear();
            for (int i = 0; i < casts.Count; i++)
            {
                var cast = casts[i];
                var grid = cast.gameObject.GetComponent<Grid>();
                if (grid != null)
                {
                    if (grid.Select == false) {
                        inputMode = InputMode.Add;
                        var key = new KeyValuePair<int, int>(grid.PosX, grid.PosY);
                        grid.SetSelect( _randomColor);
                        grids[key] = grid;
                    }
                    else {
                        inputMode = InputMode.Clear;
  
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (inputMode == InputMode.Add) 
            {
                Grid negGrid = null;
                foreach (var g in grids.Values)
                {
                    if (g.Neg != 0)
                    {
                        negGrid = g;
                        break;
                    }
                }
                if (negGrid == null || negGrid.Neg != grids.Count)
                {
                    foreach (var g in grids.Values)
                    {
                        g.ClearSelect();
                    }
                    grids.Clear();
                }
                else if (negGrid != null)
                {
                    foreach (var g in grids.Values)
                    {
                        g.SetRelevancy(negGrid.PosX, negGrid.PosY);
                    }
                    grids.Clear();
                }
                var complete = true;
                foreach (var g in Grids.Values) {
                    if (g.gameObject.activeSelf == true) {
                        if (g.Select == false) {
                            complete = false;
                            break;
                        }
                    }
                }
                if (complete) {
                    this.SendMessageUpwards("OnSuccess");
                }
            }
            
        }

        if (Input.GetMouseButton(0)) {
            var casts = IsPointerOverGameObject(Input.mousePosition);
            for (int i = 0; i < casts.Count; i++) { 
                var cast = casts[i];
                var grid = cast.gameObject.GetComponent<Grid>();
                if (grid != null) {
                    if (inputMode == InputMode.Add) {
                        if (grid.Select == false) {
                            Grid negGrid = null;
                            var minx = 256;
                            var miny = 256;
                            var maxx = 0;
                            var maxy = 0;

                            if (grid.PosX < minx)
                            {
                                minx = grid.PosX;
                            }
                            if (grid.PosX > maxx)
                            {
                                maxx = grid.PosX;
                            }
                            if (grid.PosY < miny)
                            {
                                miny = grid.PosY;
                            }
                            if (grid.PosY > maxy)
                            {
                                maxy = grid.PosY;
                            }

                            foreach (var g in grids.Values)
                            {
                                if (g.PosX < minx) {
                                    minx = g.PosX;
                                }
                                if (g.PosX > maxx)
                                {
                                    maxx = g.PosX;
                                }
                                if (g.PosY < miny)
                                {
                                    miny = g.PosY;
                                }
                                if (g.PosY > maxy)
                                {
                                    maxy = g.PosY;
                                }
                            }
                            foreach (var g in grids.Values) {
                                if (g.Neg != 0) {
                                    negGrid = g;
                                    break;
                                }
                            
                            }
                            for (int x = minx; x <= maxx; x++)
                            {
                                var canSelect = true;
                                for (int y = miny; y <= maxy; y++)
                                {
                                    var g = Grids[new KeyValuePair<int, int>(x, y)];
                                    if (g.Select == false)
                                    {
                                        if ((negGrid != null && g.Neg == 0 && g != negGrid) || negGrid == null)
                                        {
                            
                                        }
                                        else
                                        {
                                            canSelect = false;
                                        }
                                    }

                                }
                                if (canSelect == false)
                                {
                                    break;
                                }
                                for (int y = miny; y <= maxy; y++)
                                {
                                    var key = new KeyValuePair<int, int>(x, y);
                                    var g = Grids[key];
                                    grids[key] = g;
                                    g.SetSelect( _randomColor);
                                }
                            }
                        }
                    }
                    else if (inputMode == InputMode.Clear)
                    {
                        var clears = new List<Grid>();
                        foreach (var g in Grids.Values)
                        {
                            if (g.RelevancyPosX == grid.RelevancyPosX && g.RelevancyPosY == grid.RelevancyPosY
                                || g.RelevancyPosX == grid.PosX && g.RelevancyPosY == grid.PosY)
                            {
                                clears.Add(g);
                            }
                        }
                        for (int j = 0; j < clears.Count; j++)
                        {
                            clears[j].ClearSelect();
                        }
                    }
                   
                }
            }
        }

    }
    private List<RaycastResult> IsPointerOverGameObject(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
