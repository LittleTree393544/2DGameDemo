using Godot;
using System;

public partial class Board : BaseInteractable
{
	[Export] public Sprite2D Sprite;
	[Export] public CollisionShape2D Collision;
	
	protected override void OnInteractSuccess()
	{
		base.OnInteractSuccess();
		
		Sprite.Visible = false;
		Collision.Disabled = true;
		
		PlayerInventoryManager.Instance.AddItem("005");
		
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue1,() =>
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
