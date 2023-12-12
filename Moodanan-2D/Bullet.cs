using Godot;
using System;

public class Bullet : StaticBody2D{
	// damage dealing variables
	private float _critSeconds = 0.5f; /* for how long the user can wait before their hit attempts reset */ 
	private int _dmg = 3; /* base damage the user can deal */ 
	private float _critChance = 0.25f;  /* base critical strike chance */
	private int _dmgMultiplier = 2;  /* how much dmg is multiplied on a critical hit */
	private float bulletLife = 0.5f; 
	private Timer _ttlTimer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		_ttlTimer = GetNode<Timer>("Time To Live");
		_ttlTimer.Start(bulletLife);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) {
	 if (_ttlTimer.IsStopped()){
		QueueFree();
	}
  }
	
	
	// Public getters are OK. Bullet properties are read only.
	public int getDmg(){
		return _dmg;
	}
	
	public float getCritChance(){
		return _critChance;
	}
	
	public int getDmgMultiplier(){
		return _dmgMultiplier;
	}
	
	public float getCritSeconds(){
		return _critSeconds;
	}
} /* end of Bullet class */
