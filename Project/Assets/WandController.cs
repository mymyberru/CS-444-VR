using UnityEngine;

public class WandController : MonoBehaviour {

	// Store the hand type to know which button should be pressed
	public enum HandType : int { LeftHand, RightHand };
	[Header( "Hand Properties" )]
	public HandType handType;


	// Store the player controller to forward it to the object
	[Header( "Player Controller" )]
	public MainPlayerController playerController;
	

	static protected WandObject[] anchors_in_the_scene;
	protected WandObject wand_grasped = null;
	
	
	
	
	void Start () {
		// Prevent multiple fetch
		if ( anchors_in_the_scene == null ) anchors_in_the_scene = GameObject.FindObjectsOfType<WandObject>();
		
	}

	void Update () { handle_controller_behavior(); }

	
	
	protected bool try_to_grab () {
		// Case of a left hand
		if ( handType == HandType.LeftHand ) return
			OVRInput.Get( OVRInput.Button.Three );                           // Check that the A button is pressed
		// Case of a right hand
		else return
			OVRInput.Get( OVRInput.Button.One );                             // Check that the A button is pressed

	}
	protected bool try_to_release () {
		if ( handType == HandType.LeftHand ) return
			OVRInput.Get( OVRInput.Button.Four );                         // Check that the B button is pressed
		// Case of a right hand
		else return
			OVRInput.Get( OVRInput.Button.Two );                          // Check that the B button is pressed

	}
	
	private OVRInput.Button[] desiredSequence = { OVRInput.Button.PrimaryIndexTrigger};//, OVRInput.Button.SecondaryIndexTrigger, OVRInput.Button.PrimaryIndexTrigger, OVRInput.Button.SecondaryIndexTrigger };
	private int currentStep = 0;
	public float timeBetweenPresses = 1.0f;
	private float lastPressTime = 0.0f;
	
	protected void activate_wand()
	{
		if (OVRInput.GetDown( OVRInput.Button.PrimaryIndexTrigger)&& OVRInput.GetDown( OVRInput.Button.SecondaryIndexTrigger)){
			wand_grasped.activate_wand();
		}
        foreach (OVRInput.Button button in desiredSequence)
        {
            if (OVRInput.GetDown(button))
            {
                if (button == desiredSequence[currentStep])
                {
                    if (Time.time - lastPressTime <= timeBetweenPresses)
                    {
                        currentStep++;
                        if (currentStep == desiredSequence.Length)
                        {
                            Debug.Log("Sequence completed successfully!");
							if(wand_grasped!=null){
								wand_grasped.activate_wand();
							}
                            ResetSequence();
                        }
                    }
                    else
                    {
                        ResetSequence();
                    }
                    lastPressTime = Time.time;
                }
                else
                {
                    ResetSequence();
                }
            }
        }
    }

    void ResetSequence()
    {
        currentStep = 0;
        lastPressTime = 0.0f;
    }
	
	
	
	protected void handle_controller_behavior () {

		bool grab_attempt = try_to_grab();
		bool release_attempt=try_to_release();
		
		
		//try to grab item
		if (grab_attempt && wand_grasped == null){

			Debug.LogWarningFormat( "{0} get closed", this.transform.parent.name );
			int best_object_id = -1;
			float best_object_distance = float.MaxValue;
			float oject_distance;
			for ( int i = 0; i < anchors_in_the_scene.Length; i++ ) {
				if ( !anchors_in_the_scene[i].is_available() ) continue;
				oject_distance = Vector3.Distance( this.transform.position, anchors_in_the_scene[i].transform.position );
				if ( oject_distance < best_object_distance && oject_distance <= anchors_in_the_scene[i].get_grasping_radius() ) {
					best_object_id = i;
					best_object_distance = oject_distance;
				}
			}
			if ( best_object_id != -1 ) {
				wand_grasped = anchors_in_the_scene[best_object_id];
				Debug.LogWarningFormat( "{0} grasped {1}", this.transform.parent.name, wand_grasped.name );
				wand_grasped.attach_to( this );
			}			
		}
		
		
		//release item
		if (release_attempt && wand_grasped != null){
			Debug.LogWarningFormat("{0} released {1}", this.transform.parent.name, wand_grasped.name );
			wand_grasped.detach_from( this );
			wand_grasped=null;
		}
		
		//show ray and activate_wand
		if(wand_grasped != null){
			
			wand_grasped.DisplayRay();
			activate_wand();

		}

	}
}
