using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    public GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, at, Quaternion.identity);
    }

    public GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab);
    }

    public GameObject Instantiate(string path, Transform under)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, under);
    }

    public Sprite LoadSprite<T>(string filePath)
    {
      Sprite load = Resources.Load<Sprite>(filePath);
      return load;
    }
  }
}