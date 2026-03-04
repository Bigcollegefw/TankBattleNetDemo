using UnityEngine;

//挂上这个脚本的是那些别的玩家的坦克
public class SyncTank : BaseTank
{
    //预测信息，哪个时间到达哪个位置
    private Vector3 lastPos;//最近一次收到的位置同步协议(MsgSyncTank)的位置
    private Vector3 lastRot;//最近一次收到的旋转同步协议(MsgSyncTank)的位置
    private Vector3 forecastPos; //预测的信息
    private Vector3 forecastRot;
    private float forecastTime; //forecastTime代表最近一次收到的位置同步协议的时间。


    new void Update()
    {
        base.Update();
        //更新位置
        ForecastUpdate();
    }

    //重写Init
    // 1)冻结rigidBody，让坦克不受物理系统影响；2)设置useGravity，让坦克不受重力影响；3)初始化lastPos、lastRot等成员。
    public override void Init(string skinPath)
    {
        base.Init(skinPath);
        //不受物理运动影响
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.useGravity = false;
        //初始化预测信息
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        forecastPos = transform.position;
        forecastRot = transform.eulerAngles;
        forecastTime = Time.time;
    }

    //更新位置
    // forecastPos forecastRot forecastTime都需要收到协议后才更新，那这个预测都是在服务端算好的？
    public void ForecastUpdate()
    {
        //时间
        float t = (Time.time - forecastTime) / CtrlTank.syncInterval; //同步帧率0.1
        t = Mathf.Clamp(t, 0f, 1f);
        //位置
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;
        //旋转
        Quaternion quat = transform.rotation;
        Quaternion forcastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forcastQuat, t);
        transform.rotation = quat;
    }

    //更具服务端的协议来处理得到移动同步的数据
    public void SyncPos(MsgSyncTank msg)
    {
        //预测位置
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        forecastPos = pos + 2 * (pos - lastPos);
        forecastRot = rot + 2 * (rot - lastRot);
        //更新
        lastPos = pos;
        lastRot = rot;
        forecastTime = Time.time;
        //炮塔
        Vector3 le = turret.localEulerAngles;
        le.y = msg.turretY;
        turret.localEulerAngles = le;
    }

    //更具服务端的协议来处理得到子弹的数据
    public void SyncFire(MsgFire msg)
    {
        Bullet bullet = Fire();
        //更新坐标
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        bullet.transform.position = pos;
        bullet.transform.eulerAngles = rot;
    }
}