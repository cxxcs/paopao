using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Games : MonoBehaviour
{
    public int Current = 0;
    void Start()
    {
        OnNext();
    }
    private void LoadLevel(GameObject res)
    {
        if (LevelParent.childCount > 0) {
            Object.Destroy(LevelParent.GetChild(0).gameObject);
        }
        GameObject.Instantiate(res, LevelParent);
        Panel.gameObject.SetActive(false);
    }
    public Transform LevelParent;
    public Transform Panel;
    public TextMeshProUGUI Tips;
    public void OnSuccess() 
    {
        Panel.gameObject.SetActive(true);
        Tips.text = string.Format("Success ! {0}", Current);
    }
    public void OnNext()
    {
        var res = Resources.Load<GameObject>("Levels/" + (Current + 1));
        if (res != null) {
            Current = Current + 1;
            LoadLevel(res);
        }

    }
    public TMP_InputField LevelID;
    public void OnSelect()
    {
        var res = Resources.Load<GameObject>("Levels/" + int.Parse(LevelID.text));
        if (res != null)
        {
            LoadLevel(res);
        }
    }
}
