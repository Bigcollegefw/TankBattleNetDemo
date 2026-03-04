# TankBattleNetDemo

坦克大乱斗 - 网络对战游戏客户端

## 项目简介

这是一个Unity开发的坦克对战多人网络游戏客户端。

## 技术栈

- **引擎**: Unity
- **编程语言**: C#
- **网络通信**: TCP Socket
- **后端**: C++ 服务端（独立项目）
- **数据库**: MySQL（部署在虚拟机上）

## 项目结构

```
Assets/
├── Scripts/          # 脚本代码
│   ├── framework/   # 框架基础（网络、面板管理等）
│   ├── mudule/      # 业务模块
│   │   ├── Battle/  # 战斗模块
│   │   ├── Login/   # 登录模块
│   │   ├── Room/    # 房间模块
│   │   └── Common/ # 公共模块
│   └── proto/       # 协议定义
├── Resources/       # 预制体资源
├── Scenes/          # 场景
├── TankPrefab/     # 坦克模型
├── TerrainAsset/   # 地形资源
└── ui/              # UI资源
```

## 相关项目

- **服务端**: [TankBattleNetServer](https://github.com/Bigcollegefw/TankBattleNetServer) - C++ 编写的游戏服务器

## 运行说明

1. 使用 Unity Hub 打开本项目
2. 确保 Unity 版本兼容（建议 2020.3+）
3. 打开 Scenes/SampleScene 场景
4. 点击 Play 运行游戏

## 网络配置

游戏连接的服务端地址和端口需要在代码中配置（`NetManager.cs`）。

---
