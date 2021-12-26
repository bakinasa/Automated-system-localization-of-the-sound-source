using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationManager : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SceneManager.LoadScene("Main");
    }
  }
}
