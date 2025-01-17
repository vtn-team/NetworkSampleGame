using System;

public enum EnvironmentSetting
{
    Local,
    Develop,
    Production
};

/// <summary>
/// システム情報保存クラス
/// </summary>
[Serializable]
public class SystemSaveData
{
    public string Version = "0.1";          //バージョン情報

    //環境設定
    public EnvironmentSetting Environment = EnvironmentSetting.Develop; //環境設定

    public string AppName = "NetworkSample";                //アプリ名
    public string UserName = "Nanashi";                     //ランキング時に使用する名前
    public string SaveKey = Guid.NewGuid().ToString();      //セーブ時に使用するキー

    public bool IsMMONetwork = false;           //
}