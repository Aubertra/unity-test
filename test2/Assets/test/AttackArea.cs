using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public int damage = 1;
    public string targetTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag)&&targetTag == "Enemy")
        {
            BoarAI boar = other.GetComponent<BoarAI>();
            if (boar != null)
            {
                boar.TakeDamage(damage);
                Debug.Log("命中" + targetTag + "，造成伤害：" + damage);
            }
        }

        if (other.CompareTag(targetTag) && targetTag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("命中" + targetTag + "，造成伤害：" + damage);
            }
        }
    }
}
