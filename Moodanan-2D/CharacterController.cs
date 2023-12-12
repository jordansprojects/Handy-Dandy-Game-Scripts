using Godot;
using System;

public class CharacterController : Area2D
{
	private bool isShooting = false;
	private AnimatedSprite _animatedSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		
		// connect signal
		Connect("body_entered", this, "_OnBodyEnteredx");
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta) {
	/* check for attempt to shoot */
	if(Input.IsActionPressed("mouse_left")){
		isShooting = true;
		_animatedSprite.Play("shoot");
	}
	/* if player is not already shooting */
	if(isShooting == false){
		if 	(Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left") ||
		Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down")){
			_animatedSprite.Play("walk");
		}else{
			_animatedSprite.Play("default");
		}
	}
  } /* end of _Process */



	private void _OnBodyEntered(object body)
	{
		GD.Print("idk, something entered.");
		
		
		
	}

private void onAnimationFinished(){
	// Replace with function body.
	isShooting = false;
	_animatedSprite.Play("default");
}

} /* end of class */




