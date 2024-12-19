using UnityEngine;

public class Player : MonoBehaviour
{
    private WeaponParent weaponParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 포인터의 위치를 월드 좌표로 변환
        Vector3 PointerPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z))
        );
        weaponParent.PointerPosition = PointerPosition;
        // 플레이어와 마우스 포인터의 X 좌표 비교
        if (PointerPosition.x < transform.position.x)
        {
            // 마우스가 왼쪽에 있으면 X축 반전
            FlipDirection(-1);
        }
        else if (PointerPosition.x > transform.position.x)
        {
            // 마우스가 오른쪽에 있으면 X축 반전
            FlipDirection(1);
        }

        // Mouse Left Button Down
        if (Input.GetMouseButtonDown(0)) {
            weaponParent.Attack();
        }
    }

    void FlipDirection(int direction)
    {
        Vector3 currentScale = transform.localScale;

        // X축 방향만 변경
        currentScale.x = Mathf.Abs(currentScale.x) * direction;

        transform.localScale = currentScale;

        //Debug.Log($"Player flipped to: {(direction == 1 ? "Right" : "Left")}");
    }
}
