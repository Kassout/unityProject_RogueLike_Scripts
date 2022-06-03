using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>MerchantController</c> is a Unity script representing the general behavior of a Merchant NPC game object.
/// </summary>
public class MerchantController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>collisionBoxOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision box of the buy item game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionBoxSize</c> is a Unity <c>Vector2</c> representing the size vector of the collision box of the buy item game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxSize;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the buy item game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the Merchant NPC game object animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance field <c>shopItems</c> is a list of Unity <c>GameObject</c> instances representing the different items the merchant NPC can sell.
    /// </summary>
    [SerializeField]
    private List<GameObject> _shopItems;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
        if (hit && hit.CompareTag("Player"))
        {
            _animator.SetBool("isAwake", true);

            if (_shopItems.Count >= 1)
            {
                foreach (GameObject shopItem in _shopItems)
                {
                    if (shopItem == null)
                    {
                        _shopItems.Remove(shopItem);
                        break;
                    }

                    IShopItem item = (IShopItem)shopItem.GetComponent(typeof(IShopItem));

                    if (item.isActive())
                    {
                        _animator.SetBool("isSpeaking", true);
                        break;
                    }

                    _animator.SetBool("isSpeaking", false);
                }
            }
            else
            {
                _animator.SetBool("isSpeaking", false);
            }
        }
        else
        {
            _animator.SetBool("isAwake", false);
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize);
    }
    
    #endregion
}