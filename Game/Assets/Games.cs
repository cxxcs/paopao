using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Games : MonoBehaviour
{
    private GameObject _gridRes;
    private JsonFileList _fileList;
    public GameObject SelectButton;
    void Start()
    {
        StartCoroutine(TextReader("s.json", delegate (string text)
        {
            _fileList = JsonUtility.FromJson<JsonFileList>(text);
            SelectButton.gameObject.SetActive(true);
        }));

    }
    public GameObject LevelSelectPanel;
    public GameObject LevelToggle;
    private List<Toggle> _levelToggles = new List<Toggle>();
    public void OnLevelSelect(string filename) 
    {
        StartCoroutine(TextReader("Levels/" + filename + ".json", delegate (string text)
        {
            var jsonData = JsonUtility.FromJson<JsonData>(text);
            LoadLevel(jsonData);
        }));
    }
    public static IEnumerator TextReader(string configName, UnityAction<string> action = null)
    {
        string path;
#if UNITY_WIN_STANDALONE || UNITY_IPHONE && !UNITY_EDITOR
        path = "file://" + Application.streamingAssetsPath + configName;
#else
        path = Application.streamingAssetsPath + "/" + configName;
#endif
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.error != null)
            Debug.Log(unityWebRequest.error);
        else
        {
            string content = unityWebRequest.downloadHandler.text;
            if (action != null)
                action(content);
        }
    }
    public void ShowLevelSelect() {
        if (LevelSelectPanel.activeSelf == false)
        {
            for (int i = 0; i < _levelToggles.Count; i++)
            {
                var toggle = _levelToggles[i];
                toggle.onValueChanged.RemoveAllListeners();
                Object.Destroy(toggle.gameObject);
            }
            _levelToggles.Clear();
            _gridRes = Resources.Load<GameObject>("Grid");
            for (int i = 0; i < _fileList.Files.Count; i++)
            {
                var filename = _fileList.Files[i];
                var go =  GameObject.Instantiate(LevelToggle, LevelToggle.transform.parent);
                go.gameObject.SetActive(true);
                var toggle = go.gameObject.GetComponent<Toggle>();
                toggle.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = filename;
                toggle.onValueChanged.AddListener(delegate (bool isOn)
                {
                    if (isOn) 
                    {
                        LevelSelectPanel.gameObject.SetActive(false);
                        if (_gameGroup != null)
                        {
                            _gameGroup.SetActive(false);
                        }
                        OnLevelSelect(filename);
                    }
                });
                _levelToggles.Add(toggle);
            }
            LevelSelectPanel.gameObject.SetActive(true);
            if ( _gameGroup != null )
            {
                _gameGroup.SetActive(false);
            }
            
        }
        else {
            LevelSelectPanel.gameObject.SetActive(false);
            if (_gameGroup != null)
            {
                _gameGroup.SetActive(false);
            }
        }

    }

    public Transform LevelParent;
    public Transform Panel;
    public TextMeshProUGUI Tips;
    private GameObject _gameGroup;
    public void OnSuccess() 
    {
        Panel.gameObject.SetActive(true);
        Tips.text = string.Format("Success !");
    }
    public void OnNext()
    {
        Panel.gameObject.SetActive(false);
        ShowLevelSelect();
    }



    private void LoadLevel(JsonData jsonData)
    {
        if (_gameGroup != null)
        {
            UnityEngine.Object.DestroyImmediate(_gameGroup);
        }
        _gameGroup = new GameObject("Game");
        _gameGroup.transform.SetParent(this.transform);
        _gameGroup.transform.SetSiblingIndex(2);
        _gameGroup.transform.localPosition = Vector3.zero;
        _gameGroup.transform.localScale = Vector3.one;

        _gameGroup.transform.localPosition = new Vector2(jsonData.Canvas.PosX, jsonData.Canvas.PosY);


        for (int i = 0; i < jsonData.Grids.Count; i++)
        {
            var data = jsonData.Grids[i];
            GameObject plane = GameObject.Instantiate(_gridRes, _gameGroup.transform);
            plane.gameObject.SetActive(true);
            var grid = plane.GetComponent<Grid>();
            grid.SetPos(data.PosX, data.PosY);
            grid.Neg = data.Neg;
            grid.Enbale = data.Enbale;
            if (grid.Enbale == false) {
                grid.gameObject.SetActive(false);
            }
            plane.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(data.SizeX, data.SizeY);
            plane.transform.localPosition = new Vector3(data.DisplayX, data.DisplayY, 0);
        }
        _gameGroup.AddComponent<GameInput>();

        Panel.gameObject.SetActive(false);
    }
}
