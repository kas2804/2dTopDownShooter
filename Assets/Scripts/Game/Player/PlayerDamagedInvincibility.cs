using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField]private float _invincibilityDuration;

    private InvincibiltyController _invincibilityController;

    private void Awake()
    {
        _invincibilityController = GetComponent<InvincibiltyController>();    
    }

    public void StartInvincibility()
    {
        _invincibilityController.StartInvincibility(_invincibilityDuration);
    }
}
