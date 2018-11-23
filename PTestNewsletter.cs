// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

internal class PTestNewsletter : PTest
{
	public static void Subscribe(Action done)
	{
		UnityEngine.Debug.Log("PTestNewsletter.Subscribe");
		PNewsletterOptions options = new PNewsletterOptions
		{
			email = "invalid @email.com"
		};
		Playtomic.Newsletter.Subscribe(options, delegate(PResponse r)
		{
			PTest.AssertFalse("PTestNewsletter.Subscribe#1", "Request failed", r.success);
			PTest.AssertEquals("PTestNewsletter.Subscribe#1", "Mailchimp API error", r.errorcode, 602);
			options["email"] = "valid@testuri.com";
			options["fields"] = new Dictionary<string, object>
			{
				{
					"STRINGVALUE",
					"this is a string"
				}
			};
			Playtomic.Newsletter.Subscribe(options, delegate(PResponse r2)
			{
				PTest.AssertTrue("PTestNewsletter.Subscribe", "Request succeeded", r2.success);
				PTest.AssertEquals("PTestNewsletter.Subscribe", "No errorcode", r2.errorcode, 0);
				done();
			});
		});
	}
}
