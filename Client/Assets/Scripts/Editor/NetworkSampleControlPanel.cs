using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// コントロールパネル
/// </summary>
public class NetworkSampleControlPanel : EditorWindow
{
    [MenuItem("NetworkSampleMenu/Open ControlPanel")]
    static void CreateWindow()
    {
        var window = (NetworkSampleControlPanel)EditorWindow.GetWindow(typeof(NetworkSampleControlPanel));
        window.Show();
        window.Init();
    }

    SystemSaveData _saveData = null;

    void Init()
    {
        //エディタプレイ中とプレイ外で挙動を変化させる
        if (_saveData == null)
        {
            //エディタの際はファイルから読む
            var systemSave = LocalData.Load<SystemSaveData>("SystemSave.json");
            if (systemSave == null)
            {
                systemSave = new SystemSaveData();
            }
            _saveData = systemSave;

            if (systemSave.SaveKey == "")
            {
                systemSave.SaveKey = Guid.NewGuid().ToString();
            }
        }
    }

    bool CheckParam<T>(ref T baseParam, T editorParam)
    {
        bool isDirty = !baseParam.Equals(editorParam);
        if (isDirty)
        {
            baseParam = editorParam;
        }
        return isDirty;
    }

    /// <summary>
    /// インスペクタ上で設定
    /// </summary>
    public void OnGUI()
    {
        //base.OnInspectorGUI();

        var headerStyle = new GUIStyle();
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 20;
        headerStyle.normal.textColor = Color.white;

        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("サンプル", headerStyle);
        EditorGUILayout.Space(10);
        if (GUILayout.Button("実装サンプルを開く", GUILayout.Width(200)))
        {
            System.Diagnostics.Process.Start(Application.dataPath + "/Scene/SampleGame");
        }

        EditorGUILayout.Space(30);

        //システムセーブ監視
        if (_saveData != null)
        {
            bool isDirty = false;
            EditorGUILayout.LabelField("システム設定", headerStyle);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("環境設定系");
            isDirty |= CheckParam(ref _saveData.Environment, (EnvironmentSetting)EditorGUILayout.EnumPopup("APIの環境ターゲット", _saveData.Environment, GUILayout.Width(400)));

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("ネットワークに使用する情報");

            EditorGUIUtility.labelWidth = 250;
            isDirty |= CheckParam(ref _saveData.AppName, EditorGUILayout.TextField("ランキングに載せるアプリ名", _saveData.AppName, GUILayout.Width(400)));
            isDirty |= CheckParam(ref _saveData.SaveKey, EditorGUILayout.TextField("ネットワークセーブに使用するキー", _saveData.SaveKey, GUILayout.Width(400)));
            isDirty |= CheckParam(ref _saveData.UserName, EditorGUILayout.TextField("ランキング、チャットサンプルに使用する名前", _saveData.UserName, GUILayout.Width(400)));

            isDirty |= CheckParam(ref _saveData.IsMMONetwork, EditorGUILayout.Toggle("ネットワークに参加する", _saveData.IsMMONetwork, GUILayout.Width(400)));

            if (isDirty)
            {
                Debug.Log("システムデータを保存");
                LocalData.Save<SystemSaveData>("SystemSave.json", _saveData);
            }
        }
    }
}