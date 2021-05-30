using Sandbox;
using System;
using System.Linq;

namespace DVD
{
	public partial class DVDEntity : Entity
	{
		public static readonly string[] HexColors = new[] { "#be00ff", "#00feff", "#ff8300", "#0026ff", "#fffa01", "#ff2600", "#ff008b", "#25ff01" };

		public float xConverted, yConverted;
		public int currentColor;

		Random random;
		// X & Y in [0; 1]
		//[Net]
		private float x;
		//[Net]
		private float y;

		float xAcc, yAcc = 0.0f;

		//[Net]
		float xDir;
		//[Net]
		float yDir;
		float speed;
		bool hyped = false;
		Vector2 cornerOfInterest = new( .0f, .0f );

		static SoundEvent AlmostSound = new( "sounds/close.vsnd" );
		static SoundEvent DamnSound = new( "sounds/damn.vsnd" );
		static SoundEvent CornerSound = new( "sounds/yooouuu.vsnd" );

		public DVDEntity()
		{
			if ( IsServer )
			{
				random = new Random();
				xDir = yDir = 1.0f;
				RandomiseParams();
			}
		}

		[Event.Tick]
		public void Tick()
		{
			if ( !IsServer )
				return;

			x = MathX.Clamp( x + xAcc * xDir * speed * Time.Delta, 0, 1920 );
			y = MathX.Clamp( y + yAcc * yDir * speed * Time.Delta, 0, 1080 );

			xConverted = x / 1920.0f;
			yConverted = y / 1080.0f;

			if ( !hyped )
			{
				if ( Vector3.DistanceBetween( new Vector3( cornerOfInterest, 0 ), new Vector3( x, y, 0 ) ) < 300.0f )
				{
					hyped = true;
					PlaySound( AlmostSound );
				}
			}

			var ts = TouchSides();
			var tcf = TouchCeilFloor();
			if ( ts || tcf )
			{
				NextColor();
				if ( ts )
					xDir *= -1;
				if ( tcf )
					yDir *= -1;
				if ( ts && tcf )
					PlaySound( CornerSound );
				else if ( hyped )
				{
					PlaySound( DamnSound );
				}
				hyped = false;

				RandomiseParams();
			}
		}

		private void PlaySound( SoundEvent sound )
		{
			foreach (DVDPlayer dvdp in Entity.All.OfType<DVDPlayer>())
			{
				if ( dvdp != null )
					dvdp.PlaySound( sound.Name );
			}
		}

		private void NextColor()
		{
			currentColor = (currentColor + 1) % HexColors.Length;
		}

		private void RandomiseParams()
		{
			var angle = (double)random.Next( 40, 90 - 40 ) / 180 * Math.PI;

			xAcc = (float)Math.Cos( angle );
			yAcc = (float)Math.Sin( angle );
			speed = random.Next( 50, 100 );
			SetCornerOfInterest();
		}
		private void SetCornerOfInterest()
		{
			cornerOfInterest = new Vector2( xDir < 0 ? .0f : 1920f, yDir < 0 ? .0f : 1080f );
		}

		private bool TouchSides()
		{
			return (x <= 0) || (x >= 1920f);
		}

		private bool TouchCeilFloor()
		{
			return (y <= 0) || (y >= 1080f);
		}
	}
}
