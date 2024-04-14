using UnityEngine;

public class WandObject : MonoBehaviour {

	[Header( "Grasping Properties" )]
	public float graspingRadius = 0.1f;
	

	
	// Store initial transform parent
	protected Transform initial_transform_parent;
	public GameObject rayPrefab; // Prefab for the visual representation of the ray
    private GameObject rayObject; // Instance of the ray prefab
	private GameObject hitObject=null;
	public AudioClip soundClip;
	
	// Store the hand controller this object will be attached to
	protected WandController hand_controller = null;

	void Start () {
		rayObject = Instantiate(rayPrefab);
		initial_transform_parent = transform.parent;
	}
	
	public void attach_to ( WandController hand_controller ) {
		// Store the hand controller in memory
		this.hand_controller = hand_controller;


		transform.position=hand_controller.transform.position;
		transform.rotation=Quaternion.LookRotation(hand_controller.transform.forward,hand_controller.transform.up);
		transform.rotation *= Quaternion.Euler(90f, 0f, 0f);
		transform.SetParent( hand_controller.transform );
	}

	public void detach_from ( WandController hand_controller ) {
		// Make sure that the right hand controller ask for the release
		if ( this.hand_controller != hand_controller ) return;

		// Detach the hand controller
		this.hand_controller = null;

		// Set the object to be placed in the original transform parent
		transform.SetParent( initial_transform_parent );
	}

	public bool is_available () { return hand_controller == null; }

	public float get_grasping_radius () { return graspingRadius; }
	
	
	
	
	
	public void DisplayRay() {
		
		Vector3 rayOrigin = transform.position;
		Vector3 rayDirection = transform.forward;
		float distance_to_target=100;

		RaycastHit hit;
		if (Physics.Raycast(rayOrigin, rayDirection, out hit))
		{
			distance_to_target=Vector3.Distance(hit.point, rayOrigin);
			hitObject = hit.collider.gameObject;
			TorchObject torchObject = hitObject.GetComponent<TorchObject>();
			if(torchObject != null){
				torchObject.selected();
			}

		}
		else
		{
			if(hitObject!=null){
				TorchObject torchObject = hitObject.GetComponent<TorchObject>();
				if(torchObject != null){
					torchObject.unselected();
				}
			}			
			hitObject=null;
			
		}
		
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        // Set the positions for the line renderer
        Vector3[] positions = { rayOrigin, rayOrigin + rayDirection.normalized * distance_to_target };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }

	public void activate_wand(){
		
		if(hitObject!=null){
			TorchObject torchObject = hitObject.GetComponent<TorchObject>();
			if(torchObject != null){
				torchObject.toggle_visibility();
				AudioSource audioSource = GetComponent<AudioSource>();
				audioSource.clip = soundClip;
				audioSource.Play();
				
			}
			
		}
	}
}

