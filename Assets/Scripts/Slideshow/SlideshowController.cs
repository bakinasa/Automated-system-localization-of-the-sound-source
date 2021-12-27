using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlideshowController : MonoBehaviour
{
  [SerializeField]
  private SlideshowManager manager;
  [SerializeField]
  private RawImage recordingIndicator;
  [SerializeField]
  private Button quitButton;

  void Start()
  {
    UpdateIndicator();
    quitButton.onClick.AddListener(Quit);
  }

  void UpdateIndicator()
  {
    recordingIndicator.enabled = manager.IsRecording;
  }

  public void StartRecord()
  {
    if (manager.IsRecording) StopRecord();
    manager.StartRecording();
    UpdateIndicator();
  }

  public void StopRecord()
  {
    manager.StopRecording();
    UpdateIndicator();
  }

  public void StartStopRecord()
  {
    if (manager.IsRecording) StopRecord();
    else StartRecord();
  }

  public void Quit()
  {
    if (manager.IsRecording) StopRecord();
    manager.Dispose();
    SceneManager.LoadScene("Main");
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Quit();
    }
  }
}
