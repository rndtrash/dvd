using Sandbox;
using Sandbox.UI;
using System.Linq;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace DVD
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class DVDHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public DVDPanel dvdp;

		private DVDEntity dvde;

		[Net]
		private float xLocal { get; set; }
		[Net]
		private float yLocal { get; set; }
		[Net]
		private int colorLocal { get; set; }


		public DVDHudEntity()
		{
			if ( IsClient )
			{
				//RootPanel.SetTemplate( "/minimalhud.html" );
				RootPanel.StyleSheet.Load( "/DVDHud.scss" );
				var p = RootPanel.Add.Panel( "dvdcont" );
				p.AddChild( out dvdp );

				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			}
			else
			{
				dvde = All.OfType<DVDEntity>().First();
				if ( dvde == null )
					throw new System.Exception( "fuck" );
			}
		}

		[Event.Tick]
		public void Tick()
		{
			if ( IsClient )
			{
				dvdp.x = xLocal;
				dvdp.y = yLocal;
				dvdp.currentColor = colorLocal;
			}
			else
			{
				xLocal = dvde.xConverted;
				yLocal = dvde.yConverted;
				colorLocal = dvde.currentColor;
			}
		}
	}

}
