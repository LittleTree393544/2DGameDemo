using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

//自动巡路、停留、循环路线
public partial class NPCController : CharacterBody2D
{
	[ExportGroup("移动设置")]
	//移动速度
	[Export] public float MoveSpeed = 120f; 
	//判定到达目标的距离
	[Export] public float ReachDistance = 5f;
	
	[ExportGroup("淡入淡出时间")]
	[Export] public float FadeDuration = 0.3f;
	
	[ExportGroup("路径设置")]
	//所有场景的路线配置
	[Export] public Array<NPCSceneRoute> SceneRoutes = new();
	
	[ExportGroup("碰撞体和贴图设置")]
	[Export] public AnimatedSprite2D Sprite { get; private set; }
	[Export] public CollisionShape2D Collision { get; private set; }
	
	private enum NPCState
	{
		Finish,
		Moving,   
		Staying,   
		EnteringDoor
	}
	
	//npc所在场景资源路径
	private string _currentScene;
	
	//玩家所在场景资源路径
	private string _currentPlayerScene;
	
	//当前场景行走路径
	private Array<NPCPath> _currentPaths;
	
	//当前状态
	private NPCState _currentState;
	
	//当前目标点索引
	private int _currentPointIndex;
	
	//停留计时器
	private float _stayTimer;
	
	//玩家与角色是否在同一个场景里面
	private bool _isOneCurrentPaths;
	
	private AnimatedSprite2D _sprite;
	
	private float _timer;
	
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print("sprite",_sprite);
		LoadSceneRoute(GetTree().CurrentScene.SceneFilePath);
		GD.Print("是否重复执行");
		SwitchState(NPCState.Moving);
	}
	
	public override void _Process(double delta)
	{
		//没有路径点时退出
		if (_currentPaths.Count == 0 ) return;
		
		GetCurrentScenePath();
		if(_currentPlayerScene == _currentScene)
		{
			_isOneCurrentPaths = true;
		}
		else
		{
			_isOneCurrentPaths = false;
		}
		
		GD.Print(_isOneCurrentPaths);
		GD.Print(_currentScene);
		InOneSceneOrNot();
		
		switch (_currentState)
		{
			case NPCState.Moving:
				ProcessMove((float)delta);
				break;
			case NPCState.Staying:
				ProcessStay((float)delta);
				break;
			case NPCState.EnteringDoor:
				ProcessEnterDoor((float)delta);
				break;
			case NPCState.Finish:
				FinishPath();
				break;
		}
	}
	
	//加载当前场景路线
	private void LoadSceneRoute(string scenePath)
	{
		_currentScene = scenePath;
		GD.Print(_currentScene);
		foreach (var route in SceneRoutes)
		{
			if (route.SceneFilePath == scenePath)
			{
				_currentPaths = route.Paths;
				_currentPointIndex = 0;
				GD.Print("NPCController       加载场景路径",scenePath);
				return;
			}
		}
	}
	
	//移动逻辑
	private void ProcessMove(float delta)
	{
		var target = _currentPaths[_currentPointIndex].TargetPosition;
		Vector2 dir = (target - Position).Normalized();
		
		Velocity = dir * MoveSpeed;
		MoveAndSlide();
		
		if (Position.DistanceTo(target) < ReachDistance)
		{
			var door = GetDoorAt(target);
			GD.Print(door);
			if (door != null)
			{
				EnterDoor(door);
				GD.Print("进门");
			}
			else
			{
				SwitchState(NPCState.Staying);
			}
		}
	}
	
	//停留逻辑
	private void ProcessStay(float delta)
	{
		_timer -= delta;
		if (_timer <= 0)
		{
			SwitchState(NPCState.Moving);
			NextPoint();
		}
	}
	
	//进门消失 + 切换场景路径
	private void EnterDoor(MarkerDoor door)
	{
		SwitchState(NPCState.EnteringDoor);
		_timer = FadeDuration;
		
		LoadSceneRoute(door.TargetScenePath);
		Position = door.SpawnPosition;
	}
	
	private void ProcessEnterDoor(float delta)
	{
		_timer -= delta;
		_sprite.Modulate = new Color(1, 1, 1, _timer / FadeDuration);
		
		if (_timer <= 0)
		{
			_sprite.Modulate = Colors.White;
			SwitchState(NPCState.Moving);
		}
	}
	
	private MarkerDoor GetDoorAt(Vector2 pos)
	{
		foreach (var door in GetTree().GetNodesInGroup("MarkerDoor"))
		{
			if (door is MarkerDoor d && d.Position.DistanceTo(pos) < 10)
			return d;
		}
		return null;
	}
	
	private void NextPoint()
	{
		_currentPointIndex++;
		
		if(_currentPointIndex+1 > _currentPaths.Count)
		{
			SwitchState(NPCState.Finish);
		}
	}
	
	private void SwitchState(NPCState newState)
	{
		_currentState = newState;
		
		if (newState == NPCState.Staying)
		{
			_timer = _currentPaths[_currentPointIndex].StayTime;
		}
	}
	
	//获取当前玩家场景资源路径
	private void GetCurrentScenePath()
	{
		Node currentScene = GetTree().CurrentScene;
		if (currentScene == null)  return;
		string scenePath = currentScene.SceneFilePath;
		//GD.Print("scenePAth",scenePath);
		_currentPlayerScene = scenePath;
	}
	
	//玩家与角色是否在同一个场景
	private void InOneSceneOrNot()
	{
		//GD.Print("scenePAth",_currentScene);
		if(_isOneCurrentPaths == false)
		{
			if(Collision.Disabled == false)
			{
				NPCManager.Instance.BeforeChangeScene();
			}
			if(Sprite.Visible == true)
			{
				NPCManager.Instance.HideSprite();
			}
			
		}
		else
		{
			if(Collision.Disabled == true)
			{
				NPCManager.Instance.AfterChangeScene();
			}
			if(Sprite.Visible == false)
			{
				NPCManager.Instance.RestoreSprite();
			}
		}
	}
	
	//走完所有路径后
	private void FinishPath()
	{
		if(NPCManager.Instance.CanAlive == false)
		{
			DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue3, () =>
			{
				
			});
			
			QueueFree();
		}
		else
		{
			DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue4, () =>
			{
				
			});
		}
	}
	
}
