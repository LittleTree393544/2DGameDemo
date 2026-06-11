using Godot;
using System;
using System.Collections.Generic;

public partial class NPCManager : Node
{
	public static NPCManager Instance;
	private Node _currentPlayerSceneContext { get; set; }
	private NPCController _currentNPC;
	//NPC是否已经生成
	private bool _isSpawn = false;
	
	public bool CanAlive;
	
	public NPCController CurrentNPC
	{
		get => _currentNPC;
		set => _currentNPC = value;
	}
	
	public override void _Ready()
	{
		if (Instance == null)
		{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
		CanAlive = false;
		}
		else
		{
			QueueFree();
			return;
		}
	}
	
	//全局NPC位置生成
	public void SpawnNPC(Node sceneContext, Marker2D spawnPoint)
	{
		if (_currentNPC == null)
		{
			PackedScene playerScene = GD.Load<PackedScene>("res://gameScene/character/brother.tscn");
			_currentNPC = playerScene.Instantiate<NPCController>();
			GetTree().Root.AddChild(_currentNPC);
			_currentNPC.Owner = GetTree().Root;
			GD.Print("NPCManager       首次创建NPC节点");
		}
		
		_currentNPC.Visible = true;
		_currentNPC.ProcessMode = ProcessModeEnum.Always;
		_currentNPC.ZIndex = 100;
		
		if (spawnPoint != null && _isSpawn == false)
		{
			_currentNPC.GlobalPosition = spawnPoint.GlobalPosition;
			_isSpawn = true;
		}
		
		_currentPlayerSceneContext = sceneContext;
	}
	
	public void KeepPlayerOnSceneChange()
	{
		if (_currentNPC != null)
		{
			_currentPlayerSceneContext = null;
			//GD.Print("SpawnPlayer    ",_currentPlayerSceneContext);
		}
	}
	
	//取消碰撞箱
	public void BeforeChangeScene()
	{
		if (_currentNPC == null) return;
		
		if (_currentNPC.GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D collision)
		{
			collision.Disabled = true;
		}
	}
	
	//恢复碰撞箱
	public void AfterChangeScene()
	{
		if (_currentNPC == null) return;
		
		if (_currentNPC.GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D collision)
		{
			collision.Disabled = false;
		}
	}
	
	
	
	//隐藏npc精灵图像
	public void HideSprite()
	{
		if (_currentNPC == null) return;
		
		if (_currentNPC.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D") is AnimatedSprite2D sprite)
		{
			sprite.Visible = false;
		}
	}
	
	//恢复npc精灵图像
	public void RestoreSprite()
	{
		if (_currentNPC == null) return;
		
		if (_currentNPC.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D") is AnimatedSprite2D sprite)
		{
			sprite.Visible =true;
		}
	}
}
	
