using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
