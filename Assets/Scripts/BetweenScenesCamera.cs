
using UnityEngine;

public class BetweenScenesCamera : MonoBehaviour
{
    [Range(0, 360)][SerializeField] private float angle;
    [SerializeField] private float circleSpeed;
    [SerializeField] private float distance;
    [SerializeField] private float hoverDist;
    [SerializeField] private float hoverSpeed;
    
    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, player.position, Color.magenta);

        angle += Time.deltaTime * circleSpeed;
        angle %= 360;
        
        float targetX = distance * Mathf.Cos(Mathf.Deg2Rad * angle);
        float targetY =  hoverDist * Mathf.Sin(Time.time * hoverSpeed);
        float targetZ = distance * Mathf.Sin(Mathf.Deg2Rad * angle);

        
        Vector3 localPos = new Vector3(targetX, targetY, targetZ);
        
        transform.position = player.TransformPoint(localPos);
        transform.LookAt(player);

    }
}
