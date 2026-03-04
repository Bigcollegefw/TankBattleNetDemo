using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlTank : BaseTank
{
    // 上一次发送同步信息的时间
    private float lastSendSyncTime = 0;
    // 同步帧率
    public static float syncInterval = 0.1f;

    new void Update()
    {
        base.Update();
        //移动控制
        MoveUpdate();
        TurretUpdate();
        FireUpdate();
        SyncUpdate();
    }

    //移动控制
    public void MoveUpdate()
    {
        if (IsDie())
        {
            return;
        }
        //旋转
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);
        //前进后退
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.transform.position += s;
    }

    public void TurretUpdate()
    {
        if (IsDie())
        {
            return;
        }
        float axis = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            axis = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            axis = 1;
        }

        //旋转角度
        Vector3 le = turret.localEulerAngles;

        le.y += axis * Time.deltaTime * turretSpeed;

        turret.localEulerAngles = le;
    }
    //开炮
    public void FireUpdate()
    {
        if (IsDie())
            return;
        //按键判断
        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }
        //cd是否判断
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }
        //发射
        Bullet bullet = Fire();
        MsgFire msg = new MsgFire();
        msg.x = bullet.transform.position.x;
        msg.y = bullet.transform.position.y;
        msg.z = bullet.transform.position.z;
        //欧拉角直观、易传输、易调试，适合大多数同步场景。四元数更专业、无万向节锁，但不直观，数据量略大。
        msg.ex = bullet.transform.eulerAngles.x;
        msg.ey = bullet.transform.eulerAngles.y;
        msg.ez = bullet.transform.eulerAngles.z;
        NetManager.Send(msg);
        
    } 

    //发送同步信息
    public void SyncUpdate(){
        //时间间隔判断
        if(Time.time - lastSendSyncTime < syncInterval){
            return;
        }
        lastSendSyncTime = Time.time;
        //发送同步协议
        MsgSyncTank msg = new MsgSyncTank();
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        msg.ex = transform.eulerAngles.x;
        msg.ey = transform.eulerAngles.y;
        msg.ez = transform.eulerAngles.z;
        msg.turretY = turret.localEulerAngles.y;
        NetManager.Send(msg);
    }
}