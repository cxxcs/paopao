using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Grid : MonoBehaviour
{
    public TextMeshProUGUI PosText;
    public TextMeshProUGUI NegText;
    public Image SelectBg;
    public Color NoramlColor = new Color(53 / 255f, 53 / 255f, 53 / 255f, 1);
    public Color DeleteColor = new Color(53 / 255f, 53 / 255f, 53 / 255f, 0.1f);
    [ContextMenu("HideChild")]
    public void HideChild()
    {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
    }
    public int Neg {
        set {
            if (value <= 0)
            {
                NegText.text = "";
            }
            else {
                NegText.text = value.ToString();
            }

        }
        get {
            var ret = 0;
            int.TryParse(NegText.text, out ret);
            return ret;
        }
    }
    public int PosX;
    public int PosY;
    public bool Select = false;
    public int RelevancyPosX;
    public int RelevancyPosY;
    public void SetSelect(Color color) {
        Select = true;

        SelectBg.color = color;
    }
    public void SetRelevancy(int x,int y)
    {
        RelevancyPosX = x;
        RelevancyPosY = y;
    }
    public void ClearSelect()
    {
        Select = false;
        SelectBg.color = NoramlColor;
        RelevancyPosX = 0;
        RelevancyPosY = 0;
    }
    public bool Enbale = true;
    public void SetEnable(bool isOn)
    {
        Enbale = isOn;
        if (isOn)
        {
            ClearSelect();
        }
        else {
            SelectBg.color = DeleteColor;
        }
    }
    public void SetPos(int x, int y) {
        PosX = x;
        PosY = y;
        PosText.text = string.Format("{0},{1}", PosX, PosY);
    }
}
