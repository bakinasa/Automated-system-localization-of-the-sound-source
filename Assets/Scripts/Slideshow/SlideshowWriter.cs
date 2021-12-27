using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class SlideshowWriter
{
  public DateTime timestamp;
  private FileStream file;

  public SlideshowWriter(string dir)
  {
    timestamp = DateTime.Now;
    file = File.OpenWrite(dir + "/" + Filename);
  }

  public void Add(Texture2D tex)
  {
    var bytes = tex.EncodeToPNG();
    file.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
    file.Write(BitConverter.GetBytes(tex.width), 0, 4);
    file.Write(BitConverter.GetBytes(tex.height), 0, 4);
    file.Write(bytes, 0, bytes.Length);
  }

  public string Filename
  {
    get
    {
      return timestamp.ToString("o");
    }
  }

  public string Dispose()
  {
    file.Close();
    return file.Name;
  }
}