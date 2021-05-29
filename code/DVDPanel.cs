using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DVD
{
	public class DVDPanel : Panel
	{
		internal class DVDTintPanel : Panel
		{
			int currentColor = 0;

			private Color[] Colors;
			private Dictionary<char, int> hex2int;

			public DVDTintPanel()
			{
				hex2int = new()
				{
					{ '0', 0 },
					{ '1', 1 },
					{ '2', 2 },
					{ '3', 3 },
					{ '4', 4 },
					{ '5', 5 },
					{ '6', 6 },
					{ '7', 7 },
					{ '8', 8 },
					{ '9', 9 },
					{ 'a', 10 },
					{ 'b', 11 },
					{ 'c', 12 },
					{ 'd', 13 },
					{ 'e', 14 },
					{ 'f', 15 }
				};
				Colors = new[] { FromHex( "#be00ff" ), FromHex( "#00feff" ), FromHex( "#ff8300" ), FromHex( "#0026ff" ), FromHex( "#fffa01" ), FromHex( "#ff2600" ), FromHex( "#ff008b" ), FromHex( "#25ff01" ) };
			}

			public void NextColor()
			{
				currentColor = (currentColor + 1) % Colors.Length;
				Style.BackgroundColor = Colors[currentColor];
				Style.Dirty();
			}

			private Color FromHex( string hex )
			{
				if ( hex.StartsWith( "#" ) )
					hex = hex[1..].ToLower();

				if ( hex.Length != 6 ) throw new Exception( "Color not valid" );

				return new Color(
					Hex2Int( hex.Substring( 0, 2 ) ),
					Hex2Int( hex.Substring( 2, 2 ) ),
					Hex2Int( hex.Substring( 4, 2 ) ) );
			}

			private int Hex2Int( string hex )
			{
				return hex2int[hex[0]] * 16 + hex2int[hex[1]];
			}
		}

		DVDTintPanel tp;
		Random random;
		float x = 1.0f;
		float y = 1.0f;
		float xAcc, yAcc = 0.0f;
		float xDir = 1.0f;
		float yDir = 1.0f;
		float speed;
		bool hyped = false;
		Vector2 cornerOfInterest = new Vector2( .0f, .0f );

		static SoundEvent AlmostSound = new( "sounds/close.vsnd" );
		static SoundEvent DamnSound = new( "sounds/damn.vsnd" );
		static SoundEvent CornerSound = new( "sounds/yooouuu.vsnd" );

		public DVDPanel()
		{
			random = new Random();

			RandomiseParams();

			AddChild( out tp );
		}

		public override void Tick()
		{
			x = MathX.Clamp( x + xAcc * xDir * speed * Time.Delta, 0, Screen.Width - Box.Rect.width );
			y = MathX.Clamp( y + yAcc * yDir * speed * Time.Delta, 0, Screen.Height - Box.Rect.height );

			if (!hyped)
			{
				if ( Vector3.DistanceBetween( new Vector3( cornerOfInterest, 0 ), new Vector3( x, y, 0 ) ) < 300.0f )
				{
					hyped = true;
					PlayIfNotNull( AlmostSound );
				}
			}

			var ts = TouchSides();
			var tcf = TouchCeilFloor();
			if ( ts || tcf )
			{
				tp.NextColor();
				if ( ts )
					xDir *= -1;
				if ( tcf )
					yDir *= -1;

				if ( ts && tcf )
					PlayIfNotNull( CornerSound );
				else if (hyped)
				{
					PlayIfNotNull( DamnSound );
				}
				hyped = false;

				RandomiseParams();
			}

			Style.Left = Length.Pixels( x );
			Style.Top = Length.Pixels( y );
			Style.Dirty();
		}

		private bool TouchSides()
		{
			return (x <= 0) || (x + Box.Rect.width >= Screen.Width);
		}
		private bool TouchCeilFloor()
		{
			return (y <= 0) || (y + Box.Rect.height >= Screen.Height);
		}

		private void SetCornerOfInterest()
		{
			cornerOfInterest = new Vector2( xDir < 0 ? .0f : Screen.Width - Box.Rect.width, yDir < 0 ? .0f : Screen.Height - Box.Rect.height );
		}

		private void PlayIfNotNull( SoundEvent s )
		{
			if ( Local.Pawn != null )
				Local.Pawn.PlaySound( s.Name );
		}

		private void RandomiseParams()
		{
			var angle = (double)random.Next( 40, 90 - 40 ) / 180 * Math.PI;

			xAcc = (float)Math.Cos( angle );
			yAcc = (float)Math.Sin( angle );
			speed = random.Next( 50, 100 );
			SetCornerOfInterest();
		}
	}
}
