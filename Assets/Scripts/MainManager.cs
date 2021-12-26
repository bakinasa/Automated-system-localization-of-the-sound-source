using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
  [SerializeField]
  private Button calibrationButton;
  [SerializeField]
  private Button magicButton;
  [SerializeField]
  private Button galleryButton;

  void Start()
  {
    calibrationButton.onClick.AddListener(() => SceneManager.LoadScene("Calibration"));
    magicButton.onClick.AddListener(() => SceneManager.LoadScene("Magic"));
    galleryButton.onClick.AddListener(() => SceneManager.LoadScene("Gallery"));
  }
}
