using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;//Tile �? hi?n th? b?n sao c?a Piece
    public Board mainBoard;//Tham chi?u �?n Board hi?n t?i
    public Piece trackingPiece;//Piece �ang ��?c theo d?i b?i Ghost

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()//hi?t l?p c�c gi� tr? ban �?u cho Ghost, bao g?m thi?t l?p tham chi?u �?n Tilemap v� kh?i t?o m?ng cells.
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }

    private void LateUpdate()//
    {
        Clear();//�? x�a b?n sao c?a Piece hi?n t?i tr�n m�n h?nh
        Copy();//�? sao ch�p c�c � Tile c?a Piece hi?n t?i v�o m?ng cells c?a Ghost
        Drop();//�? t�nh to�n v? tr� m� b?n sao c?a Piece s? r�i v�o.
        Set();//�? hi?n th? b?n sao c?a Piece t?i v? tr� �? t�nh to�n ��?c tr�?c ��.
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
