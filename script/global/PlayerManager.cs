using Godot;
using System;

public partial class PlayerManager : Node
{
	public static PlayerManager Instance;
	private Node _currentPlayerSceneContext { get; set; }
	private Player _currentPlayer;
	
	public Player CurrentPlayer
	{
		get => _currentPlayer;
		set => _currentPlayer = value;
	}
	
	public override void _Ready()
	{
		if (Instance == null)
		{
		Instance = this;
		SetOwner(null);
		ProcessMode = ProcessModeEnum.Always;
		}
		else
		{
			QueueFree();
			return;
		}
	}
	
	//全局玩家位置生成
	public void SpawnPlayer(Node sceneContext, Marker2D spawnPoint)
	{
		if (_currentPlayer == null)
		{
			PackedScene playerScene = GD.Load<PackedScene>("res://gameScene/character/character_body_2d.tscn");
			_currentPlayer = playerScene.Instantiate<Player>();
			GetTree().Root.AddChild(_currentPlayer);
			_currentPlayer.Owner = GetTree().Root;
			GD.Print("PlayerManager       首次创建玩家节点");
		} 
		
		_currentPlayer.Visible = true;
		_currentPlayer.ProcessMode = ProcessModeEnum.Always;
		_currentPlayer.ZIndex = 100;
		
		if (spawnPoint != null)
		{
			_currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
		}
		
		_currentPlayerSceneContext = sceneContext;
		//GD.Print("KeepPlayerOnSceneChange()    ",_currentPlayerSceneContext);
	}
	
	public void KeepPlayerOnSceneChange()
	{
		if (_currentPlayer != null)
		{
			_currentPlayerSceneContext = null;
			//GD.Print("SpawnPlayer    ",_currentPlayerSceneContext);
		}
	}
	
	//切换场景前取消碰撞箱
	public void BeforeChangeScene()
	{
		if (CurrentPlayer == null) return;
		
		if (CurrentPlayer.GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D collision)
		{
			collision.Disabled = true;
		}
	}
	
	//切换场景完成恢复碰撞箱
	public void AfterChangeScene()
	{
		if (CurrentPlayer == null) return;
		
		if (CurrentPlayer.GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D collision)
		{
			collision.Disabled = false;
		}
	}
}
