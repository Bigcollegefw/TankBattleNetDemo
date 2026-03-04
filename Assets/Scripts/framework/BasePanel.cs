using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {
    //皮肤路径
    public string skinPath;
    //皮肤
    public GameObject skin;
    //层级
    public PanelManager.Layer layer = PanelManager.Layer.Panel;
    //初始化
    public void Init(){
        //皮肤
        GameObject skinPrefab = ResManager.LoadPrefab(skinPath);
        skin = (GameObject)Instantiate(skinPrefab);
    }
    //关闭
    public void Close(){
        string name = this.GetType().ToString();
        PanelManager.Close(name);
    }

    //初始化时
    public virtual void OnInit(){
    }
    //显示时
////params是C#开发语言中的关键字，主要的用处是给函数传递不定长度的参数。例如，以“tipPanel.OnShow（"呵呵"）​”调用面板类的OnShow方法，会得到(string)args[0]=="呵呵"，以“tipPanel.OnShow（"第一",1234）​”的形式调用，会得到(string)args[0]=="第一"，(int)args[1]==1234。
    public virtual void OnShow(params object[] para)
    {
    }
    //关闭时
    public virtual void OnClose(){
    }
}