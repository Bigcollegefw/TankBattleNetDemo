using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {
    // 但在多人网络游戏中，每个玩家运行的都是自己独立的客户端程序，每个客户端进程有自己的 GameMain.id。
    public static string id = ""; //用于记录玩家角色id

    // Use this for initialization
    void Start () {
        //网络监听
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //初始化
        PanelManager.Init();
        BattleManager.Init();
        //打开登录面板
        PanelManager.Open<LoginPanel>();
    }

    //关闭连接
    void OnConnectClose(string err){
        Debug.Log("GameMain收到：断开连接");
    } 

    //被踢下线
    void OnMsgKick(MsgBase msgBase){
        PanelManager.Open<TipPanel>("被踢下线");
    }

    // Update is called once per frame
    void Update () {
        NetManager.Update();
    }
}