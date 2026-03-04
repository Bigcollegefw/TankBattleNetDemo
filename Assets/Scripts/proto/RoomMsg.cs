//查询成绩
public class MsgGetAchieve : MsgBase
{
    public MsgGetAchieve() { protoName = "MsgGetAchieve"; }
    //服务端回玩家的总胜利次数win和总失败次数lost。
    public int win = 0;
    public int lost = 0;
}

//房间信息，Unity的JsonUtility才能够正确解析rooms数组
[System.Serializable]
public class RoomInfo{
    public int id = 0;        //房间id
    public int count = 0;     //人数
    public int status = 0;    //状态0-准备中1-战斗中
}

//请求房间列表
public class MsgGetRoomList : MsgBase
{
    public MsgGetRoomList() { protoName = "MsgGetRoomList"; }
    //服务端也会有一个RoomInfo[]，这样接受服务端发回来的消息后就可用这个协议类点出来使用
    public RoomInfo[] rooms;
}

//创建房间
public class MsgCreateRoom : MsgBase
{
    public MsgCreateRoom() { protoName = "MsgCreateRoom"; }
    //服务端回result为0代表创建成功，其他数值代表创建失败。
    public int result = 0;
}

//进入房间
//加入房间时将房间序号(id)发送给服务端，服务端把玩家添加到房间中。服务端的返回值result代表执行结果，result为0代表成功进入，其他数值代表进入失败。例如玩家已经在房间中，就不能重复进入。
public class MsgEnterRoom : MsgBase
{
    public MsgEnterRoom() { protoName = "MsgEnterRoom"; }
    //客户端发
    public int id = 0;
    //服务端回
    public int result = 0;
}


//玩家信息
[System.Serializable]
//告诉客户端房间里所有玩家的信息，包括账号(id)、所在队伍(camp)、胜利总数(win)、失败总数(lost)、是否是房主(isOwner)。camp的取值为1或者2，代表在第一个阵营或者第二个阵营；如果isOwner为1，代表玩家是房主，如果为0，代表是普通成员。
public class PlayerInfo{
    public string id = "lpy";    //账号
    public int camp = 0;         //阵营
    public int win = 0;          //胜利数
    public int lost = 0;         //失败数
    public int isOwner = 0;      //是否是房主
}

//获取房间信息，如有玩家加入或离开房间，服务端还会给房间里的所有玩家推送该协议，让客户端更新界面。
public class MsgGetRoomInfo : MsgBase
{
    public MsgGetRoomInfo() { protoName = "MsgGetRoomInfo"; }
    //服务端回
    public PlayerInfo[] players;
}

//离开房间，result为0代表离开成功，其他数值代表离开失败（如玩家不在房间中却发送离开房间的协议）​。
public class MsgLeaveRoom : MsgBase
{
    public MsgLeaveRoom() { protoName = "MsgLeaveRoom"; }
    //服务端回
    public int result = 0;
}

//开战，房主点击开始战斗按钮时，客户端会发送MsgStartBattle协议。服务端的返回值result代表开始战斗的结果，result为0代表成功，其他数值代表失败。
public class MsgStartBattle : MsgBase
{
    public MsgStartBattle() { protoName = "MsgStartBattle"; }
    //服务端回
    public int result = 0;
} 


