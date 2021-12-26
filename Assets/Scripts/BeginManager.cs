using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginManager : MonoBehaviour
{
  private const string prefsKey = "permissionsGranted";
  private string[] permissions = { Permission.Camera, Permission.Microphone };
  [SerializeField]
  private Button nextButton;


  private int grantedCount = 0;

  internal void Denied(string permission)
  {
    print(permission);
    PlayerPrefs.SetInt(prefsKey, 0);
    PlayerPrefs.Save();
    Application.Quit();
  }

  internal void Granted(string permission)
  {
    grantedCount++;
    if (grantedCount >= permissions.Length)
    {
      PlayerPrefs.SetInt(prefsKey, 1);
      PlayerPrefs.Save();
      SceneManager.LoadScene("Main");
    }
  }

  void RequestPermissons()
  {
    if (permissions.Length == 0)
    {
      PlayerPrefs.SetInt(prefsKey, 1);
      PlayerPrefs.Save();
      SceneManager.LoadScene("Main");
      return;
    }
    var callbacks = new PermissionCallbacks();
    callbacks.PermissionDenied += Denied;
    callbacks.PermissionDeniedAndDontAskAgain += Denied;
    callbacks.PermissionGranted += Granted;
    Permission.RequestUserPermissions(permissions, callbacks);
  }

  void Start()
  {
    nextButton.onClick.AddListener(RequestPermissons);
    if (PlayerPrefs.GetInt(prefsKey, 0) == 1)
    {
      RequestPermissons();
    }
  }
}
