using UnityEngine;

public class MovingObjects : MonoBehaviour {

    public float dir;
    public bool bothWays;
    protected float myWidth;
    public AudioClip soundFX;

    protected virtual void Start () {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x = Random.Range ( -20, Screen.width + 21 );
        transform.position = new Vector2 ( Camera.main.ScreenToWorldPoint ( pos ).x, transform.position.y );
        if ( bothWays && Random.Range ( 0, 2 ) == 0 ) {
            dir *= -1;
            transform.localScale = new Vector3 ( transform.localScale.x * Mathf.Sign ( dir ), transform.localScale.y, transform.localScale.z );
        }
        myWidth = GetComponent<SpriteRenderer> ().bounds.extents.x;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate () {
        transform.position += new Vector3(dir, 0, 0);
    }

    public virtual void Die () {
        Destroy ( gameObject );
    }
}
