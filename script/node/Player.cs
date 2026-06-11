using Godot;
using System;

public partial class Player : CharacterBody2D
{
	 // 移动速度，可在编辑器直接调整
	[Export] public float MoveSpeed = 350;
	
	public override void _Process(double delta)
	{
		//监听E键 交互
		if (Input.IsActionJustPressed(InteractConst.INTERACT_KEY))
		{
			PlayerInteractManager.Instance.TryInteract();
		}
		
		//监听空格 拾取
		if (Input.IsActionJustPressed(InteractConst.PICKUP_KEY))
		{
			PlayerInteractManager.Instance.TryPickup();
			GD.Print("拾取");
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// 获取 WASD / 方向键 输入
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		// 计算移动方向
		Vector2 velocity = inputDir * MoveSpeed;
		
		// 移动
		Velocity = velocity;
		MoveAndSlide();
	}
	
}
