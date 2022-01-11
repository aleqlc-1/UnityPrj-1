using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class ShootScript : MonoBehaviour
{
    private GameController gc;

    public float power = 2f;

    public int dots = 15;

    private Vector2 startPos;

    private bool shoot, aiming;

    private GameObject Dots;
    public GameObject ballPrefab;
    public GameObject ballsContainer;

    private List<GameObject> projectilesPath; // ����ü ��ε��� �����ϴ� ����Ʈ

    private Rigidbody2D ballBody;

    void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        Dots = GameObject.Find("Dots");
    }

    void Start()
    {
        Dots = GameObject.Find("Dots");

        // �θ�ü�� Dots�� �ȵ���
        projectilesPath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);

        HideDots();
    }

    void Update()
    {
        ballBody = ballPrefab.GetComponent<Rigidbody2D>();

        // �� �ܰ賻���� 3�������� �� �� ����
        if (gc.shotCount <= 3 && !IsMouseOverUI())
        {
            Aim();
            Rotate();
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void Aim()
    {
        if (shoot) return;

        // Input.GetAxis�� -1.0f ���� 1.0f ������ ������ ���� ��ȯ�Ѵ�. ���̽�ƽó�� �ε巯�� �̵��� �ʿ��� ���
        // Input.GetAxisRaw�� -1, 0, 1 �� ���� �� �� �ϳ��� ��ȯ�ȴ�. Ű����ó�� ��� �����ؾ� �ϴ� ���
        // Update�� �־ ���� �Ѵ� Ŭ���ϸ� 1 Ŭ�����ϸ� 0�ε�?
        if (Input.GetAxis("Fire1") == 1)
        {
            if (!aiming)
            {
                aiming = true;
                startPos = Input.mousePosition; // ���콺 ó�� ���� ��ġ ����
                gc.CheckShotCount();
            }
            else
            {
                PathCalculation();
            }
        }
        else if (aiming && !shoot) // aiming�ϴٰ� ���콺 ���� ��
        {
            aiming = false;
            HideDots();
            StartCoroutine(Shoot());

            // ù ��° �߻�ÿ��� ī�޶� ȸ��, �ٷ����� Shoot�� �ڷ�ƾ�̹Ƿ� gc.shotCount++ �Ǳ����� �˻簡��
            if (gc.shotCount == 1)
                Camera.main.GetComponent<CameraTransition>().RotateCameraToSide();
        }
    }

    private void PathCalculation()
    {
        // mass�� Ŭ���� ���ϰ� �߻�ǵ��� �ϱ����� ballBody.mass�� ������
        // vel���� ���콺�� ó�� Ŭ���Ѱ����κ��� �ָ� �̵��Ҽ��� Ŀ��
        Vector2 vel = ShootForce(Input.mousePosition) * Time.fixedDeltaTime / ballBody.mass;
        //Debug.Log("ShootForce" + vel);

        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 15f; // ����ü�� �� 16���̹Ƿ� 15�� ����
            Vector3 point = DotPath(transform.position, vel, t);
            //Debug.Log("DotPath : " + point);
            point.z = 1;
            projectilesPath[i].transform.position = point;
        }
    }

    // ���콺 ó����ġ - ���콺 ������ġ �� ����
    private Vector2 ShootForce(Vector3 force)
    {
        return (new Vector2(startPos.x, startPos.y) - new Vector2(force.x, force.y)) * power;
    }

    // ����ü ������ ��ġ ����
    private Vector2 DotPath(Vector2 startP, Vector2 startVel, float t)
    {
        // Physics2D.gravity�� (0.0, -9.8)
        // (startVel * t)�� t�� 1�϶� ���� ������ ����ü�� ��ġ�̹Ƿ� t�� ���� Ű������ startP�� ���ؼ� ���� ����ü�� ���� ����ü���� �� ������ ������ ����
        // (Physics2D.gravity * t * t * 0.5f)�� ������ ����ü�� �߷��� ������ ���������� �׸��� ����. �� �� �ȴ��ϸ� �������� �׷���
        //Debug.Log((startP));
        //Debug.Log((startVel * t));
        //Debug.Log(Physics2D.gravity * t * t * 0.5f);
        //Debug.Log((startP) + (startVel * t) + (Physics2D.gravity * t * t * 0.5f));
        return (startP) + (startVel * t) + (Physics2D.gravity * t * t * 0.5f);
    }

    private void ShowDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
            //Debug.Log(projectilesPath[i].gameObject.name);
        }
    }

    private void HideDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
            //Debug.Log(projectilesPath[i].gameObject.name);
        }
    }

    private void Rotate()
    {
        Vector2 dir = GameObject.Find("dot (1)").transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // ĳ�� �ڽ��ݶ��̴� �������. ������ �ݴ�� �߻� �ȵǰ� �߻���ġ�� ��¦����������
    private IEnumerator Shoot()
    {
        for (int i = 0; i < gc.ballsCount; i++)
        {
            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(ballsContainer.transform);
            ballBody = ball.GetComponent<Rigidbody2D>();
            ballBody.AddForce(ShootForce(Input.mousePosition));

            int balls = gc.ballsCount - i;
            gc.ballsCountText.text = (gc.ballsCount - i - 1).ToString();
        }

        yield return new WaitForSeconds(0.5f);
        gc.shotCount++;
        gc.ballsCountText.text = gc.ballsCount.ToString();
    }
}
