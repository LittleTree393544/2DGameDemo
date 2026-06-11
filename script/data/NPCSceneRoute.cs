using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class NPCSceneRoute : Resource
{
	//场景路径
	[Export] public string SceneFilePath; 
	
	[Export] public Array<NPCPath> Paths = new();
}
