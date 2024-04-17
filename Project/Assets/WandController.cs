using UnityEngine;

public class WandController : MonoBehaviour {

	public enum HandType : int { LeftHand, RightHand };
	[Header( "Hand Properties" )]
	public HandType handType;
	[Header( "Player Controller" )]
	public MainPlayerController playerController;
	static protected WandObject[] anchors_in_the_scene;
	
	protected WandObject wand_grasped = null;// wand that is currently in hand
	

	void Start () {
		if ( anchors_in_the_scene == null ) anchors_in_the_scene = GameObject.FindObjectsOfType<WandObject>();		
	}

	void Update () { handle_controller_behavior(); }
	
	
/////////////////////////////commands

	protected bool try_to_grab () //grabbing wand
	{
		if ( handType == HandType.LeftHand ) return
			OVRInput.Get( OVRInput.Button.Three );                 
		else return
			OVRInput.Get( OVRInput.Button.One );                   
	}
	protected bool try_to_release ()//releasing wand
	{
		if ( handType == HandType.LeftHand ) return
			OVRInput.Get( OVRInput.Button.Four );                  
		else return
			OVRInput.Get( OVRInput.Button.Two );                   
	}
	protected bool try_to_interact_with_puzzle()//move puzzle piece
	{
		return OVRInput.GetDown( OVRInput.Button.SecondaryHandTrigger)&& OVRInput.GetDown( OVRInput.Button.PrimaryHandTrigger);

	}	
	protected bool try_to_light_torch()//light torch
	{
		return OVRInput.GetDown( OVRInput.Button.PrimaryIndexTrigger)&& OVRInput.GetDown( OVRInput.Button.SecondaryIndexTrigger);
        // foreach (OVRInput.Button button in desiredSequence)
        // {
            // if (OVRInput.GetDown(button))
            // {
                // if (button == desiredSequence[currentStep])
                // {
                    // if (Time.time - lastPressTime <= timeBetweenPresses)
                    // {
                        // currentStep++;
                        // if (currentStep == desiredSequence.Length)
                        // {
                            // Debug.Log("Sequence completed successfully!");
							// if(wand_grasped!=null){
								// wand_grasped.light_torch();
							// }
                            // ResetSequence();
                        // }
                    // }
                    // else
                    // {
                        // ResetSequence();
                    // }
                    // lastPressTime = Time.time;
                // }
                // else
                // {
                    // ResetSequence();
                // }
            // }
        // }
    }

    // void ResetSequence()
    // {
        // currentStep = 0;
        // lastPressTime = 0.0f;
    // }	
	// private OVRInput.Button[] desiredSequence = { OVRInput.Button.PrimaryIndexTrigger};//, OVRInput.Button.SecondaryIndexTrigger, OVRInput.Button.PrimaryIndexTrigger, OVRInput.Button.SecondaryIndexTrigger };
	// private int currentStep = 0;
	// public float timeBetweenPresses = 1.0f;
	// private float lastPressTime = 0.0f;
	
///////////////////////////////////////////	


	
	
	
	protected void handle_controller_behavior () {
		
		
		if (try_to_grab() && wand_grasped == null)//grab wand
		{

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
		
		if (try_to_release() && wand_grasped != null)//release wand
		{
			wand_grasped.detach_from( this );
			wand_grasped=null;
		}
		
		if(try_to_light_torch() && wand_grasped != null)//light torch
		{
			GameObject hitObject=wand_grasped.get_hitObject();//object pointed at by wand
			if(hitObject!=null){
				TorchObject torchObject = hitObject.GetComponent<TorchObject>();
				if(torchObject != null){
					torchObject.toggle_visibility();
				}
			}	
		}
		
		if(try_to_interact_with_puzzle() && wand_grasped != null)
		{
			GameObject hitObject=wand_grasped.get_hitObject();//object pointed at by wand
			if(hitObject!=null){
				PuzzleObject puzzleObject = hitObject.GetComponent<PuzzleObject>();
				if(puzzleObject != null){
					// do something ?
				}
			}				
		}

	}
}
