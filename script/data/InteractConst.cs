using Godot;
using System;

public static class InteractConst 
{
	//玩家交互检测范围
	public const float INTERACT_RADIUS = 2.5f;
	
	//交互按键E
	public const string INTERACT_KEY = "interact";
	
	//交互按键空格
	public const string PICKUP_KEY = "pickup";
	
	//交互状态
	public const bool CAN_INTERACT = true;
	public const bool LOCKED_INTERACT = false;
}
