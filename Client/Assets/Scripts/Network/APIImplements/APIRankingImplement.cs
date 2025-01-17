using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// スコア登録リクエスト
/// </summary>
[Serializable]
public class SendRankingRequest
{
    public string AppName;
    public string UserId;
    public int Score;
}

/// <summary>
/// スコア登録戻り値
/// </summary>
[Serializable]
public class SendRankingResult
{
    public int Status;
}

/// <summary>
/// ランキング戻り値
/// </summary>
[Serializable]
public class GetRankingResult
{
    public string AppName;              //アプリ名
    public RankingData[] RankingData;   //ランキングデータ
}

/// <summary>
/// ランキング実装
/// </summary>
public class APIRankingImplement
{
    async public UniTask<GetRankingResult> GetRanking()
    {
        string request = String.Format("{0}/ranking/list", NetworkManager.Environment.APIServerURI);
        string json = await Network.WebRequest.GetRequest(request);
        var ret = JsonUtility.FromJson<GetRankingResult>(json);
        return ret;
    }

    async public UniTask<SendRankingResult> SendRequest(int Score)
    {
        var req = new SendRankingRequest() { Score = Score  };
        string request = String.Format("{0}/ranking/register", NetworkManager.Environment.APIServerURI);
        string json = await Network.WebRequest.PostRequest(request, req);
        var ret = JsonUtility.FromJson<SendRankingResult>(json);
        return ret;
    }
}