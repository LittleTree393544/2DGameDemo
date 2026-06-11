using Godot;
using System;

public partial class Room1 : SceneBase
{
	public override async void _Ready()
	{
		base._Ready();
		SpawnNPCInScene();
	}
}
