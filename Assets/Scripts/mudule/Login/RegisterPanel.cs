using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel {
	//账号输入框
	private InputField idInput;
	//密码输入框
	private InputField pwInput;
	//重复输入框
	private InputField repInput;
	//注册按钮
	private Button regBtn;
	//关闭按钮
	private Button closeBtn;


	//初始化，在被打开时候调用
	public override void OnInit() {
		skinPath = "RegisterPanel";
		layer = PanelManager.Layer.Panel;
	}

	//显示
	public override void OnShow(params object[] args) {
		//寻找组件
		idInput = skin.transform.Find("InputId").GetComponent<InputField>();
		pwInput = skin.transform.Find("InputPw").GetComponent<InputField>();
		repInput = skin.transform.Find("InputRep").GetComponent<InputField>();
		regBtn = skin.transform.Find("btnRegister").GetComponent<Button>();
		closeBtn = skin.transform.Find("btnClose").GetComponent<Button>();
		//监听
		regBtn.onClick.AddListener(OnRegClick);
		closeBtn.onClick.AddListener(OnCloseClick);
		//网络协议监听
		NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
	}

	//关闭
	public override void OnClose() {
		//网络协议监听
		NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
	}

	//当按下注册按钮
	public void OnRegClick() {
		//用户名密码为空
		if (idInput.text == "" || pwInput.text == "") {
			PanelManager.Open<TipPanel>("用户名和密码不能为空");
			return;
		}
		//两次密码不同
		if (repInput.text != pwInput.text) {
			PanelManager.Open<TipPanel>("两次输入的密码不同");
			return;
		}
		//发送
		MsgRegister msgReg = new MsgRegister();
		msgReg.id = idInput.text;
		msgReg.pw = pwInput.text;
		NetManager.Send(msgReg);
	}

	//收到注册协议
	public void OnMsgRegister (MsgBase msgBase) {
		MsgRegister msg = (MsgRegister)msgBase;
		if(msg.result == 0){
			Debug.Log("注册成功");
			//提示
			PanelManager.Open<TipPanel>("注册成功");
			//关闭界面
			Close();
		}
		else{
			PanelManager.Open<TipPanel>("注册失败");
		}
	}

	//当按下关闭按钮
	public void OnCloseClick() {
		Close();
	}
}
