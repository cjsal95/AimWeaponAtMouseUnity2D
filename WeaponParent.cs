using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] public SpriteRenderer characterRenderer, weaponRendere;
    public Vector2 PointerPosition { get; set; }

    [SerializeField] public Animator animator;
    [SerializeField] public float delay = 0.2f;

    private bool attackBlocked;

    public bool IsAttacking {get; private set;}

    [SerializeField] public Transform circleOrigin;
    [SerializeField] public float radius;
    [SerializeField] public Transform player;

    public void ResetIsAttacking() {
        IsAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttacking) return;

        Vector2 direction = (PointerPosition-(Vector2)transform.position).normalized;
        transform.right = direction;
        Vector2 scale = transform.localScale;

        if(direction.x < 0 ){
            scale.y = -1;
            scale.x = -1;
        }
        else if (direction.x > 0) {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
            weaponRendere.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else {
            weaponRendere.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
    }

    public void Attack() {
        if(attackBlocked) return;

        //animator.SetTrigger("Attack");
        // 공격 방향 설정
        float attackAngle = -120f;


    // WeaponParent의 현재 회전 각도 가져오기
        float currentAngle = NormalizeAngle(transform.eulerAngles.z);

        // 플레이어의 방향(Scale X) 확인
        float playerDirection = player.localScale.x > 0 ? 1f : -1f;

        // 공격 각도 적용
        float newAngle = currentAngle + attackAngle * playerDirection;

        // 정규화된 각도 계산 (-180 ~ 180)
        newAngle = NormalizeAngle(newAngle);

        // WeaponParent의 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, newAngle);


        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        IsAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius)) {
            Debug.Log(collider.name);
        }
    }
}
