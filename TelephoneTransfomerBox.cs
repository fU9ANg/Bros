// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TelephoneTransfomerBox : MonoBehaviour
{
	private void Awake()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, 1f);
		foreach (Collider collider in array)
		{
			if (collider != base.GetComponent<Collider>() && collider.GetComponent<TelephoneTransfomerBox>() != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			}
		}
	}

	public virtual void StepOn(TestVanDammeAnim unit)
	{
		if (unit.yI < -250f)
		{
			base.GetComponent<DoodadDestroyable>().Damage(new DamageObject(1, DamageType.Normal, 0f, 0f, unit));
		}
	}
}
