using Godot;
using System;

public partial class Shovel : BasePickupItem
{
	protected override void OnPickupSuccess()
	{
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue1,() =>
		{
			
		});
	}
}
