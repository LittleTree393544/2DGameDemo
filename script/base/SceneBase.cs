using Godot;
using System;
using System.Collections.Generic;
 
//负责执行场景功能，角色生成，与场景切换
public partial class SceneBase : Node
{
	private string _sceneAssetPath;
	
	public override async void _Ready()
	{
		_sceneAssetPath = GetTree().CurrentScene.SceneFilePath;
		//GD.Print(_sceneAssetPath);
		await ToSignal(GetTree(), "process_frame");
		SpawnPlayerInScene();
		
		
		await ToSignal(GetTree(), "physics_frame");
		await ToSignal(GetTree(), "process_frame");
		PlayerManager.Instance.AfterChangeScene();
		NPCManager.Instance.AfterChangeScene();
		//NPCManager.Instance.RestoreSprite();
	}
	
	public override void _EnterTree()
	{
		
		if (SceneManager.Instance != null)
		{
		 LoadSceneData();
		}
		
	}
	
	public override void _ExitTree()
	{
		if (SceneManager.Instance != null)
		{
		 SaveSceneData();
		}
		
		PlayerManager.Instance.BeforeChangeScene();
	}
	
	//加载数据
	protected virtual void LoadData(Dictionary<string, Variant> data)
	{
		
	}
	
	//角色生成
	protected virtual void SpawnPlayerInScene()
	{
		string spawnPoint = SceneManager.Instance.LoadTargetSpawnPoint();
		
		Marker2D spawn = null;
		
		if (!string.IsNullOrEmpty(spawnPoint))
		{
			spawn = GetNodeOrNull<Marker2D>(spawnPoint);
		}
		else
		{
			spawn = GetNodeOrNull<Marker2D>("PlayerSpawn");
		}
		
		PlayerManager.Instance.SpawnPlayer(this, spawn);
		
		spawn = null;
	}
	
	//要保存的数据
	protected virtual Dictionary<string, Variant> SaveData()
	{
		return null;
	} 
	
	private void SaveSceneData()
	{
		var data = SaveData();
		if(data != null)
		{
			SceneManager.Instance.SaveData(_sceneAssetPath,data);
		}
	}
	
	private void LoadSceneData()
	{
		var data = SceneManager.Instance.LoadData(_sceneAssetPath);
		if(data != null)
		{
			LoadData(data);
		}
	}
	
	protected virtual void SpawnNPCInScene()
	{
		
		Marker2D spawn = null;
		
		spawn = GetNodeOrNull<Marker2D>("NPCSpawn");
		
		NPCManager.Instance.SpawnNPC(this, spawn);
		
		spawn = null;
	}
}
