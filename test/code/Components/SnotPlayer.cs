using System;
using Sandbox.Citizen;
using Sandbox.Diagnostics;

namespace Sandbox.Components;
 
public sealed class SnotPlayer : Component
{

	[Property]
	[Category("Components")]
	public GameObject Camera { get; set; }

	[Property]
	[Category( "Components" )]

	public CharacterController Controller { get; set; }

	[Property]
	[Category( "Components" )]

	public CitizenAnimationHelper Animator { get; set; }

	[Property]
	[Category( "Stats" )]
	[Range(0, 800f, 1f)]
	public float RunSpeed { get; set; } = 20f;

	[Property]
	[Category( "Stats" )]
	[Range(0, 1000f, 10f)]
	public float JumpStrength { get; set; } = 400f;

	[Property]
	[Category( "Stats" )]
	[Range( 0, 400f, 1f )] 
	public float WalkSpeed { get; set; } = 120f;

	[Property]
	public Vector3 EyePosition { get; set; }

	public Angles EyeAngles { get; set; }
	private Transform _initalCameraTransform;

	protected override void DrawGizmos()
	{
		Gizmo.Draw.LineSphere(EyePosition, 10f);
	}

	protected override void OnUpdate()
	{
		EyeAngles += Input.AnalogLook;
		EyeAngles = EyeAngles.WithPitch( Math.Clamp( EyeAngles.pitch, -80f, 80f ) );
		Transform.Rotation = Rotation.FromYaw( EyeAngles.yaw );

		if ( Camera != null )
		{
			Camera.Transform.Local = _initalCameraTransform.RotateAround( EyePosition, EyeAngles.WithYaw( 0 ) );
		}
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( Controller == null ) return;

		var wishSpeed = Input.Down( "Run" ) ? RunSpeed : WalkSpeed;
		var wishVelocity = Input.AnalogMove.Normal * wishSpeed * Transform.Rotation;

		Controller.Accelerate( wishVelocity );

		if ( Controller.IsOnGround )
		{
			Controller.Acceleration = 10f;
			Controller.ApplyFriction( 5f );

			if ( Input.Down( "Jump" ) )
			{
				Controller.Acceleration = 2.5f;
				Controller.Punch( Vector3.Up * JumpStrength );
				
				if (Animator != null)
					Animator.TriggerJump();
			}
		}
		else
			Controller.Velocity += Scene.PhysicsWorld.Gravity * Time.Delta;

		Controller.Move();

		if ( Animator != null )
		{
			Animator.IsGrounded = Controller.IsOnGround;
			Animator.WithVelocity( Controller.Velocity );
		}
	}

	protected override void OnStart()
	{
		base.OnStart();

		if ( Camera != null )
			_initalCameraTransform = Camera.Transform.Local;

		if ( Components.TryGet<SkinnedModelRenderer>( out var model ) )
		{
			var clothing = ClothingContainer.CreateFromLocalUser();
			clothing.Apply( model  );
		}
	}
}
