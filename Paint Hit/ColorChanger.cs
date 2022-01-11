using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ball�� ������ ��ũ��Ʈ
public class ColorChanger : MonoBehaviour
{
    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "red") // ���ӿ���
        {
            base.gameObject.GetComponent<Collider>().enabled = false; // ?
            target.gameObject.GetComponent<MeshRenderer>().enabled = true;
            target.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            base.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.Impulse); // �����������߸� ball�� �Ʒ��� ������
            Destroy(gameObject, 0.5f); // �� �ı�
            SceneManager.LoadScene(0);
        }
        else
        {
            base.gameObject.GetComponent<Collider>().enabled = true; // �̹� Ȱ��ȭ���ִµ�?
            GameObject gameObject = Instantiate(Resources.Load("splash1")) as GameObject; // ���� ����
            gameObject.transform.parent = target.gameObject.transform; // ������ �������� �پ ȸ���ϵ���
            Destroy(gameObject, 0.1f); // ���� ����
            target.gameObject.name = "color";
            target.gameObject.tag = "red"; // ������� �� ���߸� ���ӿ����ǵ���
            StartCoroutine(ChangeColor(target.gameObject));
        }
    }

    private IEnumerator ChangeColor(GameObject g)
    {
        yield return new WaitForSeconds(0.1f);
        g.gameObject.GetComponent<MeshRenderer>().enabled = true;
        g.gameObject.GetComponent<MeshRenderer>().material.color = BallHandler.oneColor; // ���� ���������� ball�� ����� ����
        Destroy(base.gameObject); // �� ����
    }
}
