using Cysharp.Threading.Tasks;
using System;
using UnityEngine;


/// <summary>
/// ネットワークまわりここに全部詰め込んだクラス
/// NOTE: すべてここからアクセスできます。
/// </summary>
public class NetworkManager
{
    //公開インタフェース

    /// <summary>環境変数を返す</summary>
    static public IEnvironment Environment => _instance._environment;
    /// <summary>システム変数を返す</summary>
    static public SystemSaveData SystemSave => _instance._systemSave;


    //APIまわり。非同期関数
    /// <summary>ランキング取得</summary>
    static async public UniTask<GetRankingResult> GetRanking() { return await _instance.GetRankingImplement(); }
    /// <summary>スコア送信</summary>
    static async public UniTask<SendRankingResult> SendRanking(int Score) { return await _instance.SendRankingImplement(Score); }

    /// <summary>ロード</summary>
    static async public UniTask<T> LoadGameData<T>() { return await _instance.LoadGameDataImplement<T>(); }
    /// <summary>セーブ</summary>
    static async public UniTask<GameSaveResult> SaveGameData<T>(T save) { return await _instance.SaveGameDataImplement<T>(save); }


    //ゲームサーバーまわり。
    /// <summary>WebSocketの準備ができているか</summary>
    static public bool IsSetup => _instance._wsManager.IsConnecting;
    /// <summary>接続に必要なユーザIDを取得</summary>
    static public string GetUserId() { return _instance._userId; }
    /// <summary>イベントを受信するクラスを登録(何個登録しても良い)</summary>
    static public void RegisterEventReceiver(IEventReceiver receiver) { _instance.RegisterEventReceiverImplement(receiver); }

    /// <summary>ネットワークに参加</summary>
    static public void JoinNetwork() { _instance.JoinNetworkImplement(); }

    /// <summary>イベントを送信(イベントデータを作って送信)</summary>
    static public void SendEvent(EventData data) { _instance.SendEventImplement(data); }


    #region 内部処理用
    static NetworkManager _instance = new NetworkManager();
    NetworkManager() { }


    //それぞれの処理委譲系
    EventSystem _eventSystem = new EventSystem();
    SystemSaveData _systemSave = new SystemSaveData();

    //セットアップ時に実態が生まれるもの
    IEnvironment _environment = null;
    WebSocketEventManager _wsManager = null;

    //APIリスト
    APIRankingImplement _rankingAPI = new APIRankingImplement();
    APIGameSaveImplement _gameSave = new APIGameSaveImplement();

    //ユーザID
    string _userId = "";

    //内部インタフェース

    //エントリポイント(ゲーム開始時にコールされる)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Run()
    {
        Debug.Log("NetworkManager Setup Ready");

        //システムセーブのロード
        var systemSave = LocalData.Load<SystemSaveData>("SystemSave.json");
        if(systemSave == null)
        {
            systemSave = new SystemSaveData();
        }
        _instance._systemSave = systemSave;

        //環境構成
        switch(systemSave.Environment)
        {
            case EnvironmentSetting.Local:
                _instance._environment = new LocalEnvironment();
                break;

            case EnvironmentSetting.Develop:
            case EnvironmentSetting.Production:
                _instance._environment = new ProductionEnvironment();
                break;
        }

        //常駐する管理オブジェクトの生成
        GameObject obj = new GameObject("WS");
        _instance._wsManager = obj.AddComponent<WebSocketEventManager>();
        GameObject.DontDestroyOnLoad(obj);

        //イベント登録系など
        _instance._wsManager.SetEventSystem(_instance._eventSystem);
        _instance._eventSystem.SystemInitialSave();
    }



    // ゲーム管理系 ///////////////////

    /// <summary>
    /// ランキングの実装
    /// </summary>
    async UniTask<GetRankingResult> GetRankingImplement()
    {
        return await _rankingAPI.GetRanking();
    }

    /// <summary>
    /// スコア送信の実装
    /// </summary>
    async UniTask<SendRankingResult> SendRankingImplement(int score)
    {
        return await _rankingAPI.SendRequest(score, 0);
    }

    /// <summary>
    /// ロード実装
    /// </summary>
    async UniTask<T> LoadGameDataImplement<T>()
    {
        return await _gameSave.Load<T>(_systemSave.SaveKey);
    }

    /// <summary>
    /// セーブ実装
    /// </summary>
    async UniTask<GameSaveResult> SaveGameDataImplement<T>(T data)
    {
        return await _gameSave.Save(_systemSave.SaveKey, data);
    }


    //イベント系

    /// <summary>
    /// イベント登録
    /// NOTE: 購読している人全員に飛ぶ
    /// </summary>
    void RegisterEventReceiverImplement(IEventReceiver receiver)
    {
        _eventSystem.RegisterReceiver(receiver);
    }

    /// <summary>
    /// ネットワーク参加
    /// NOTE: 
    /// </summary>
    void JoinNetworkImplement()
    {
        _userId = Guid.NewGuid().ToString();
        _wsManager.Join(_userId, NetworkManager.SystemSave.UserName);
    }

    /// <summary>
    /// イベントの送信
    /// NOTE: 
    /// </summary>
    void SendEventImplement(EventData d)
    {
        _eventSystem.SendEvent(d);
    }

#endregion
}