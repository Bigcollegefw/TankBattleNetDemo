using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoginPanel : BasePanel
{
    //账号输入框
    private InputField idInput;
    //密码输入框
    private InputField pwInput;
    //登录按钮
    private Button loginBtn;
    //注册按钮
    private Button regBtn;

    //初始化
    // /设置了皮肤地址skinPath和面板的层级layer；
    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        idInput = skin.transform.Find("InputId").GetComponent<InputField>();
        pwInput = skin.transform.Find("InputPw").GetComponent<InputField>();
        loginBtn = skin.transform.Find("btnLogin").GetComponent<Button>();
        regBtn = skin.transform.Find("btnRegister").GetComponent<Button>();
        //监听
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);
        // 网络协议监听
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        // 网络事件监听
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        // 连接服务器
        NetManager.Connect("127.0.0.1", 8888);
    }

    private void OnConnectFail(string err)
    {

    }

    private void OnConnectSucc(string err)
    {

    }

    public override void OnClose()
    {
        // 网络协议监听
        NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
        // 网络事件监听
        NetManager.RemoveEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.RemoveEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
    }

    //当按下登录按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (idInput.text == "" || pwInput.text == "")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }
        Debug.Log("为什么发不出去");
        //发送
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        NetManager.Send(msgLogin);
    }

    //当按下注册按钮
    public void OnRegClick()
    {
        PanelManager.Open<RegisterPanel>();
    }

    //收到登录协议
    public void OnMsgLogin (MsgBase msgBase) {
            MsgLogin msg = (MsgLogin)msgBase;
            if(msg.result == 0){
                Debug.Log("登录成功");
                // 设置id
                GameMain.id = msg.id;
                // 打开房间列表界面
                PanelManager.Open<RoomListPanel>();
                //关闭界面
                Close();
            }
            else{
                PanelManager.Open<TipPanel>("登录失败");
            }
    }
}
