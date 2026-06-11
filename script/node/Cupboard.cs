using Godot;
using System;

public partial class Cupboard : BaseInteractable
{
	[Export] public AnimatedSprite2D Sprite;
	
	private bool _isOpen;
	
	public override void _Ready()
	{
		base._Ready();
		
		_isOpen = false;
		
		Sprite.Play("close");
	}
	
	protected override void OnInteractSuccess()
	{
		base.OnInteractSuccess();
		
		if (!_isOpen)
		{
			Sprite.Play("open");
		}
		
		Sprite.AnimationFinished += OnOpenFinished;
		
		PlayerInventoryManager.Instance.AddItem("002");
		
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
	
	private void OnOpenFinished()
	{
		
	}
}
