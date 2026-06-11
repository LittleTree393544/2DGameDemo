using Godot;
using System;

public partial class Window : BaseInteractable
{
	[Export] public Sprite2D Sprite;
	
	protected override void OnPlayerEnterRange(Node2D body)
	{
		base.OnPlayerEnterRange(body);
		
		if(!body.IsInGroup("Brother")) return;
		
		Sprite.Visible = false;
	}
	
	protected override void OnInteractSuccess()
	{
		SetDeferred("monitoring", false);
		
		NPCManager.Instance.CanAlive = true;
		
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue5, () =>
			{
				
			});
	}
}
