using Godot;
using System;

public class InteractItemData 
{
	//是否要指定道具互动
	public bool NeedSpecificItem { get; set; }
	
	//所需的道具ID
	public string NeedItemId { get; set; }
	
	//是否只能交互一次
	public bool IsOnceInteract { get; set; }
	
	//当前是否已经交互完成
	public bool IsInteracted { get; set; }
	
	
}
