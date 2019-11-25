using Godot;
using System;
using Atmo.OgmoLoader;

public class Overlord : Node
{
	public static float STANDARD_GRAVITY = 18f;

	public static Vector2 LevelBoundsX;
	public static Vector2 LevelBoundsY;
	public static Vector2 ViewportSize;

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		LoadLevel();
	}

	public void LoadLevel()
	{
		var ogmo = new OgmoLoader();
		Node2D player;
		var level = ogmo.Load(out player, out Overlord.LevelBoundsX, out Overlord.LevelBoundsY);

		Viewport root = GetTree().GetRoot();
		Overlord.ViewportSize = root.Size;

		var CurrentScene = root.GetChild(root.GetChildCount() - 1);

		if (player != null)
			CurrentScene.CallDeferred("add_child", player);
		CurrentScene.CallDeferred("add_child", level);
	}

	public void Reset()
	{
		//Remove the current level
		var level = GetNode("../GameScene");
		level.GetParent().RemoveChild(level);
		level.CallDeferred("free");

		//Add the next level
		var nextLevel = ((PackedScene)ResourceLoader.Load("res://scenes/GameScene.tscn")).Instance();
		GetNode("/root").AddChild(nextLevel);

		LoadLevel();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
