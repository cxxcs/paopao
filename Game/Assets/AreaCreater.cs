
using System.Collections.Generic;
using UnityEngine;


public class AreaCreater : MonoBehaviour
{

    public Vector2 Frame = new Vector2(50, 50);
    public Vector2 Complexity = new Vector2(10,10);
    public Transform Plane;
    public int maxPolygon = 20;
    public Vector2 GridRange = new Vector2(2,6);

    private Dictionary<KeyValuePair<int,int>, Grid> _map = new Dictionary<KeyValuePair<int, int>, Grid>();
    [ContextMenu("Create")]
    public void Create() {
        Vector2 size = Frame;
        var linem = Resources.Load<Material>("line");
        var game = GameObject.Find("Game");
        if (game != null) {
            Object.DestroyImmediate(game);
        }
        _map.Clear();
        game = new GameObject("Game");
        game.transform.localPosition = new Vector2(-Frame.x / 2, -Frame.y / 2);
        Vector2 offset = Vector2.zero;
        Vector2 border = new Vector2(0.02f,0.02f);
        Vector2 spacing = new Vector2(size.x / Complexity.x, size.y / Complexity.y);
        int index = 0;
        for (int x = 0; x < Complexity.x; x++)
        {
            for (int y = 0; y < Complexity.y; y++) {
                GameObject plane = GameObject.Instantiate(Plane.gameObject, game.transform);
                var grid = plane.GetComponent<Grid>();
                grid.SetPos(x, y);
                plane.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                plane.transform.localScale = new Vector3(spacing.x / 10 - border.x, 1, spacing.y / 10 - border.y);
                plane.transform.localPosition = new Vector3(x * spacing.x + offset.x + spacing.x / 2, y * spacing.y + offset.y + spacing.y / 2, 0);
                _map.Add(new KeyValuePair<int, int>(x,y), grid);
                index++;
    
            }     
        }
    }
}
