using Godot;
using System;

public partial class Door : BaseInteractable
{
	[Export] public AnimatedSprite2D Sprite;
	[Export] public CollisionShape2D Collision;
	
	private bool _isOpen;
	
	public override void _Ready()
	{
		base._Ready();
		
		_isOpen = false;
		
		Sprite.Play("door_Close");
		
		Collision.SetDeferred("disabled", false);
	}
	
	protected override void OnInteractSuccess()
	{
		base.OnInteractSuccess();
		
		if (!_isOpen)
		{
			_isOpen = true;
			
			Sprite.Play("door_Open");
			
			Sprite.AnimationFinished += OnOpenFinished;
		}
		else
		{
			_isOpen = false;
			
			Sprite.PlayBackwards("door_Open");
			
			Sprite.AnimationFinished += OnCloseFinished;
		}
	}
	
	private void OnOpenFinished()
	{
		Sprite.AnimationFinished -= OnOpenFinished;
		
		Sprite.Pause();
		
		Collision.SetDeferred("disabled", true);
	}
	
	private void OnCloseFinished()
	{
		Sprite.AnimationFinished -= OnCloseFinished;
		
		Sprite.Pause();
		
		Collision.SetDeferred("disabled", false);
	}
}
