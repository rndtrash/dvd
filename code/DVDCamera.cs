using Sandbox;

namespace DVD
{
	class DVDCamera : FirstPersonCamera
	{
		public override void BuildInput( InputBuilder input )
		{
			input.Clear();
			input.StopProcessing = true;
		}
	}
}
