using Sandbox;

namespace DVD
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	public partial class Game : Sandbox.Game
	{
		public static Game Instance { get; internal set; }

		public Game()
		{
			if ( IsServer )
			{
				new DVDEntity();
				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new HudEntity();

				GameServices.StartGame();
			}

			Instance = this;
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
		}

		public override void Shutdown()
		{
			base.Shutdown();

			GameServices.EndGame();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
		}

		public static void AddScore()
		{
			foreach ( var cl in Client.All )
			{
				cl.AddInt( "corners" );
				GameServices.RecordScore( cl.PlayerId, cl.IsBot, GameplayResult.None, cl.GetInt( "corners" ) );
			}
		}
	}

}
