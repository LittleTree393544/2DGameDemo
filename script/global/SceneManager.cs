using Godot;
using System;
using System.Collections.Generic;

//管理场景并储存场景数据
public partial class SceneManager : Node
{
	 public static SceneManager Instance;
	
	 // 存储所有场景的数据
	 private Dictionary<string, Dictionary<string, Variant>> _sceneData = new();
	
	//目标角色生成位置
	 private string targetSpawnPoint;
	
	 public override void _Ready()
	{
		// 游戏启动时，把自己赋值给静态Instance
		Instance = this;
		
		//GameFlowManager.Instance.OpeningAnimationFinished += OnOpeningAnimEnd;
	}
	
	//播放开场动画后进入游戏
	private void OnOpeningAnimEnd()
	{
		LoadToscene("res://gameScene/scene_1.tscn");
	}
	
	private void DeferredLoadToscene(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}
	
	//加载场景
	public void LoadToscene(string scenepath)
	{
		CallDeferred(nameof(DeferredLoadToscene), scenepath);
		
		PlayerManager.Instance.KeepPlayerOnSceneChange();
	}
	
	//保留场景数据
	public void SaveData(string scenePath,Dictionary<string, Variant> data)
	{
		_sceneData[scenePath] = data;
		GD.Print("save  ",scenePath);
	}
	
	//加载场景数据
	public Dictionary<string, Variant> LoadData(string scenePath)
	{
		if (string.IsNullOrEmpty(scenePath))
		{
			GD.Print("场景数据为空");
			return null;
		}
		
		if (_sceneData.ContainsKey(scenePath))
		{
			GD.Print("load  ",scenePath);
			return _sceneData[scenePath];
		}
		else
		{
			//GD.Print("load  ",scenePath);
			return null;
		} 
	}
	
	//场景中玩家生成位置
	public void GetTargetSpawnPoint(string spawnPoint)
	{
		targetSpawnPoint = spawnPoint;
	}
	
	public string LoadTargetSpawnPoint()
	{
		GD.Print("SceneManager    ",targetSpawnPoint);
		string targetPoint = targetSpawnPoint;
		targetSpawnPoint = null;
		return targetPoint;
	}
}
