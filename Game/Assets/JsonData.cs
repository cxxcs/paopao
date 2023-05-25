using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData 
{
    public List<GridData> Grids = new List<GridData>();
    public CanvasData Canvas = new CanvasData();
}
[Serializable]
public class GridData
{
    public int PosX;
    public int PosY;
    public int DisplayX;
    public int DisplayY;
    public int SizeX;
    public int SizeY;
    public int Neg;
    public bool Select;
    public int RelevancyPosX;
    public int RelevancyPosY;
    public bool Enbale;
}
[Serializable]
public class CanvasData
{
    public int PosX;
    public int PosY;
}

public class JsonFileList
{

    public List<string> Files = new List<string>();

}