using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace DVD
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class HudEntity : Sandbox.HudEntity<RootPanel>
	{
		public class DVDScoreboard<T> : Scoreboard<T> where T : ScoreboardEntry, new()
		{
			protected override void AddHeader()
			{
				Header = Add.Panel( "header" );
				Header.Add.Label( "Name", "name" );
				Header.Add.Label( "Corner Hits Seen", "corners" );
				Header.Add.Label( "Ping", "ping" );
			}
		}

		public class DVDScoreboardEntry : ScoreboardEntry
		{
			Label CornerHitsSeen;

			public DVDScoreboardEntry()
			{
				AddClass( "entry" );

				CornerHitsSeen = Add.Label( "0", "corners" );
				Kills.Delete( true );
				Deaths.Delete( true );
				Ping.Delete( true );
				Ping = Add.Label( "", "ping" );
			}

			public override void UpdateData()
			{
				PlayerName.Text = Client.Name;
				CornerHitsSeen.Text = Client.GetInt( "corners" ).ToString();
				Ping.Text = Client.Ping.ToString();
				SetClass( "me", Client == Sandbox.Game.LocalClient );
			}
		}

		public HudEntity()
		{
			if ( !Sandbox.Game.IsClient )
			{
				return;
			}

			//RootPanel.SetTemplate( "/minimalhud.html" );
			RootPanel.StyleSheet.Load( "/Hud.scss" );

			RootPanel.AddChild<DVDPanel>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<DVDScoreboard<DVDScoreboardEntry>>();
		}
	}

}
