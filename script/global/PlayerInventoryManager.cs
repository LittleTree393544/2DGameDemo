using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInventoryManager : Node
{
	public static PlayerInventoryManager Instance { get; private set; }
	
	//存储玩家所有道具ID
	private HashSet<string> _itemPool = new HashSet<string>();
	
	public override void _Ready()
	{
		if (Instance == null) Instance = this;
		else QueueFree();
	}
	
	//添加道具
	public void AddItem(string itemId)
	{
		if (!_itemPool.Contains(itemId))
		{
			_itemPool.Add(itemId);
		}
	}
	
	//判断是否拥有道具
	public bool HasItem(string itemId)
	{
		return _itemPool.Contains(itemId);
	}
	
	//消耗道具
	public void RemoveItem(string itemId)
	{
		if (_itemPool.Contains(itemId))
		{
			_itemPool.Remove(itemId);
		}
	}
}
