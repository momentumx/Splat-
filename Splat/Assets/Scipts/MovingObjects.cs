using UnityEngine;

public class MovingObjects : MonoBehaviour {

    public float dir;
	
	// Update is called once per frame
	public virtual void FixedUpdate () {
        transform.position = new Vector3(transform.position.x + dir, transform.position.y, transform.position.z);
        
    }
}
