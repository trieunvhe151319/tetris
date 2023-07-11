using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    //Playagain
    // Start is called before the first frame update
    public void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene("Tetris-Normal");
    }
}
