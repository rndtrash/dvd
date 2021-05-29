using Sandbox.UI;

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

		public DVDHudEntity()
		{
			if ( IsClient )
			{
				//RootPanel.SetTemplate( "/minimalhud.html" );
				RootPanel.StyleSheet.Load( "/DVDHud.scss" );

				RootPanel.AddChild<DVDPanel>(out dvdp);
				RootPanel.AddChild<ChatBox>();
			}
		}
	}

}
