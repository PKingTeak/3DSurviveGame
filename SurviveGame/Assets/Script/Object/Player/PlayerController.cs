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
    private float edgeOffset = 0.05f; //센터 주변 링 좀더 세세한 판정을 위해서 
    private float centerRayExtra = 0.3f; //발 아래 
    private float centerRingRadius = 0.1f; //링반지름
    private bool debugRay = true;

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
        aJump = _map.FindAction(jumpActionName, false); //액션 맵핑 해주기 
    }

    private void FixedUpdate()
    {
        if (aMove == null || player == null || rigid == null)
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


        if (aJump != null && aJump.WasPressedThisFrame() && IsGrounded(out var origins))
        {

            var vel = rigid.linearVelocity; 
            vel.y = 0f;
            rigid.linearVelocity = vel;
            rigid.AddForce(Vector3.up * player.JumpForce,ForceMode.VelocityChange); //값을 가져와서 사용하는방식
        }


    }

    private bool IsGrounded(out Vector3[] origins)
    {
        if (capCol == null)
        {
            capCol = GetComponent<CapsuleCollider>();
        }

        // (변경 1) 월드 좌표 기준 '발바닥 중앙'
        Vector3 buttomCenter = new Vector3(
            capCol.bounds.center.x,
            capCol.bounds.min.y + 0.01f, //살짝 띄우기
            capCol.bounds.center.z
        );

        float r = capCol.radius + edgeOffset;
                
        origins = new[] 
        {
           buttomCenter + transform.forward *r,
           buttomCenter - transform.forward *r,
           buttomCenter + transform.right *r,
           buttomCenter - transform.right *r,
        };

        bool any = false;

        for (int i = 0; i < origins.Length; i++)
        {
            bool hit = Physics.Raycast(origins[i], Vector3.down, out _, rayLength, groundMask, QueryTriggerInteraction.Ignore);
            Debug.DrawRay(origins[i], Vector3.down * rayLength , hit ? Color.green : Color.red , 0.1f);
            if (hit)
            {
                any = true;
            }

        }
        return any;
    }


    private void OnDrawGizmos()
    {
        if (!debugRay) return;

        Vector3 worldCenter = transform.TransformPoint(capCol.center);
        float halfHeight = capCol.height * 0.5f;
        float legLength = Mathf.Max(0f, halfHeight - capCol.radius);


        float length = legLength + centerRayExtra;
        bool hitCenter = Physics.Raycast(worldCenter, Vector3.down, out _, length, groundMask, QueryTriggerInteraction.Ignore);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(worldCenter, worldCenter + Vector3.down * length);
        Gizmos.DrawSphere(worldCenter + Vector3.down * length, 0.02f);

        
    }

}


    