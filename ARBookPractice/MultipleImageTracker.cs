using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultipleImageTracker : MonoBehaviour
{
    private ARTrackedImageManager imageManager;

    void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        // �̹��� �ν� ��������Ʈ�� ����� �Լ��� ����
        imageManager.trackedImagesChanged += OnTrackedImage;

        StartCoroutine(TurnOnImageTracking());
    }

    public void OnTrackedImage(ARTrackedImagesChangedEventArgs args)
	{
        // ���� �ν��� �̹������� ��� ��ȸ�Ѵ�.
		foreach (ARTrackedImage trackedImage in args.added)
		{
            // �̹��� ���̺귯������ �ν��� �̹����� �̸��� �����´�
            string imageName = trackedImage.referenceImage.name;

            // Resources �������� �ν��� �̹����� �̸��� ������ �̸��� �������� ã�´�.
            GameObject imagePrefab = Resources.Load<GameObject>(imageName);

            if (imagePrefab != null)
			{
                if (trackedImage.transform.childCount < 1) // �̹� ������ �������� ���ٸ�
				{
                    // �̹��� ��ġ�� �������� �����ϰ� �̹����� �ڽĿ�����Ʈ�� ���
                    GameObject go = Instantiate(imagePrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    go.transform.SetParent(trackedImage.transform);
                }
			}

            // Firebase ����
            // �ڽ��� ���� ��ġ��ǥ�� �������·� ��ȯ�Ѵ�.
            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude, GPS_Manager.instance.longitude);

            // DB�˻� �� ������ ���� �ڷ�ƾ�Լ��� ����
            StartCoroutine(DB_Manager.instance.LoadData(myPos, trackedImage.transform));
		}

		foreach (ARTrackedImage trackedImage in args.updated)
		{
            // �̹� ������ �������� �ִٸ�
            if (trackedImage.transform.childCount > 0)
			{
                // �νĵ� �̹����� ��ġ�� ����Ǹ� �ڽİ�ü�� �������� ��ġ�� ����ǵ���
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
			}
		}
	}

    public IEnumerator TurnOnImageTracking()
	{
        imageManager.enabled = false;

        // ��ġ������ ���ŵ� ������ ����Ѵ�.
        while (!GPS_Manager.instance.receiveGPS)
		{
            yield return null;
		}

        imageManager.enabled = true;

        // �̹��� �ν� ��������Ʈ�� ����� �Լ��� �����Ѵ�
        imageManager.trackedImagesChanged += OnTrackedImage;
	}
}
