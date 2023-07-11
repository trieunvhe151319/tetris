using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piece : MonoBehaviour
{

    public Board board { get; private set; } //B?ng tr? ch�i
    public TetrominoData data { get; private set; } //D? li?u tetromino
    public Vector3Int[] cells { get; private set; } // m?t m?ng c�c t?a �? (Vector3Int) c?a c�c � vu�ng
    public Vector3Int position { get; private set; } //V? tr� hi?n t?i c?a piece tr�n board
    public int rotationIndex { get; private set; } //h? s? c?a g�c quay hi?n t?i c?a piece

    public static float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;


    public static void tru()
    {
        stepDelay -= 0.5f;
    }


    public void Initialize(Board board, Vector3Int position, TetrominoData data) //Kh?i t?o 1 Tetromino m?i
    {
        this.data = data;
        this.board = board;
        this.position = position;

        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update() //Tr?ng th�i c?a piece
    {
        
        board.Clear(this);


        // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        lockTime += Time.deltaTime;

        // Handle rotation
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        // Handle hard drop
        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        // Allow the player to hold movement keys but only after a move delay
        // so it does not move too fast
        if (Time.time > moveTime)
        {
            HandleMoveInputs();
        }

        // Advance the piece to the next row every x seconds
        if (Time.time > stepTime)
        {
            Step();
        }

        board.Set(this);

    }
    //
    private void HandleMoveInputs() //X? l? di chuy?n
    {
        // Soft drop movement
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (Move(Vector2Int.down))
            {
                // Update the step time to prevent double movement
                stepTime = Time.time + stepDelay;
            }
        }

        // Left/right movement
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
    }

    private void Step() //Di chuy?n piece xu?ng 1 h�ng
    {
        stepTime = Time.time + stepDelay;

        // Step down to the next row
        Move(Vector2Int.down);

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void HardDrop() //Di chuy?n piece xu?ng h�ng cu?i c�ng 
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

    private void Lock() //Lock piece v�o board, x�a h�ng �? �?y
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }

    private bool Move(Vector2Int translation) //Di chuy?n piece theo h�?ng ch? �?nh
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f; // reset
        }

        return valid;
    }

    private void Rotate(int direction) // Xoay theo h�?ng ch? �?nh
    {
        // Store the current rotation in case the rotation fails
        // and we need to revert
        int originalRotation = rotationIndex;

        // Rotate all of the cells using a rotation matrix
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        // Revert the rotation if the wall kick tests fail
        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction) //Xoay t?t c? c�c kh?i vu�ng c?a Tetromino s? d?ng ma tr?n quay
    {
        float[] matrix = Data.RotationMatrix;

        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)//ki?m tra xem Tetromino c� th? ��?c xoay v?i v? tr� quay hi?n t?i hay kh�ng?
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)//T�nh to�n v? tr� "wall kick" c?a m?t kh?i g?ch sau khi n� quay m?t s? l�?t nh?t �?nh
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) //H�?ng quay
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
   

}