using UnityEngine;


public class ItemScript : MonoBehaviour {

    public byte kills;
    public float speed, magnitude;
    public AudioClip crashSFX, collideSFX;

    public virtual void Start()
    {
        GetComponent<Rigidbody2D>().velocity += new Vector2(0, speed);
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        GetComponent<Animator>().SetTrigger("Crash");
        GameController balloon = GameObject.Find("Balloon").GetComponent<GameController>();
        balloon.audioSource.PlayOneShot( crashSFX );
        balloon.ShakeCall(magnitude);
        //die gets called in the animator
    }

    public void Die()
    {
        if ( kills > 0 ) {
            GameObject text = Instantiate ( Resources.Load<GameObject> ( "KillText" ) );
            text.GetComponent<UnityEngine.UI.Text> ().text = GameController.kills [ kills-1 ];
            text.transform.position = Camera.main.WorldToScreenPoint ( transform.position );
        }
        Destroy(gameObject);
    }
    public void ChangeTag(string _tag ) {
        tag = _tag;
    }
}
