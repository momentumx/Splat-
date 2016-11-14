using UnityEngine;


public class ItemScript : MonoBehaviour {

    public byte kills;
    public float speed, magnitude;
    public AudioClip crashSFX, collideSFX;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity += new Vector2(0, speed);
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        GetComponent<Animator>().SetTrigger("Crash");
        GameController balloon = GameObject.Find("Balloon").GetComponent<GameController>();
        balloon.audioSource.PlayOneShot( crashSFX );
        balloon.ShakeCall(magnitude);
        //the box cllider gets taken away in the animator since not all lose there collider
        //die gets called in teh animator
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
