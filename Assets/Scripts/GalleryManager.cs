using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
  private string[] filenames;
  [SerializeField]
  private VerticalLayoutGroup wrapper;
  [SerializeField]
  private GameObject listItem;
  [SerializeField]
  private GameObject slideshowWrapper;
  [SerializeField]
  private Button backButton;
  [SerializeField]
  private Button deleteButton;
  [SerializeField]
  private ViewportManager viewport;

  private string selectedFname;
  private Slideshow selectedSlideshow;
  private Coroutine showSelectedCoroutine;

  private bool videoVerticallyMirrored = false;
  private int videoRotationAngle = 0;

  void ApplyCameraSettings()
  {
    videoVerticallyMirrored = PlayerPrefs.GetInt("mirrored", 0) == 1;
    videoRotationAngle = PlayerPrefs.GetInt("orient", 0);
    viewport.mirrored = videoVerticallyMirrored;
    viewport.orient = videoRotationAngle;
  }

  string[] ListFileNames()
  {
    var fnames = new List<string>();
    var info = new DirectoryInfo(Application.persistentDataPath);
    var regex = @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{7}\+\d{2}:\d{2}$";
    foreach (var file in info.GetFiles())
    {
      if (Regex.IsMatch(file.Name, regex))
        fnames.Add(file.FullName);
    }
    return fnames.ToArray();
  }

  async Task Select(string fname)
  {
    wrapper.transform.parent.gameObject.SetActive(false);
    selectedFname = fname;
    selectedSlideshow = await Slideshow.LoadFromFile(fname);
    slideshowWrapper.SetActive(true);
    showSelectedCoroutine = StartCoroutine(ShowSlideshow());
  }

  float DrawObject(string fname, RectTransform parent)
  {
    var text = DateTime.Parse(Path.GetFileName(fname), null, System.Globalization.DateTimeStyles.RoundtripKind);
    var item = Instantiate(listItem, Vector3.zero, Quaternion.identity);
    item.GetComponentInChildren<Text>().text = " " + text.ToString();
    var rect = item.GetComponent<RectTransform>();
    var btn = item.GetComponent<Button>();
    btn.onClick.AddListener(async () => await Select(fname));
    rect.SetParent(parent);
    return rect.rect.height;
  }

  void DrawFilenames()
  {
    var parent = wrapper.GetComponent<RectTransform>();
    var dh = wrapper.spacing;
    var height = -dh;
    foreach (RectTransform child in parent)
    {
      Destroy(child.gameObject);
    }
    foreach (var fname in filenames)
    {
      height += DrawObject(fname, parent) + dh;
    }
    parent.sizeDelta = new Vector2(parent.rect.width, height);
  }

  void Start()
  {
    ApplyCameraSettings();
    slideshowWrapper.SetActive(false);
    wrapper.transform.parent.gameObject.SetActive(true);
    filenames = ListFileNames();
    DrawFilenames();
    backButton.onClick.AddListener(PressBack);
    deleteButton.onClick.AddListener(PressDelete);
  }

  void PressBack()
  {
    if (showSelectedCoroutine != null)
    {
      StopCoroutine(showSelectedCoroutine);
      showSelectedCoroutine = null;
    }
    selectedSlideshow = null;
    selectedFname = null;
    wrapper.transform.parent.gameObject.SetActive(true);
    slideshowWrapper.SetActive(false);
  }

  void PressDelete()
  {
    if (selectedFname == null) return;
    File.Delete(selectedFname);
    PressBack();
    filenames = ListFileNames();
    DrawFilenames();
  }

  IEnumerator<WaitForSecondsRealtime> ShowSlideshow()
  {
    const float DELTA_T = SlideshowManager.DELTA_T;
    for (int i = 0; selectedSlideshow != null; i++)
    {
      viewport.SetTexture(selectedSlideshow.images[i % selectedSlideshow.images.Count]);
      yield return new WaitForSecondsRealtime(DELTA_T);
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (selectedFname == null) SceneManager.LoadScene("Main");
      else
        PressBack();
    }
  }
}
