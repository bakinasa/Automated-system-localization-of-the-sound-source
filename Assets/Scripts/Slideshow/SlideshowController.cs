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

  public async Task StartRecord()
  {
    if (manager.IsRecording) await StopRecord();
    manager.StartRecording();
    UpdateIndicator();
  }

  public async Task StopRecord()
  {
    var ss = manager.StopRecording();
    UpdateIndicator();
    await ss.SaveToFile(Application.persistentDataPath);
  }

  public async void StartStopRecord()
  {
    if (manager.IsRecording) await StopRecord();
    else await StartRecord();
  }

  public async void Quit()
  {
    if (manager.IsRecording) await StopRecord();
    SceneManager.LoadScene("Main");
  }
}
