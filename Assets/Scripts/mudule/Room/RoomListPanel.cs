using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomListPanel : BasePanel
{
    //账号文本
    private Text idText;
    //战绩文本
    private Text scoreText;
    //创建房间按钮
    private Button createButton;
    //刷新列表按钮
    private Button reflashButton;
    //列表容器
    private Transform content;
    //房间物体
    private GameObject roomObj;

    //初始化
    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        idText = skin.transform.Find("InfoPanel/IdText").GetComponent<Text>();
        scoreText = skin.transform.Find("InfoPanel/ScoreText").GetComponent<Text>();
        createButton = skin.transform.Find("CtrlPanel/CreateButton").GetComponent<Button>();
        reflashButton = skin.transform.Find("CtrlPanel/ReflashButton").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        roomObj = skin.transform.Find("Room").gameObject;
        //按钮事件
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        //不激活房间
        roomObj.SetActive(false);
        //显示id
        idText.text = GameMain.id;

        // 协议监听
        NetManager.AddMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.AddMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }

    public override void OnClose()
    {
        // 移除所有注册的协议监听
        NetManager.RemoveMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.RemoveMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.RemoveMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }
    //收到成绩查询协议,根据服务端返回的胜(win)负(lost)数，更新scoreText。
    public void OnMsgGetAchieve(MsgBase msgBase)
    {
        MsgGetAchieve msg = (MsgGetAchieve)msgBase;
        scoreText.text = msg.win + "胜" + msg.lost + "负";
    }

    //收到房间列表协议,会先删去已经生成的列表项。然后根据服务端发来的房间信息重新生成列表项
    public void OnMsgGetRoomList(MsgBase msgBase)
    {
        Debug.Log("客户端处理MsgGetRoomList协议");
        MsgGetRoomList msg = (MsgGetRoomList)msgBase;
        //清除房间列表
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //如果没有房间，不需要进一步处理
        if (msg.rooms == null)
        {
            Debug.Log("rooms == null，生成不了");
            return;
        }
        Debug.Log("开始生成房间了");
        for (int i = 0; i < msg.rooms.Length; i++)
        {
            // 创建列表项（房间信息）
            GenerateRoom(msg.rooms[i]);
        }
    }

    //创建一个房间单元
    public void GenerateRoom(RoomInfo roomInfo)
    {
        //创建物体
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;
        //获取组件
        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text statusText = trans.Find("StatusText").GetComponent<Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();
        //填充信息
        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        if (roomInfo.status == 0)
        {
            statusText.text = "准备中";
        }
        else
        {
            statusText.text = "战斗中";
        }
        //按钮事件，的是方便在按钮点击事件中获取房间ID。可以直接用局部变量捕获（如 OnJoinClick(idText.text)）
        btn.name = idText.text;
        //，OnJoinClick方法被调用，它的参数是按钮名字，即房间序号。点击按钮后，客户端将会向服务端发送MsgEnterRoom协议
        btn.onClick.AddListener(delegate ()
        {
            OnJoinClick(btn.name);
        });
    }

    //点击加入房间按钮
    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id = int.Parse(idString);
        NetManager.Send(msg);
    }

    //收到进入房间协议
    public void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        //成功进入房间
        if (msg.result == 0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        //进入房间失败
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败");
        }
    }

    //点击新建房间按钮
    public void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        NetManager.Send(msg);
    }

    //收到新建房间协议
    public void OnMsgCreateRoom(MsgBase msgBase)
    {
        Debug.Log("客户端OnMsgCreateRoom方法处理服务端发来的新建房间协议");
        MsgCreateRoom msg = (MsgCreateRoom)msgBase;
        //成功创建房间
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("创建成功");
            PanelManager.Open<RoomPanel>();
            Close();// 关闭RoomListPanel
        }
        //创建房间失败
        else
        {
            PanelManager.Open<TipPanel>("创建房间失败");
        }
    }

    //点击刷新按钮
    public void OnReflashClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        NetManager.Send(msg);
    }
    
}