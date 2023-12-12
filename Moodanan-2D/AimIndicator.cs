using Godot;
using System;
/*
 AimIndicator is responsible for handling mouse input,
 spawning bullets and playing animation to indicate to the user where their
 gun is aiming
*/ 
public class AimIndicator : Area2D
{
	private Timer _critTimer; 
	private AnimatedSprite _animatedSprite;
	private int _attempts = 0; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		
		/* retrieve sprite and play default spin animation */
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_animatedSprite.Play("default");
		
		_critTimer = GetNode<Timer>("Critical Hit Timer");
		/* Set timer so that by default it will stop when it reaches 0 */
		_critTimer.OneShot = true;
		
		/* seed RNG with random seed */
		 GD.Randomize(); 
		
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta){
	
	/* use GlobalPosition so that placement does not rely on parent node */
	GlobalPosition =    GetViewport().GetMousePosition();
	
	// check for mouse clicks
	if(Input.IsActionPressed("mouse_left")){
		var scene = GD.Load<PackedScene>("res://prefabs/Bullet.tscn"); 
		StaticBody2D bullet = scene.Instance() as StaticBody2D;
		GetTree().Root.AddChild(bullet);
		bullet.GlobalPosition = Position;
	}
	
  }

	
	/* 
	doDmg - takes in parameters from bullet class and then generates dmg amount 
	
	@param int dmg:  damage amount passed 
	@param float critChance: critical chance percentage passed 
	@param float critSeconds: time the user has to shoot again and get crit points from it
	@param int dmgMultiplier: how much the damage increases on critical hit
	@return int adjustedDmg: how much damage that was done
	
	reference for damage generation algorithm 
	https://dota2.fandom.com/wiki/Random_Distribution#Example
	*/	
	public int doDmg(int dmg, float critChance, float critSeconds, int dmgMultiplier){ 
		int adjustedDmg = dmg;
	
		if(_critTimer.IsStopped()){
			_attempts = 1;
		}
		
		float fate = GD.Randf();
		float adjustedCrit = 0;
		if(fate >= critChance){
			adjustedCrit = critChance * _attempts;
 		}
		fate = GD.Randf();
		
		if(fate < adjustedCrit){
			adjustedDmg*=dmgMultiplier; // critical hit! 
		}
		
		_attempts++;
		_critTimer.Start(critSeconds); // timer resets everytime user hits 
		
		return adjustedDmg;
		
	}
	
	

} // end of AimIndicator class
