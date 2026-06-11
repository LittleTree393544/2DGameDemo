using Godot;
using System;

public partial class TimeMachine : BaseInteractable
{
	protected override void OnInteractSuccess()
	{
		base.OnInteractSuccess();
		
		SceneManager.Instance.LoadToscene("res://gameScene/scene/room1.tscn");
		
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue2,() =>
		{
			
		});
	}
	
	protected override void OnInteractFailed()
	{
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue0,() =>
		{
			
		});
	}
}
