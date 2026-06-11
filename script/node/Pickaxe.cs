using Godot;
using System;

public partial class Pickaxe : BasePickupItem
{
	protected override void OnPickupSuccess()
	{
		DialogManager.Instance.StartDialog(DialogueData.Scene1Dialogue1,() =>
		{
			
		});
	}
}
