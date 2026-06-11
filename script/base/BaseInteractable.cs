using Godot;
using System;

//可交互物体基类
public partial class BaseInteractable : Area2D
{
	[Export] public string ItemName;
	[Export] public bool NeedSpecificItem;
	[Export] public string RequireItemId;
	[Export] public bool OnlyOnce = false;
	
	//实例化InteractItemData类
	protected InteractItemData _interactData;
	
	//是否在玩家交互范围内
	protected bool _inPlayerRange;
	
	//初始化
	public override void _Ready()
	{
		
		_interactData = new InteractItemData()
		{
			NeedSpecificItem = NeedSpecificItem,
			NeedItemId = RequireItemId,
			IsOnceInteract = OnlyOnce,
			IsInteracted = false
		};
		
		//监听
		BodyEntered += OnPlayerEnterRange;
		BodyExited += OnPlayerExitRange;
	}
	
	
	//玩家进入交互范围
	protected virtual void OnPlayerEnterRange(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("进入交互范围");
			_inPlayerRange = true;
			
			PlayerInteractManager.Instance.SetCurrentInteractTarget(this);
		}
	}
	
	//玩家离开交互范围
	protected virtual void OnPlayerExitRange(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("离开交互范围");
			_inPlayerRange = false;
			PlayerInteractManager.Instance.ClearInteractTarget(this);
		}
	}
	
	public void DoInteract()
	{
		//单次交互次数判断
		if (_interactData.IsOnceInteract && _interactData.IsInteracted) return;
		
		// 是否需要道具
		if (_interactData.NeedSpecificItem)
		{
			if (!PlayerInventoryManager.Instance.HasItem(_interactData.NeedItemId))
			{
				OnInteractFailed();
				return;
			}
		}
		
		OnInteractSuccess();
		
		if (_interactData.IsOnceInteract)
		{
			_interactData.IsInteracted = true;
		}
	}
	
	//交互成功逻辑
	protected virtual void OnInteractSuccess()
	{
		GD.Print("交互成功");
	}
	
	//交互失败逻辑
	protected virtual void OnInteractFailed()
	{
		GD.Print("{ItemName}需要指定道具才能交互！");
	}
	
	public InteractItemData GetInteractData()
	{
		return _interactData;
	}
	
	public bool IsInInteractRange()
	{
		return _inPlayerRange;
	}
}
