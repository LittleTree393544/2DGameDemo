using Godot;
using System;

[GlobalClass]
public partial class MarkerDoor : Marker2D
{
	//要切换到的场景
	[Export] public string TargetScenePath;
	
	//切换后的出生点
	[Export] public Vector2 SpawnPosition;
}
