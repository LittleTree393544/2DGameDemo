using Godot;
using System;
using System.Collections.Generic;

//玩家交互总管理器
public partial class PlayerInteractManager : Node
{
	public static PlayerInteractManager Instance { get; private set; }
	
	//当前范围内可交互物体
	private List<BaseInteractable> _currentInteractTargets = new List<BaseInteractable>();
	
	//当前范围可以拾取物品
	private List<BasePickupItem> _currentPickupTargets = new List<BasePickupItem>();
	
	public override void _Ready()
	{
		if (Instance == null) Instance = this;
		else QueueFree();
	}
	
	//玩家尝试互动
	public void TryInteract()
	{
		if (_currentInteractTargets.Count <= 0) return;
		_currentInteractTargets[0].DoInteract();
	}
	
	//设置当前可以交互物品
	public void SetCurrentInteractTarget(BaseInteractable interactable)
	{
		if (!_currentInteractTargets.Contains(interactable))
		{
			_currentInteractTargets.Add(interactable);
			//GD.Print(interactable);
		}
	}
	
	//清除离开范围的物品
	public void ClearInteractTarget(BaseInteractable interactable)
	{
		if (_currentInteractTargets.Contains(interactable))
		{
			_currentInteractTargets.Remove(interactable);
		}
	}
	
	//玩家尝试拾取物品
	public void TryPickup()
	{
		if (_currentPickupTargets.Count <= 0) return;
		
		_currentPickupTargets[0].PickupItem();
	}
	
	//设置可以拾取物品
	public void SetCurrentPickupTarget(BasePickupItem pickupable)
	{
		if (!_currentPickupTargets.Contains(pickupable))
		{
			_currentPickupTargets.Add(pickupable);
		}
	}
	
	//移除可拾取物品
	public void ClearPickupTarget(BasePickupItem pickupable)
	{
		if (_currentPickupTargets.Contains(pickupable))
		{
			_currentPickupTargets.Remove(pickupable);
		}
	}
}
