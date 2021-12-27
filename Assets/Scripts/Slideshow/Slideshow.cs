using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class Slideshow
{
  public List<Texture2D> images;
  public DateTime timestamp;

  public Slideshow()
  {
    timestamp = DateTime.Now;
    images = new List<Texture2D>();
  }

  public string Filename
  {
    get
    {
      return timestamp.ToString("o");
    }
  }

  public static async Task<Slideshow> LoadFromFile(string fname)
  {
    var file = File.OpenRead(fname);
    var ss = new Slideshow();
    var intBuf = new byte[4];
    int i = 0;
    var len = file.Length;
    while (i < len)
    {
      await file.ReadAsync(intBuf, 0, 4);
      var imlen = BitConverter.ToInt32(intBuf, 0);
      await file.ReadAsync(intBuf, 0, 4);
      var w = BitConverter.ToInt32(intBuf, 0);
      await file.ReadAsync(intBuf, 0, 4);
      var h = BitConverter.ToInt32(intBuf, 0);
      var bytes = new byte[imlen];
      await file.ReadAsync(bytes, 0, imlen);
      var tex = new Texture2D(w, h);
      tex.LoadImage(bytes);
      tex.Apply();
      ss.images.Add(tex);
      i += 12 + imlen;
    }
    ss.timestamp = DateTime.Parse(Path.GetFileName(fname), null, System.Globalization.DateTimeStyles.RoundtripKind);
    return ss;
  }
}