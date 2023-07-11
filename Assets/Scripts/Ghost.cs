using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;//Tile ð? hi?n th? b?n sao c?a Piece
    public Board mainBoard;//Tham chi?u ð?n Board hi?n t?i
    public Piece trackingPiece;//Piece ðang ðý?c theo d?i b?i Ghost

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()//hi?t l?p các giá tr? ban ð?u cho Ghost, bao g?m thi?t l?p tham chi?u ð?n Tilemap và kh?i t?o m?ng cells.
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }

    private void LateUpdate()//
    {
        Clear();//ð? xóa b?n sao c?a Piece hi?n t?i trên màn h?nh
        Copy();//ð? sao chép các ô Tile c?a Piece hi?n t?i vào m?ng cells c?a Ghost
        Drop();//ð? tính toán v? trí mà b?n sao c?a Piece s? rõi vào.
        Set();//ð? hi?n th? b?n sao c?a Piece t?i v? trí ð? tính toán ðý?c trý?c ðó.
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = trackingPiece.cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int position = trackingPiece.position;

        int current = position.y;
        int bottom = -mainBoard.boardSize.y / 2 - 1;

        mainBoard.Clear(trackingPiece);

        for (int row = current; row >= bottom; row--)
        {
            position.y = row;

            if (mainBoard.IsValidPosition(trackingPiece, position)) {
                this.position = position;
            } else {
                break;
            }
        }

        mainBoard.Set(trackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, tile);
        }
    }

}
