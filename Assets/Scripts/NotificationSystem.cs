using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem instance { get; private set; }

    [SerializeField] private AudioClip NotificationSound;
    private AudioSource _AudioSource;
    private void Start()
    {
        instance = this;

        _AudioSource = GetComponent<AudioSource>();
    }
    public void Notify()
    {
        _AudioSource.PlayOneShot(NotificationSound);
    }
}
