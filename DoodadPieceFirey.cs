// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DoodadPieceFirey : DoodadPiece
{
	public override void Death()
	{
		base.Death();
		Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), 24f, this.x, this.y, Map.groundLayer, false);
		Map.HitLivingUnits(null, 15, 3, DamageType.Fire, 12f, this.x, this.y, 0f, 50f, true, false);
	}
}
