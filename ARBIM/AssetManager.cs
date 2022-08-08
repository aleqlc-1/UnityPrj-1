using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class AssetManager : MonoBehaviour
{
    public static AssetManager instance;
    public Dictionary<string, AssetBundle> dicBundles = new Dictionary<string, AssetBundle>();

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

 //   // ����Ƽ ������ ���¹��� ������ ����Ʈ�迭�� �о �񵿱� ������� �ε�
 //   public IEnumerator LoadFromMemoryAsync(string path, System.Action<AssetBundle> callback)
	//{
 //       byte[] binary = File.ReadAllBytes(path);
 //       Debug.Log("binary.Length : " + binary.Length);
 //       AssetBundleCreateRequest req = AssetBundle.LoadFromMemoryAsync(binary);
 //       yield return req;
 //       callback(req.assetBundle);
	//}

    // �������� �о�ͼ� ��������ҿ� write�س��� ���¹����� ��ųʸ��� �����ϰ� �ݹ��� ȣ���Ͽ� instantiate
    public IEnumerator LoadFromFileAsync(string path, string fileName, System.Action callback)
    {
        var req = AssetBundle.LoadFromFileAsync(string.Format("{0}/{1}", path, fileName));
        yield return req;
        var bundle = req.assetBundle;
        Debug.LogFormat("bundle: {0}", bundle);
        dicBundles.Add(fileName, bundle);
        callback();
    }

    // �������� ������ ���鿡�� Ư�� ���� �ε�
    public GameObject LoadAsset(string bundleName, string prefabName)
    {
        return this.dicBundles[bundleName].LoadAsset<GameObject>(prefabName);
    }

 //   // ���������� �ִ� ���¹��� �о��. �߰������� ����
 //   public IEnumerator LoadFromServer(string uri)
	//{
	//	UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(uri);
	//	yield return req.SendWebRequest();

	//	AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(req);
	//	Debug.Log(bundle);
	//}

    // �������� ���¹��� �����ͼ� ��������ҿ� write
	public IEnumerator LoadFromServer(string path, string fileName)
    {
        // �������� building ����Ʈ�� ������
        string bundleUri = string.Format("{0}\\{1}", path, fileName);
        Debug.Log(bundleUri);
        UnityWebRequest req1 = UnityWebRequest.Get(bundleUri);
        yield return req1.SendWebRequest();
        var bytes = req1.downloadHandler.data;
        Debug.Log("bytes.Length : " + bytes.Length);

        // �������� building.manifest �� ���ڿ��� ������
        var manifestUri = string.Format("{0}.manifest", bundleUri);
        UnityWebRequest req2 = UnityWebRequest.Get(manifestUri);
        yield return req2.SendWebRequest();
        var manifest = Encoding.UTF8.GetString(req2.downloadHandler.data);

        // building�� ��������ҿ� write
        string bundlePath = string.Format("{0}\\{1}", Application.persistentDataPath, fileName);
        Debug.Log("bundlePath : " + bundlePath);
        File.WriteAllBytes(bundlePath, bytes);

        // building.manifest�� ��������ҿ� write
        string manifestPath = string.Format("{0}.manifest", bundlePath);
        File.WriteAllBytes(manifestPath, ObjectToByteArray(manifest));
    }

    // building.manifest�� �迭�� �ٲ㼭 ��ȯ
    private byte[] ObjectToByteArray(object obj)
	{
        if (obj == null) return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);
        return ms.ToArray();
	}
}
