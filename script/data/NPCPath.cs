using Godot;
using System;

//NPC 路径点数据类：位置 + 停留时间
[GlobalClass]
public partial class NPCPath : Resource
{
	//目标位置
	[Export] public Vector2 TargetPosition { get; set; }
	
	//停留秒数
	[Export] public float StayTime { get; set; }
	
	public NPCPath() 
	{
		
	}
	
	public NPCPath(Vector2 targetPosition, float stayTime)
	{
		TargetPosition = targetPosition;
		StayTime = stayTime;
	}
}
