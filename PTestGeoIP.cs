// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

internal class PTestGeoIP : PTest
{
	public static void Lookup(Action done)
	{
		UnityEngine.Debug.Log("PTestGeoIP.Lookup");
		Playtomic.GeoIP.Lookup(delegate(PlayerCountry geo, PResponse r)
		{
			geo = (geo ?? new PlayerCountry());
			PTest.AssertTrue("PTestGeoIP.Lookup", "Request succeeded", r.success);
			PTest.AssertEquals("PTestGeoIP.Lookup", "No errorcode", r.errorcode, 0);
			PTest.AssertFalse("PTestGeoIP.Lookup", "Has country name", string.IsNullOrEmpty(geo.name));
			PTest.AssertFalse("PTestGeoIP.Lookup", "Has country code", string.IsNullOrEmpty(geo.code));
			done();
		});
	}
}
