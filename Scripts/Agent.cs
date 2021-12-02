using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{	
	[SerializeField] Sight sight;
	int enemies = 0;

	void Awake()
	{
		sight.OnEnterVision += EnteredVision;
		sight.OnLeaveVision += LeftVision;
	}

	void EnteredVision(object sender, VisionEventArgs args)
	{
		Debug.Log($"{gameObject.name} sighted: {args.collider.gameObject.name}");
		if (args.collider.gameObject.tag == "Enemy")
		{
			enemies ++;
		}

	}

	void LeftVision(object sender, VisionEventArgs args)
	{
		Debug.Log($"{gameObject.name} no longer seeing: {args.collider.gameObject.name}");
		if (args.collider.gameObject.tag == "Enemy")
		{
			enemies --;
		}
	}


	void FixedUpdate()
	/* act state
	   as long as there are enemies within it's visible surroundings 
	   it will keep moving to the right */
	{
		if (enemies > 0)
		{ 
			transform.Translate(new Vector3(0.2f, 0f, 0f));
		}
	}

}
