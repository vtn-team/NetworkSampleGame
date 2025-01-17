using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// セーブ登録リクエスト
/// </summary>
[Serializable]
public class SendGameSaveRequest
{
    public string SaveKey;
    public string SaveValue;
}

/// <summary>
/// ロード結果
/// </summary>
[Serializable]
public class GameLoadResult
{
    public int Status;
    public string GameData;
}

/// <summary>
/// 登録結果
/// </summary>
[Serializable]
public class GameSaveResult
{
    public int Status;
}

/// <summary>
/// ゲームセーブ実装
/// NOTE: クラスはシリアライズ可能であること
/// </summary>
public class APIGameSaveImplement
{
    async public UniTask<T> Load<T>(string saveKey)
    {
        string request = String.Format("{0}/load/{1}", NetworkManager.Environment.APIServerURI, saveKey);
        string json = await Network.WebRequest.GetRequest(request);
        var ret = JsonUtility.FromJson<GameLoadResult>(json);
        if (ret.GameData != "")
        {
            var result = JsonUtility.FromJson<T>(ret.GameData);
            return result;
        }
        return default(T);
    }
    async public UniTask<GameSaveResult> Save<T>(string saveKey, T data)
    {
        SendGameSaveRequest req = new SendGameSaveRequest();
        req.SaveKey = saveKey;
        req.SaveValue = JsonUtility.ToJson(data);
        string request = String.Format("{0}/save", NetworkManager.Environment.APIServerURI);
        string json = await Network.WebRequest.PostRequest(request, req);
        var ret = JsonUtility.FromJson<GameSaveResult>(json);
        return ret;
    }
}