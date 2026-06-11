using Godot;
using System;
using System.Collections.Generic;

public partial class DialogManager : Node
{
	public static DialogManager Instance;
	
	[Export] private RichTextLabel _dialogText;
	[Export] public float _dialogDelay = 2f;
	
	private List<string> _currentLines;
	private int _currentIndex;
	private System.Action _onFinish;
	public bool IsPlaying { get; private set; }
	
	public override void _Ready()
	{
		if(Instance != null) 
		{
			//删除重复的管理器
			QueueFree();
			return;
		}
		
		Instance = this;
		_dialogText.Text = "";
	}
	
	//开始对话，外部调用
	public void StartDialog(List<string> lines, System.Action onFinish = null)
	{
		if (IsPlaying || lines == null || lines.Count == 0)
		{
			return;
		}
		
		IsPlaying = true;
		_currentLines = lines;
		_currentIndex = 0;
		_onFinish = onFinish;
		
		ShowCurrentLine();
	}
	
	//显示当前句子 异步方法
	private async void ShowCurrentLine()
	{
		_dialogText.Text = _currentLines[_currentIndex];
		
		await ToSignal(GetTree().CreateTimer(_dialogDelay), "timeout");
		
		NextLine();
	}
	
	//下一句
	public void NextLine()
	{
		_currentIndex++;
		
		if (_currentIndex >= _currentLines.Count)
		{
			EndDialog();
			return;
		}
		
		ShowCurrentLine();
	}
	
	private void EndDialog()
	{
		IsPlaying = false;
		_dialogText.Text = "";
		
		//_onFinish存在就执行
		_onFinish?.Invoke();
	}
	
	//按空格下一句
	/*
	public override void _Input(InputEvent @event)
	{
	  if (IsPlaying && @event.IsActionPressed("ui_accept"))
	 {
	   NextLine();
	 }
	} 
	*/
}
