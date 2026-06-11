using Godot;
using System;

// 场景可拾取道具基类
public partial class BasePickupItem : Area2D
{
	//道具唯一ID
	[Export] public string ItemId;
	
	//道具展示名称
	[Export] public string ItemShowName;
	
	//拾取范围半径
	//[Export] public float PickupRadius = InteractConst.INTERACT_RADIUS;
	
	//实例化PickupItemData类
	protected PickupItemData _pickupData;
	
	private bool _playerInRange = false;
	
	private bool _isPicked = false;
	
	public override void _Ready()
	{
		
		_pickupData = new PickupItemData()
		{
			ItemId = ItemId,
			ItemName = ItemShowName
		};
		
		BodyEntered += OnPlayerEnter;
		BodyExited += OnPlayerExit;
	}
	
	//玩家进入拾取范围
	private void OnPlayerEnter(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = true;
			
			PlayerInteractManager.Instance.SetCurrentPickupTarget(this);
		}
	}
	
	//玩家离开拾取范围
	private void OnPlayerExit(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = false;
			
			PlayerInteractManager.Instance.ClearPickupTarget(this);
		}
	}
	
	//道具拾取逻辑
	public void PickupItem()
	{
		if(!_playerInRange || _isPicked) return;
		
		_isPicked = true;
		
		PlayerInventoryManager.Instance.AddItem(_pickupData.ItemId);
		
		GetParent().QueueFree();
		
		OnPickupSuccess();
		
		GD.Print("成功拾取道具：",ItemShowName,ItemId);
	}
	
	//拾取成功逻辑
	protected virtual void OnPickupSuccess()
	{
		
	}
}
