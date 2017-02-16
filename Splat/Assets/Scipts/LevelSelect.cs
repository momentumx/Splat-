using UnityEngine;

public class LevelSelect : MonoBehaviour {
    public void SelectLevel(string _level ) {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( _level );
    }

    public void SelectLevel ( int _level ) {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( _level );
    }

    public void SelectLevel ( ) {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( GameController.level +1);
    }
}
