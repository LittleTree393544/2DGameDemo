using Godot;
using System;

public partial class TriggerArea : Area2D
{
	 public enum TriggerType
	{
		ChangeScene,    // 切换场景
		PlaySound,      // 播放音效
		ShowDialogue,    // 显示文字
		PlayCutsceneAnim    // 播放剧情动画
	}
	[Export] public TriggerType Type = TriggerType.ChangeScene;
	
	// 切换场景
	[Export] public string ScenePath = "";
	
	//目标场景角色生成Mark2D位置
	[Export] public string TargetSpawnPoint = "";

	// 播放音效用
	[Export] public AudioStream SoundEffect;
	
	// 显示文字用
	[Export] public Godot.Collections.Array<string> DialogueLines = new();
	
	// 剧情动画用
	[Export] public string CutsceneAnimName = "";
	
	// 触发一次就消失
	[Export] public bool OneShot = false;
	
	// 内部标记：是否已经触发过
	private bool _triggered;
	
	
	 private void OnBodyEntered(Node2D body)
	{
		//判断是玩家进入
		if(!body.IsInGroup("Player")) return;
		
		if(_triggered) return;
		
		//执行
		ExecuteEffect(body);
		
		//只触发一次
		if(OneShot==true)
		{
			_triggered = true;
			SetDeferred("monitoring", false); // 关闭触发检测
		}
		
		GD.Print(" 触发成功！进来的物体是",body);
	}
	
	private void ExecuteEffect(Node body)
	{
		switch(Type)
		{
			case TriggerType.ChangeScene:
			if(!string.IsNullOrEmpty(ScenePath))
			{
				GD.Print(" 切换场景");
				SceneManager.Instance.GetTargetSpawnPoint(TargetSpawnPoint);
				SceneManager.Instance.LoadToscene(ScenePath);
			}
			break;
			
			case TriggerType.PlayCutsceneAnim:
			if(!string.IsNullOrEmpty(CutsceneAnimName))
			{
				GameFlowManager.Instance.PlayCutsceneAnimation(CutsceneAnimName);
			}
			break;
			
			case TriggerType.ShowDialogue:
			if (DialogueLines.Count > 0)
			{
				DialogManager.Instance.StartDialog(new System.Collections.Generic.List<string>(DialogueLines), () =>
				{
					
				});
			}
			else
			{
				GD.Print("TriggerArea           文本为空");
			}
			break;
		}
	}
	  
}
