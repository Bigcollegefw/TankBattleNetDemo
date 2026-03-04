using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    // Use this for initialization
    public void Start()
    {
        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
        //坦克
        GameObject tankObj = new GameObject("myTank");
        CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
        ctrlTank.Init("tankPrefab");
        //相机
        tankObj.AddComponent<CameraFollow>();

        GameObject tankObj2 = new GameObject("enemyTank");
        BaseTank baseTank = tankObj2.AddComponent<BaseTank>();
        baseTank.Init("tankPrefab");
        baseTank.transform.position = new Vector3(0,10,30);
    }
}