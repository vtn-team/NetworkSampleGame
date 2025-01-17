using UnityEngine;

/// <summary>
/// イベントレシーバーインタフェース
/// </summary>
public interface IEventReceiver
{
    //アクティブなレシーバーかどうか
    public bool IsActive { get; }

    //イベント受信関数
    void OnEventCall(EventData data);
}