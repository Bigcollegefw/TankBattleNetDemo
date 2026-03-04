using System;
using System.Collections.Generic;

struct TroopItem
{
    public int id; 
    public int type;
    public int level;
    public int load;
    public int force;
    public int own_num;
    public int select_num;
}

class Task
{
    //资源地load上限，出兵数量上限，拥有的士兵种类列表
    public List<TroopItem> QuickSelectTroopList(int res_max, int march_size_max, List<TroopItem> own_troop_list)
    {
        if (res_max <= 0 || march_size_max <= 0 || own_troop_list == null)
            return new List<TroopItem>();

        List<TroopItem> validTroops = new List<TroopItem>();
        foreach (var troop in own_troop_list)
        {
            if (troop.own_num > 0 && troop.load > 0)
            {
                validTroops.Add(troop);
            }
        }

        if (validTroops.Count == 0)return new List<TroopItem>();

        // 按 load 从大到小排序：优先用 load 高的兵
        validTroops.Sort((a, b) => b.load.CompareTo(a.load));

        List<TroopItem> result = new List<TroopItem>();
        int remainingRes = res_max;     // 剩余可用资源容量
        int remainingMarch = march_size_max; // 剩余可派人数

        foreach (var troop in validTroops)
        {
            // 如果没容量了，或者没人可派了，就退出
            if (remainingRes <= 0 || remainingMarch <= 0)
                break;

            // 当前兵种最多能派几个
            // 1. 自己有多少兵
            // 2. 还剩多少出兵名额
            // 3. 剩余资源容量还能装几个
            int maxByOwn = troop.own_num;
            int maxByMarch = remainingMarch;
            int maxByRes = remainingRes / troop.load; 

            int sendCount = Math.Min(maxByOwn, Math.Min(maxByMarch, maxByRes));

            if (sendCount > 0)
            {
                TroopItem selected = new TroopItem();
                selected.id = troop.id;
                selected.type = troop.type;
                selected.level = troop.level;
                selected.load = troop.load;
                selected.force = troop.force;
                selected.own_num = troop.own_num;
                selected.select_num = sendCount;

                result.Add(selected);

                // 更新剩余值
                remainingRes -= sendCount * troop.load;
                remainingMarch -= sendCount;
            }
        }

        return result;
    }
}