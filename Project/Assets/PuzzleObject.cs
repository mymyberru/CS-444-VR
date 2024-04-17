using UnityEngine;
using System.Collections;
public class PuzzleObject : InteractiveObject {

	[Header( "Grasping Properties" )]
	public float graspingRadius = 0.1f;
	
	public enum DirectionOfMovement : int { x, y,z };
	[Header( "direction of movement Properties" )]
	public DirectionOfMovement directionOfMovement;
	
	protected Transform initial_transform_parent;
	protected PuzzleController controller = null;
	public bool is_available () { return controller == null; }
	public float get_grasping_radius () { return graspingRadius; }
	
	void Start () {
		myType = ObjectType.Puzzle;//what type of interactive object this is
		initial_transform_parent = transform.parent;
	}
	
	public void attach_to ( PuzzleController controller ) {
		this.controller = controller;
		Transform newTransform=new GameObject().transform;
		newTransform.position = controller.transform.position;
		newTransform.rotation=Quaternion.identity;
		transform.SetParent(newTransform);
		
		StartCoroutine(UpdatePosition(newTransform.transform));
	}
	public void detach_from ( PuzzleController controller ) {
		if ( this.controller != controller ) return;
		this.controller = null;
		transform.SetParent( initial_transform_parent );
	}

	
	private IEnumerator UpdatePosition(Transform newTransform) {
		while (true) {
			
			Vector3 targetPosition = newTransform.position;
			Vector3 originalPosition=newTransform.position;
			
			
			RaycastHit hit;
			
			if (directionOfMovement == DirectionOfMovement.x){targetPosition.x = controller.transform.position.x;}
			if (directionOfMovement == DirectionOfMovement.y){targetPosition.y = controller.transform.position.y;}
			if (directionOfMovement == DirectionOfMovement.z){targetPosition.z = controller.transform.position.z;}
			
			Vector3 direction = targetPosition - originalPosition;
			
			if (Physics.Raycast(originalPosition, direction.normalized, out hit, direction.magnitude)) {
            // If a collision is detected, adjust the target position to the point of collision
            targetPosition = hit.point;
			}
			
			newTransform.position = targetPosition;
			yield return null; // Wait for next frame
		}
	}



}
