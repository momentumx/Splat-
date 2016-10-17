using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {
    public void SelectLevel(string _level ) {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( _level );
    }
}
