using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using UnityEditor.UIElements;
using System.Linq;

public class ItemEditor : EditorWindow
{
    [MenuItem("MyTools/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    private VisualTreeAsset rowTemplate;
    private ListView itemListView;
    private ScrollView scrollView;
    private ItemDetails activeItem;
    private Sprite defaultIcon;

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/ItemRowTemplate.uxml");
        scrollView = root.Q<ScrollView>("ItemDetails");
        //defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");
        root.Q<Button>("AddButton").clicked += OnAddButtonClick;
        root.Q<Button>("DeleteButton").clicked += OnDeletedButtonClick;

        LoadDataBase();
        GenerateListView();
    }

    private void OnAddButtonClick()
    {
        ItemDetails item = new ItemDetails();
        item.itemName = "New Item";
        item.itemID = 1000 + itemList.Count;
        itemList.Add(item);
        itemListView.Rebuild();
    }

    private void OnDeletedButtonClick()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        scrollView.visible = false;
    }

    private ItemData_SO database;
    private List<ItemDetails> itemList = new List<ItemDetails>();
    private void LoadDataBase() 
    {
        var dataArray = AssetDatabase.FindAssets("ItemData_SO");
        if (dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            database = AssetDatabase.LoadAssetAtPath<ItemData_SO>(path);
        }
        EditorUtility.SetDirty(database);
        itemList = database.itemDetailsList;
        //Debug.Log(itemList[0].itemName);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => rowTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
            }
        };
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
        itemListView.onSelectionChange += (IEnumerable<object> selectedItem) =>
        {
            activeItem = (ItemDetails)selectedItem.First();
            SetItemDetails();
            scrollView.visible = true;
        };
    }

    private void SetItemDetails()
    {
        scrollView.MarkDirtyRepaint();
        
        scrollView.Q<IntegerField>("ItemID").value = activeItem.itemID;
        scrollView.Q<IntegerField>("ItemID").RegisterValueChangedCallback
         (evt =>
         {
             activeItem.itemID = evt.newValue;
         });

        scrollView.Q<TextField>("ItemName").value = activeItem.itemName;
        scrollView.Q<TextField>("ItemName").RegisterValueChangedCallback
         (evt =>
         {
             activeItem.itemName = evt.newValue;
             itemListView.Rebuild();
         });

        scrollView.Q<VisualElement>("Icon").style.backgroundImage = 
            activeItem.itemIcon != null ? activeItem.itemIcon.texture : defaultIcon.texture;

        scrollView.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        scrollView.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback
         (evt =>
         {
             Sprite icon = evt.newValue as Sprite;
             activeItem.itemIcon = icon;
             scrollView.Q<VisualElement>("Icon").style.backgroundImage =
                     icon != null ? icon.texture : defaultIcon.texture;
             itemListView.Rebuild();
         });

        scrollView.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorld;
        scrollView.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorld = (Sprite)evt.newValue;
        });

        scrollView.Q<EnumField>("ItemType").Init(activeItem.itemType);
        scrollView.Q<EnumField>("ItemType").value = activeItem.itemType;
        scrollView.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        scrollView.Q<TextField>("Description").value = activeItem.itemDescription;
        scrollView.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        scrollView.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        scrollView.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        scrollView.Q<Toggle>("CanPickedup").value = activeItem.canPickedup;
        scrollView.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });

        scrollView.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        scrollView.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        scrollView.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        scrollView.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        scrollView.Q<IntegerField>("Price").value = activeItem.itemPrice;
        scrollView.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        scrollView.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        scrollView.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}