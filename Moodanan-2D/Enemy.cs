using Godot;
using System;
using DebugTools;

public class Enemy : Area2D
{
	private CharacterController _target; 
	private AnimatedSprite _animatedSprite; // we assume all enemies have this
	private int _health = 100;
	
	private int STOPPING_DISTANCE = 5; // tweak this to what looks right
	private float GROWTH_AMOUNT = 0.005f;
	
	private float _t = 0.01f;
	
	private AimIndicator _userGun; // reference to the users weapon 
	
	public Enemy (int health = 100){
		_health = health;
	}
	
	public Enemy(){
		
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_target = (GetTree().Root.GetNode("Main").FindNode("Player")) as CharacterController;
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_userGun = (GetTree().Root.GetNode("Main").FindNode("Aim Indicator")) as AimIndicator;
		#if DEBUG
		Debug.Assert(_target != null, "Enemy.cs: Failed to find user target node.");
		Debug.Assert(_userGun != null, "Enemy.cs Failed to find AimIndicator node");
		#endif
		// connect signal
		Connect("area_entered", this, "_OnAreaEntered");
		Connect("body_entered", this, "_OnBodyEntered");
		_animatedSprite.Play("default");
		
		
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) {
	behave(delta);      
  }
	/* defines how the enemy type engages with the user 
		   default enemy it follows the player 
		*/
	public virtual void behave(float delta){
		var distance = Math.Sqrt( Math.Pow( (_target.GlobalPosition.x - Position.x), 2 ) + 
		Math.Pow( (_target.GlobalPosition.y - Position.y), 2 ) ); 
		if(distance > STOPPING_DISTANCE){
			// 
			//LookAt(_target.GlobalPosition);
			//var dir = (_target.GlobalPosition - Position).Normalized();	
			//Position+=dir;
			
			Position = Position.LinearInterpolate(_target.Position, _t);
			
			// if hes moving, we also want him to grow a lil bit to give the 
			// appearance that he is coming closer
			Scale+= new Vector2(GROWTH_AMOUNT, GROWTH_AMOUNT);
		}
		
	}
		
	
   
	

	
	private void takeDmg(int dmg){
		GD.Print("Enemy.cs: OUCH! I took " + dmg + " damage!");
		_health-=dmg;
		
		if(_health <=0){
			// destroy object
			QueueFree();
		}
		
	
	}
	
	private void _OnAreaEntered(Area2D obj)
	{
		
		if (obj is AimIndicator){
			GD.Print("Enemy.cs: Aim indicator is on me!");
		}
		
		
	}
	
	private void _OnBodyEntered(Node body){
		if (body is Bullet){
			Bullet b = body as Bullet;
			GD.Print("Bullet has hit me!");
			
			// inflict dmg
			takeDmg(_userGun.doDmg(b.getDmg(), b.getCritChance(), b.getCritSeconds(), b.getDmgMultiplier() ) );
		}
	}



} // end of Enemy class
