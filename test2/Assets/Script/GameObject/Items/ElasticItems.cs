using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticItems : MonoBehaviour
{
    public float bounceForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //搜索每个接触点
        foreach(ContactPoint2D contact in collision.contacts)
        {
            //如果有一个接触点在物体上方就触发弹跳
            if (contact.point.y > transform.position.y + 0.5 * (transform.localScale.y))
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
                }
                break;
            }
        }

    }
}
