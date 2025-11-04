using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{


    #region InputActions Event
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; // 에셋의 Action Map 이름
    [SerializeField] private string moveActionName = "Move";     // Vector2
    [SerializeField] private string lookActionName = "Look";    //Vector2
    [SerializeField] private string jumpActionName = "Jump";     // Button (옵션)

    InputAction aMove, aJump;

    #endregion

    #region InputValue
    private Vector2 moveAmt;
    private Vector2 lookAmt;
    #endregion

    #region RayInfo
    [SerializeField] private LayerMask groundMask;
    private float rayLength = 0.25f;
    private float edgeOffset = 0.05f;
    private bool debugRay = false;

    private CapsuleCollider capCol;
    #endregion

    private InputActionAsset _runtimeAsset; //런타임 복제본 멀티 
    private InputActionMap _map;
    private InputAction _moveAct, _lookAct, _jumpAct;

    private Camera cam;
    private Rigidbody rigid;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        capCol = GetComponent<CapsuleCollider>();
        inputActions.FindAction(actionMapName);

        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        cam = Camera.main;

        _runtimeAsset = ScriptableObject.Instantiate(inputActions);
        _map = _runtimeAsset.FindActionMap(actionMapName, false);
        _map.Enable();

        aMove = _map.FindAction(moveActionName, false);
        aJump = _map.FindAction(lookActionName, false); //액션 맵핑 해주기 
    }

    private void FixedUpdate()
    {
        if (aMove == null)
        {
            return;
        }


        Vector2 mv = aMove.ReadValue<Vector2>();
        Vector3 f = cam ? Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized : Vector3.forward; //앞뒤
        Vector3 r = cam ? Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized : Vector3.right; // 좌우

        Vector3 dir = (f * mv.y + r * mv.x).normalized;

        //수평이동
        Vector3 v = dir * player.MoveSpeed;
        v.y = rigid.linearVelocity.y;
        rigid.linearVelocity = v;


        if (aJump != null && aJump.WasPressedThisFrame() && IsGrounded())
        {

            var vel = rigid.linearVelocity; vel.y = 0f;
            rigid.linearVelocity = vel;
            rigid.AddForce(Vector3.up * player.JumpForce, ForceMode.VelocityChange); //값을 가져와서 사용하는방식
        }


    }

    private bool IsGrounded()
    {
        if (capCol == null)
        {
            capCol = GetComponent<CapsuleCollider>();
        }

        Vector3 buttomCenter = capCol.center - new Vector3(0f, capCol.bounds.extents.y , 0f) + Vector3.up* 0.01f;

        float r = capCol.radius + edgeOffset;
        Vector3 f = transform.forward * r;
        Vector3 b = -transform.forward * r;
        Vector3 rgt = transform.right * r;
        Vector3 l = -transform.right * r;

        Vector3[] origins = new[] { buttomCenter + f, buttomCenter + b, buttomCenter + l, buttomCenter + rgt };
        foreach (var o in origins)
        {
            bool hit = Physics.Raycast(o, Vector3.down, out _, rayLength, groundMask, QueryTriggerInteraction.Ignore);
            if (debugRay)
            { 
            Debug.DrawRay(o, Vector3.down * rayLength, hit ? Color.green : Color.red, 0.02f);
            }


            if (hit) return true;
        }

        return false;

    
    }

    private void Update()
    {

    }

    private void Jump()
    {
        // rigid.AddForce()
    }



}
