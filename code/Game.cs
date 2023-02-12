using System.Collections.Generic;
using System.Diagnostics;
using Sandbox;

namespace DVD
{
	public class Game : Sandbox.GameManager
	{
		public static Game Instance { get; internal set; }

		private IList<IClient> _clients = new List<IClient>();

		public Game()
		{
			if ( Sandbox.Game.IsServer )
			{
				_ = new DVDEntity();
				_ = new HudEntity();
			}

			Instance = this;
		}

		public override void ClientJoined( IClient cl )
		{
			base.ClientJoined( cl );

			_clients.Add( cl );
		}

		public override void ClientDisconnect( IClient cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );

			_clients.Remove( cl );
		}

		public static void AddScore()
		{
			foreach ( var cl in Instance._clients )
			{
				if ( !cl.IsValid )
					continue;
				
				cl.AddInt( "corners" );
			}
			Log.Error( "TODO: leaderboards" );
		}
	}

}
