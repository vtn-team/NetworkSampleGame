
using System;
using UnityEngine;

enum WebSocketCommand
{
    WELCOME = 1,
    JOIN = 2,
    EVENT = 3,
    SEND_JOIN = 100,
    SEND_EVENT = 101
};

[Serializable]
public class WebSocketPacket
{
    public string UserId;
    public int Command;
    public string Data;
};

[Serializable]
public class WSPR_Welcome
{
    public string SessionId;
};

[Serializable]
public class WSPR_Join
{
    public string UserId;
    public string UserName;
};

[Serializable]
public class WSPS_SendEvent : EventData
{
    public string SessionId;
    public int Command = (int)WebSocketCommand.SEND_EVENT;

    public WSPS_SendEvent(string sessionId, EventData d) : base(d)
    {
        SessionId = sessionId;
    }
};

[Serializable]
public class WSPS_Join
{
    public string UserId;
    public string UserName;
    public string SessionId;
    public int Command = (int)WebSocketCommand.SEND_JOIN;
};
