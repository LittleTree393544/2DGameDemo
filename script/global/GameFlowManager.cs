using Godot;
using System;
using System.Collections.Generic;

public partial class GameFlowManager : Node
{
	//单例模式全局调用
	public static GameFlowManager Instance { get; private set; }
	
	public enum GameState
	{
		Initializing,  //初始化游戏
		OpeningAnim,   //播放开场动画中
		Playing,       //游戏中
		CutsceneAnim,  //剧情动画中
		Paused,        //游戏暂停
	}
	
	public GameState CurrentState { get; private set; } = GameState.Initializing;
	
	//动画播放器节点引用
	[Export] public AnimationPlayer OpeningAnimPlayer;
	[Export] public AnimationPlayer CutsceneAnimPlayer;
	
	//全局事件
	[Signal] public delegate void OpeningAnimationFinishedEventHandler();
	[Signal] public delegate void CutsceneAnimationFinishedEventHandler();
	
	public override void _Ready()
	{
		if(Instance != null && Instance != this)
		{
			QueueFree();
			return;
		}
		
		Instance = this;
		
		//初始化完成，自动播放开场动画
		PlayOpeningAnimation();
	}
	
	//触发开场动画
	public void PlayOpeningAnimation()
	{
		//合法性检查
		 if (CurrentState != GameState.Initializing) return;
		
		//切换状态+暂停游戏+屏蔽玩家输入
		CurrentState = GameState.OpeningAnim;
		GetTree().Paused = true;
		GD.Print("开始播放开场动画...");
		
		 // 播放动画，绑定结束回调
		OpeningAnimPlayer.Play();
		OpeningAnimPlayer.AnimationFinished += OnOpeningAnimationFinished;
	}
	
	//开场动画结束回调
	private void OnOpeningAnimationFinished(StringName animName)
	{
		//结束绑定
		OpeningAnimPlayer.AnimationFinished -= OnOpeningAnimationFinished;
		
		//恢复游戏
		GetTree().Paused = false;
		CurrentState = GameState.Playing;
		
		//发送全局信号
		EmitSignal(SignalName.OpeningAnimationFinished);
		GD.Print("开场动画结束，进入游戏游玩阶段！");
	}
	
	//触发剧情动画
	public void PlayCutsceneAnimation(string cutsceneName)
	{
		// 只有在游玩中才能触发剧情
		if (CurrentState != GameState.Playing) return; 
		
		// 切换状态 + 暂停游戏 + 屏蔽玩家输入
		CurrentState = GameState.CutsceneAnim;
		GetTree().Paused = true;
		GD.Print("开始播放剧情动画：{cutsceneName}");
		
		// 播放对应剧情动画，绑定结束回调
		CutsceneAnimPlayer.Play(cutsceneName);
		OpeningAnimPlayer.AnimationFinished += OnCutsceneAnimationFinished;
	}
	
	// 剧情动画结束回调
	private void OnCutsceneAnimationFinished(StringName animName)
	{
		CutsceneAnimPlayer.AnimationFinished -= OnCutsceneAnimationFinished;
		GetTree().Paused = false;
		CurrentState = GameState.Playing;
		
		EmitSignal(SignalName.CutsceneAnimationFinished);
		GD.Print("剧情动画结束，回到游戏！");
	}
	
	//游戏暂停
	public void PausedGame()
	{
		if(CurrentState == GameState.Playing)
		{
			CurrentState = GameState.Paused;
			GetTree().Paused = true;
		}
	}
	
	//取消游戏暂停
	public void ResumeGame()
	{
		if (CurrentState == GameState.Paused)
		{
			CurrentState = GameState.Playing;
			GetTree().Paused = false;
		}
	}
}
