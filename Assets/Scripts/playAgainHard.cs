using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playAgainHard : MonoBehaviour
{
    

    
    // Start is called before the first frame update
    public void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene("Hard");
    }


}
