using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_SO",menuName="Inventory/ItemData")] //Unity的Create菜单里多出来一项
public class ItemData_SO : ScriptableObject//在Inspetor面板上显示
{
    public List<ItemDetails> itemDetailsList;
}
