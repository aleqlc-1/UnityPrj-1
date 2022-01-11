using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameState gameState;

    // �迭 ��ü �ν��Ͻ�ȭ(�迭ũ�����). �迭��Ұ� ���� class�̹Ƿ� ���� ��üȭ�������.
    private PuzzlePiece[,] Matrix = new PuzzlePiece[GameVariables.MaxRows, GameVariables.MaxColumns];

    private int puzzleIndex;
    private GameObject[] puzzlePieces;
    private Sprite[] puzzleImages;

    private PuzzlePiece PieceToAnimate;
    private Vector3 screenPositionToAnimate;
    private int toAnimateRow, toAnimateColumn;
    private float animSpeed = 10f;

    void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (puzzleIndex > 0)
            {
                LoadPuzzle();
                GameStarted();
            }
        }
    }

    void Start()
    {
        puzzleIndex = -1;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            switch (gameState)
            {
                case GameState.Playing:
                    CheckInput();
                    break;

                case GameState.Animating:
                    AnimateMovement(PieceToAnimate, Time.deltaTime);
                    CheckIfAnimationEnded();
                    break;

                case GameState.End:
                    
                    break;
            }
        }
    }

    private void LoadPuzzle()
    {
        puzzleImages = Resources.LoadAll<Sprite>("Sprites/BG " + puzzleIndex);

        puzzlePieces = GameObject.Find("Puzzle Holder").GetComponent<PuzzleHolder>().puzzlePieces;

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            puzzlePieces[i].GetComponent<SpriteRenderer>().sprite = puzzleImages[i];
        }
    }

    private void GameStarted()
    {
        // �����߿� �������� �ϳ��̾Ƽ� ��Ȱ��ȭ
        int index = Random.Range(0, GameVariables.MaxSize);
        puzzlePieces[index].SetActive(false);

        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                // GameVariables.MaxColumns ���ϴ������� �迭�� �Ϸķ� �� �����Ƿ�
                if (puzzlePieces[row * GameVariables.MaxColumns + column].activeInHierarchy)
                {
                    Vector3 point = GetScreenCoordinatesFromViewport(row, column);
                    puzzlePieces[row * GameVariables.MaxColumns + column].transform.position = point;

                    // �迭��� ������ �ν��Ͻ�ȭ(�迭��� ������ Ŭ�����̹Ƿ�)
                    // ��ü�� �����Ƿ� ������Ƽ�� ���� null�̶� Matrix[row, column]�� null�� �ƴ�
                    Matrix[row, column] = new PuzzlePiece();
                    Matrix[row, column].obj = puzzlePieces[row * GameVariables.MaxColumns + column];
                    Matrix[row, column].OriginalRow = row;
                    Matrix[row, column].OriginalColumn = column;
                }
                else
                {
                    Matrix[row, column] = null; // ��ü�� ���� �ʰ� null�� �־����Ƿ� null��
                }
            }
        }

        Shuffle();
        gameState = GameState.Playing;
    }

    private void Shuffle()
    {
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                if (Matrix[row, column] == null) continue;

                int random_row = Random.Range(0, GameVariables.MaxRows);
                int random_column = Random.Range(0, GameVariables.MaxColumns);

                Swap(row, column, random_row, random_column);
            }
        }
    }

    private void Swap(int row, int column, int random_row, int random_column)
    {
        PuzzlePiece temp = Matrix[row, column];
        Matrix[row, column] = Matrix[random_row, random_column];
        Matrix[random_row, random_column] = temp;

        if (Matrix[row, column] != null)
        {
            Matrix[row, column].obj.transform.position = GetScreenCoordinatesFromViewport(row, column);
            Matrix[row, column].CurrentRow = row;
            Matrix[row, column].CurrentColumn = column;
        }

        Matrix[random_row, random_column].obj.transform.position = GetScreenCoordinatesFromViewport(random_row, random_column);
        Matrix[random_row, random_column].CurrentRow = random_row;
        Matrix[random_row, random_column].CurrentColumn = random_column;
    }

    private Vector3 GetScreenCoordinatesFromViewport(int row, int column)
    {
        // ����Ʈ�� ���ϴ� 0,0 ���� 1,1 �̹Ƿ� row, column�� ���� ���� ���������
        // ���� ������ ���� column�� ���η� �򸮵��� ����
        Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.225f * row, 1 - 0.235f * column, 0));
        point.z = 0;
        return point;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // hit.collider�� ray�� ���� �ݶ��̴�.
            // ��, �̷��� input ¥���� �̹����� ���� �ݶ��̴� �־����
            if (hit.collider != null)
            {
                string[] parts = hit.collider.gameObject.name.Split('-');
                int rowPart = int.Parse(parts[1]);
                int columnPart = int.Parse(parts[2]);

                int rowFound = -1;
                int columnFound = -1;

                for (int row = 0; row < GameVariables.MaxRows; row++)
                {
                    if (rowFound != -1) break;

                    for (int column = 0; column < GameVariables.MaxColumns; column++)
                    {
                        if (columnFound != -1) break;
                        if (Matrix[row, column] == null) continue;

                        // �����ص� name�� �ȹٲ����Ƿ� Original�� ��
                        if (Matrix[row, column].OriginalRow == rowPart && Matrix[row, column].OriginalColumn == columnPart)
                        {
                            rowFound = row; // Ŭ���� �̹����� ���� ��
                            columnFound = column; // Ŭ���� �̹����� ���� ��
                        }
                    }
                }

                bool pieceFound = false;

                if (rowFound > 0 && Matrix[rowFound - 1, columnFound] == null) // ���� �������
                {
                    pieceFound = true;
                    toAnimateRow = rowFound - 1;
                    toAnimateColumn = columnFound;
                }
                else if (columnFound > 0 && Matrix[rowFound, columnFound - 1] == null) // ���� �������
                {
                    pieceFound = true;
                    toAnimateRow = rowFound;
                    toAnimateColumn = columnFound - 1;
                }
                else if (rowFound < GameVariables.MaxRows - 1 && Matrix[rowFound + 1, columnFound] == null) // ������ �������
                {
                    pieceFound = true;
                    toAnimateRow = rowFound + 1;
                    toAnimateColumn = columnFound;
                }
                else if (columnFound < GameVariables.MaxColumns - 1 && Matrix[rowFound, columnFound + 1] == null) // �Ʒ��� �������
                {
                    pieceFound = true;
                    toAnimateRow = rowFound;
                    toAnimateColumn = columnFound + 1;
                }

                if (pieceFound)
                {
                    screenPositionToAnimate = GetScreenCoordinatesFromViewport(toAnimateRow, toAnimateColumn); // �̵��� ��ġ
                    PieceToAnimate = Matrix[rowFound, columnFound]; // �̵���ų �̹���
                    gameState = GameState.Animating;
                }
            }
        }
    }

    private void AnimateMovement(PuzzlePiece toMove, float time)
    {
        toMove.obj.transform.position = Vector2.MoveTowards(toMove.obj.transform.position,
                                                            screenPositionToAnimate,
                                                            animSpeed * time);
    }

    private void CheckIfAnimationEnded()
    {
        if (Vector2.Distance(PieceToAnimate.obj.transform.position, screenPositionToAnimate) < 0.1f)
        {
            Swap(PieceToAnimate.CurrentRow, PieceToAnimate.CurrentColumn, toAnimateRow, toAnimateColumn);
            gameState = GameState.Playing;

            CheckForVictory();
        }
    }

    private void CheckForVictory()
    {
        // �ϳ��� ������ġ�� ���ư��� �ʾ����� ���Ӿȳ���
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                if (Matrix[row, column] == null) continue;
                if (Matrix[row, column].CurrentRow != Matrix[row, column].OriginalRow ||
                    Matrix[row, column].CurrentColumn != Matrix[row, column].OriginalColumn)
                {
                    return;
                }
            }
        }

        gameState = GameState.End;
    }

    public void SetPuzzleIndex(int puzzleIndex)
    {
        this.puzzleIndex = puzzleIndex;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
