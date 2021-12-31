using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerDirection direction;

    [HideInInspector] public float step_Length = 0.2f;
    [HideInInspector] public float movement_Frequency = 0.1f;
    private float counter;

    [SerializeField] private GameObject tailPrefab;

    private bool move;
    private bool create_Node_At_Tail;

    private List<Vector3> delta_Position;
    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;

    private Transform tr;

    void Awake()
    {
        tr = transform; // tr = GetComponent<Transform>(); �� ����?
        main_Body = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        delta_Position = new List<Vector3>()
        {
            new Vector3(-step_Length, 0f), // Left, -x
            new Vector3(0f, step_Length), // UP, y
            new Vector3(step_Length, 0f), // RIGHT, x
            new Vector3(0f, -step_Length) // DOWN, -y
        };
    }

    private void FixedUpdate()
    {
        if (move)
        {
            move = false; // �� �ڵ� ������ ��û������
            Move();
        }
    }

    void Update()
    {
        CheckMovementFrequency();
    }

    private void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>()); // Head
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>()); // Node
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>()); // Tail

        head_Body = nodes[0];
    }

    private void InitPlayer()
    {
        SetDirectionRandom();

        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;

            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;
        }
    }

    private void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
        Debug.Log(direction);
    }

    private void Move()
    {
        Vector3 dPosition = delta_Position[(int)direction];
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;

        main_Body.position = main_Body.position + dPosition; // �̰� �ȿŰ��൵ �Ǳ��ҰŰ�����
        head_Body.position = head_Body.position + dPosition; // head ��ġ�̵�

        // body�� tail ��ġ�̵�
        for (int i = 1; i < nodes.Count; i++) // i�� 1���� ����
        {
            prevPosition = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }

        // Fruit ������ ���ο� Node ����
        if (create_Node_At_Tail)
        {
            create_Node_At_Tail = false;

            // ������ ������ ��ġ�� �����ߴٰ� ������ �������� ���� ������ node�� ����
            // -1 ���ϸ� nodes[nodes.Count].position��� ��ġ��ü�� ���⶧���� ���ο� node�� ������ �ȵ�
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true); // false�൵ ���̾��µ�?
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }
    
    // �ֱ������� move�� true�� ���� ���������� �̵��ϰ���
    private void CheckMovementFrequency()
    {
        counter += Time.deltaTime;

        if (counter >= movement_Frequency)
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
        {
            return; // 180�� ȸ���Ͽ� �̵������� ���ϵ���
        }

        direction = dir;

        ForceMove();
    }

    // �ٲ� direction�� �����Ͽ� �̵�
    private void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(Tags.FRUIT))
        {
            target.gameObject.SetActive(false);
            create_Node_At_Tail = true;

            Gameplay.instance.IncreaseScore();

            AudioManager.instance.Play_PickUpSound();
        }

        // Game Over
        if (target.CompareTag(Tags.WALL) || target.CompareTag(Tags.BOMB) || target.CompareTag(Tags.TAIL))
        {
            print("Touched");
            Time.timeScale = 0f;
            AudioManager.instance.Play_DeadSound();
        }
    }
}
