using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Android;

public class App : MonoBehaviour
{
	public static App instance;

	//// ������ؽ�Ʈ
	[SerializeField] private TextMeshProUGUI TouchStartText;
	//[SerializeField] private TextMeshProUGUI CaptureText;
	//[SerializeField] private TextMeshProUGUI AddImageStartText;
	//[SerializeField] private TextMeshProUGUI AddImageEndText;
	//[SerializeField] private TextMeshProUGUI ImageStartChangeText;
	//[SerializeField] private TextMeshProUGUI ImageAddChangeText;
	//[SerializeField] private TextMeshProUGUI ImageUpdateChangeText;
	//[SerializeField] private TextMeshProUGUI ImageRemoveChangeText;

	void Awake()
	{
		if (instance == null)
			instance = this;
	}

	void Start()
    {
		//      // ����Ƽ ������Ʈ �������� ���¹����� instantiate
		//      StartCoroutine(AssetManager.instance.LoadFromMemoryAsync("Assets/AssetBundles/building", (bundle) =>
		//{
		//          Debug.LogFormat("bundle: {0}", bundle);
		//          var prefab = bundle.LoadAsset<GameObject>("202bundle"); // ����ȿ� �ִ� Ư���� ������ �ε�
		//          var model = Instantiate<GameObject>(prefab);
		//}));

		// // ������ �ִ� ���¹��� ���̳ʸ�ȭ ���� �ٷ� ������
		//StartCoroutine(AssetManager.instance.LoadFromServer("C:\\Users\\TAESUNG SNI\\Desktop\\NodeToUnity\\AssetBundles\\building"));

		//// ������ �ִ� ���¹��� ���̳ʸ�ȭ�Ͽ� �����ͼ� ��������ҿ� write, PC
		//StartCoroutine(AssetManager.instance.LoadFromServer("C:\\Users\\TAESUNG SNI\\Desktop\\NodeToUnity\\AssetBundles", "building"));

		// ������ �ִ� ���¹��� ���̳ʸ�ȭ�Ͽ� �����ͼ� ��������ҿ� write
		if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
		{
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
		StartCoroutine(AssetManager.instance.LoadFromServer("C:\\Users\\TAESUNG SNI\\Desktop\\NodeToUnity\\AssetBundles", "building"));
	}

    public void LoadAsset()
	{
		//// ������ �ִ� ���¹��� �񵿱�� ���̳ʸ�ȭ�Ͽ� ������, ��ųʸ����� ����, PC
		//StartCoroutine(AssetManager.instance.LoadFromFileAsync(Application.persistentDataPath, "building", () =>
		//{
		//	  Debug.Log(Application.persistentDataPath.ToString());
		//    var prefab = AssetManager.instance.LoadAsset("building", "202bundle");
		//    var go = Instantiate<GameObject>(prefab);
		//}));

		// ������ �ִ� ���¹��� �񵿱�� ���̳ʸ�ȭ�Ͽ� ������, ��ųʸ����� ����, AR Mobile
		StartCoroutine(AssetManager.instance.LoadFromFileAsync(Application.persistentDataPath, "building", () =>
		{
			TouchStartText.text = Application.persistentDataPath.ToString();
			var prefab = AssetManager.instance.LoadAsset("building", "202bundle");
			TouchManager.instance.PlaceObject = prefab;
		}));
	}
}
