using UnityEngine;

public class FlyingObjectsScript : MovingObjects {
    public enum Type
    {
        delete,
        wrapping,
        turning
    }
    public Type myType;
    
	// Use this for initialization
	protected override void Start () {
        //this will override anything you put into the editor, bc that happens first
        dir = Random.Range(.1f, .5f);
        base.Start ();
        transform.position = new Vector2( transform.position.x, transform.position.y + Random.Range ( -15, 16 ) );
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        float width = Screen.width * .5f;
        if (Mathf.Abs(Camera.main.WorldToScreenPoint(transform.position).x - width)-width > 21)
        {
            switch (myType)
            {
                case Type.wrapping:
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    pos.x = width - (width + 20) * Mathf.Sign(dir);
                    transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, transform.position.y, transform.position.z);
                    break;
                case Type.turning:
                    dir *= -1;
                    transform.position += new Vector3(dir, 0,0);
                    transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
                    break;
                case Type.delete:
                    Destroy(gameObject);
                    break;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        GameObject.Find ( "Balloon" ).GetComponent<AudioSource> ().PlayOneShot ( soundFX );
        GetComponent<Animator> ().SetTrigger ( "Collided" );
    }
}
