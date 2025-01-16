using Cysharp.Threading.Tasks;
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
    /// <summary>ゲーム終了</summary>
    static async public UniTask<SendRankingResult> SendRanking(int Score) { return await _instance.SendRankingImplement(Score); }

    //ゲームサーバーまわり。
    /// <summary>イベントを受信するクラスを登録(何個登録しても良い)</summary>
    static public void RegisterEventReceiver(IEventReceiver receiver) { _instance.RegisterReceiver(receiver); }
    /// <summary>イベントを送信(イベントデータを作って送信)</summary>
    static public void SendEvent(EventData data) { _instance.SendVCEvent(data); }


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
    /// システム初期化
    /// NOTE: 各種システムを起動時の状態に戻す
    /// </summary>
    void SystemResetImplement()
    {
        _eventSystem.Reset();
    }

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
        return await _rankingAPI.SendRequest(score);
    }

    //イベント系
    void RegisterReceiver(IEventReceiver receiver)
    {
        _eventSystem.RegisterReceiver(receiver);
    }

    void SendVCEvent(EventData d)
    {
        _eventSystem.SendEvent(d);
    }

#endregion
}