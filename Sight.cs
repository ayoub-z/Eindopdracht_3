using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionEventArgs
{
	public Collider collider;
}

public class Sight : MonoBehaviour
{
	
	[SerializeField] float distance = 30f;
	[SerializeField] Color gizmoColor = Color.blue;
	[SerializeField] LayerMask layerMask;
	internal HashSet<Collider> inSight;
	HashSet<Collider>  lastInSight;

	public delegate void VisionEventHandler(object sender, VisionEventArgs args);
	public event VisionEventHandler OnEnterVision, OnLeaveVision;

	void Awake()
	{
		inSight = new HashSet<Collider>(); 
	}

	Collider[] overlaps;
	RaycastHit hitInfo;
	
	void FixedUpdate()
	{
		lastInSight = new HashSet<Collider>(inSight);

		overlaps = Physics.OverlapSphere(transform.position, distance, layerMask);
		foreach (Collider collider in overlaps)
		{
			if(Physics.Raycast(transform.position, collider.transform.position - transform.position, out hitInfo, distance, layerMask)
				&& hitInfo.collider == collider)
			{
				if (!inSight.Contains(collider))
				{
					inSight.Add(collider);
					OnEnterVision.Invoke(this, new VisionEventArgs() { collider = collider });
				}
				else
				{
					lastInSight.Remove(collider);
				}
			}
		}

		foreach (Collider collider in lastInSight)
		{
			inSight.Remove(collider);
			OnLeaveVision.Invoke(this, new VisionEventArgs() { collider = collider });
		}
	}

	void OnDrawGizmos()
	/* function that draws lines either between agent and objects 
	   or lines to display it's perceivable surrounding */
	{
		if(inSight != null) 
		{
			Gizmos.color = Color.magenta;
			foreach (Collider collider in inSight)
			{
				Gizmos.DrawLine(transform.position, collider.transform.position);
			}
		}
		// make a circle of the Agent's perceivable surroundings
		Gizmos.color = gizmoColor;
		Gizmos.DrawWireSphere(transform.position, distance);
	}
}

