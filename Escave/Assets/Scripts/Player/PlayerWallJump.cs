using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    [Header("Checks")]
    [SerializeField] private Transform _wallCheckRight;
    [SerializeField] private Transform _wallCheckLeft;
    private float _checkRadius = 0.2f;

    [Header("OnJump")]
    [SerializeField] private float _wallJumpForce;
    
    [Header("OnWall")]
    [SerializeField] private float _wallStayTime;
    [SerializeField] private float _wallFallSpeed;
    
    [Space(10)]
    [SerializeField] private LayerMask _wallLayer;

    public bool IsWallRight()
    {
        return Physics2D.OverlapCircle(_wallCheckRight.position, _checkRadius, _wallLayer);
    }

    public bool IsWallLeft()
    {
        return Physics2D.OverlapCircle(_wallCheckLeft.position, _checkRadius, _wallLayer);
    }
}
