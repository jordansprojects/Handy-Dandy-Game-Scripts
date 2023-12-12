using Godot;
using System;

public class SceneLoad : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void ChangeScene(String sceneStr)
	{
		var scenePath = "res://levels/" + sceneStr;
		GD.Print("Loading path" + scenePath);
		GetTree().ChangeScene(scenePath);
		

	}
}

