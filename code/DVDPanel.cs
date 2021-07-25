using Sandbox.UI;
using System;

namespace DVD
{
	public partial class DVDPanel : Panel
	{
		internal class DVDTintPanel : Panel
		{
			private int currentNum = 0;

			public DVDTintPanel()
			{
			}

			public void SetColor( int num )
			{
				if ( num == currentNum )
					return;
				currentNum = num;
				Style.BackgroundColor = FromHex( DVDEntity.HexColors[num] );
				Style.Dirty();
			}

			private Color FromHex( string hex )
			{
				if ( hex.StartsWith( "#" ) )
					hex = hex[1..].ToLower();

				if ( hex.Length != 6 ) throw new Exception( "Color not valid" );

				return Color.FromBytes(
					Convert.ToInt32( hex.Substring( 0, 2 ), 16 ),
					Convert.ToInt32( hex.Substring( 2, 2 ), 16 ),
					Convert.ToInt32( hex.Substring( 4, 2 ), 16 ) );
			}
		}

		public int currentColor = 0;
		public float x = .0f;
		public float y = .0f;

		DVDTintPanel tp;

		public DVDPanel()
		{
			AddChild( out tp );
		}

		public override void Tick()
		{
			Style.Left = Length.Percent( x * 100 );
			Style.Top = Length.Percent( y * 100 );
			Style.Dirty();
			tp.SetColor( currentColor );
		}
	}
}
