using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public int scoreOneLine = 20;
    public int scoreTwoLine = 50;
    public int scoreThreeLine = 120;
    public int scoreFourLine = 200;

    private int NumberRowsThisTurn = 0;
    public static int currentScore = 0;
    public int numberLineCleared = 0;
    public Text hud_score;
    public Text hud_highscore;
    
    public int score = 0;
    public int HighScore;
    //public Text hud_score2;
    //public Text hud_highscore2;

    //public int score2 = 0;
    //public int HighScore2;

    public int count = 0;

    public Tilemap tilemap { get; private set; }//s? d?ng �? hi?n th? c�c � vu�ng tr�n b?ng ch�i
    public Piece activePiece { get; private set; }//�?i di?n cho tetromino (m?t kh?i g?ch trong tr? ch�i) �ang ��?c �i?u khi?n b?i ng�?i ch�i

    public TetrominoData[] tetrominoes;//ch?a d? li?u cho t?t c? c�c tetrominoes c� th? xu?t hi?n trong tr? ch�i
    public Vector2Int boardSize = new Vector2Int(10, 20);//k�ch th�?c c?a b?ng ch�i
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);//v? tr� xu?t hi?n ban �?u c?a tetromino m?i
    //Score - A Quyen
    public void updateScore()
    {
        if (numberLineCleared > 0)
        {
            switch (numberLineCleared)
            {
                case 1:
                    score += scoreOneLine; break;
                case 2:
                    score += scoreTwoLine; break;
                case 3:
                    score += scoreThreeLine; break;
                case 4:
                    score += scoreFourLine; break;
                default: break;
            }
        }
        numberLineCleared = 0;
    }
    //public void updateScore2()
    //{
    //    if (numberLineCleared > 0)
    //    {
    //        switch (numberLineCleared)
    //        {
    //            case 1:
    //                score2 += scoreOneLine; break;
    //            case 2:
    //                score2 += scoreTwoLine; break;
    //            case 3:
    //                score2 += scoreThreeLine; break;
    //            case 4:
    //                score2 += scoreFourLine; break;
    //            default: break;
    //        }
    //    }
    //    numberLineCleared = 0;


    //}

    void UpdateUI()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.Save();
        if (HighScore < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        hud_score.text = score.ToString();
        hud_highscore.text = HighScore.ToString();      
    }
    //void UpdateUI2()
    //{
    //    //HighScore2 = PlayerPrefs.GetInt("HighScore", 0);
    //    PlayerPrefs.Save();
    //    if (HighScore2 < score2)
    //    {
    //        PlayerPrefs.SetInt("HighScore", score2);
    //    }
    //    hud_score2.text = score2.ToString();
    //    hud_highscore2.text = HighScore2.ToString();
    //}
    //Score A Quyen
    public RectInt Bounds//T�nh to�n gi?i h?n c?a b?ng ch�i
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()//kh?i t?o Tilemap v� TetrominoData
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }
    //s
    private void Start()
    {
        SpawnPiece();
    }
    void Update()
    {
        updateScore();
        UpdateUI();
        //updateScore2();
        //UpdateUI2();
    }

    public void SpawnPiece()//�? t?o m?t tetromino m?i v� �?t n� v�o v? tr� b?t �?u
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        SceneManager.LoadScene("Ketthuc");
        // Do anything else you want on game over here..
    }

    public void Set(Piece piece)//�? �?t m?t tetromino tr�n b?ng ch�i b?ng c�ch s? d?ng Tilemap
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)//X�a m?t tetromino kh?i b?ng ch�i b?ng c�ch s? d?ng Tilemap
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)//�? ki?m tra xem m?t v? tr� b?t k? c� h?p l? �? �?t m?t tetromino m?i hay kh�ng
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row))
            {
                count++;
                if (count % 3 == 0)
                {
                    Piece.tru();
                }
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)//ki?m tra xem m?t h�ng �? �?y ch�a?
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }
        numberLineCleared++;
        return true;
    }

    public void LineClear(int row)//X�a h�ng
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

}