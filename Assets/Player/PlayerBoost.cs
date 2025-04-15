using UnityEngine;
using System.Collections;

[System.Obsolete]
public class PlayerBoost : MonoBehaviour
{
    public float boostForce = 10f;
    public float boostDuration = 0.5f;
    private float boostTimer = 0f;
    private bool canBoost = true;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canBoost)
        {
            StartCoroutine(Boost());
        }
    }

    IEnumerator Boost()
    {
        canBoost = false;
        boostTimer = boostDuration;

        while (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            rb.velocity += (Vector2)rb.velocity.normalized * boostForce * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Thời gian chờ nạp lại boost
        canBoost = true;
    }

    public void RechargeBoost()
    {
        canBoost = true;
    }
}
