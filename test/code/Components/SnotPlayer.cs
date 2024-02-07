using Sandbox.Citizen;

namespace Sandbox.Components;
 
public sealed class SnotPlayer : Component
{

	[Property]
	public GameObject Camera { get; set; }

	[Property]
	public CharacterController Controller { get; set; }

	[Property]
	public CitizenAnimationHelper Animator { get; set; }

	[Property]
	[Range(0, 800f, 1f)]
	public float RunSpeed { get; set; } = 20f;

	[Property]
	[Range(0, 1000f, 10f)]
	public float JumpStrength { get; set; } = 400f;

	[Property] [Range( 0, 400f, 1f )] public float WalkSpeed { get; set; } = 120f;

	protected override void OnUpdate()
	{

	}
}
