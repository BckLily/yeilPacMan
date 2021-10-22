using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            GetItem();
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.itemList.Remove(this);

        // 아이템 다 먹었나 확인
        if (GameManager.Instance.remainItem == 0)
        {
            GameManager.Instance.GameClear();
        }
    }

    protected abstract void GetItem();


}
