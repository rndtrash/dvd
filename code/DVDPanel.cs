using Sandbox.UI;
using System;

namespace DVD
{
	public partial class DVDPanel : Panel
	{
		public override void Tick()
		{
			Style.Left = Length.Percent( DVDEntity.Instance.xConverted * 100 );
			Style.Top = Length.Percent( DVDEntity.Instance.yConverted * 100 );

			Style.BackgroundTint = FromHex( DVDEntity.HexColors[DVDEntity.Instance.currentColor] );
		}

		public static Color FromHex( string hex )
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
}
