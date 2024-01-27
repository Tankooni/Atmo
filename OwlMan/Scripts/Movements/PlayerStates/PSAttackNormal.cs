using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Atmo2.Movements.PlayerStates
{
	class PSAttackNormal : PlayerState
	{
		private float speedModifier;

		public PSAttackNormal(Player player, float initialSpeedModifier)
			: base(player)
		{
			this.player = player;
			speedModifier = initialSpeedModifier;
		}

		public override void OnEnter()
		{
			player.Animation = "attackNormal";

		}

		public override void OnExit(PlayerState newState)
		{
			player.MovementInfo.ResetBoxes();
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			if (signedHorizontal != 0)
				player.Image.FlipH = signedHorizontal < 0;

			if (player.Image.FlipH)
			{
				player.MovementInfo.LeftBox = true;
			}
			else
			{
				player.MovementInfo.RightBox = true;
			}

			if (!player.IsOnFloor())
				player.MovementInfo.Vel_New.Y += player.Gravity;
			
			// MOVEMENT --------------------------------------------------------------------------
			player.MovementInfo.Vel_New.X = player.RunSpeed * signedHorizontal + speedModifier;
			
			if (speedModifier != 0)
			{
				var modSign = Math.Sign(speedModifier);

				speedModifier = speedModifier - player.HorizontalGroundDrag * modSign;

				if(modSign != Math.Sign(speedModifier))
					speedModifier = 0;
			}
			// ---------------------------------------------------------------------------------

			return null;
		}


        public override PlayerState OnAnimationComplete()
		{
			if (player.MovementInfo.OnGround)
				if (player.InputController.LeftHeld() || player.InputController.RightHeld())
					return new PSRun(player, initialSpeedModifier: speedModifier);
				else
					return new PSIdle(player);
			else
				return new PSFall(player, initialSpeedModifier: speedModifier);
		}
	}
}
