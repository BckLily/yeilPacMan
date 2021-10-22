using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : ItemBase
{

    protected override void GetItem()
    {
        // 적을 죽일 수 있게 바뀌고
        // 적이 도망을 가게 하고
        // 기타 등등을 할 수 있게 만들어야 한다.
        GameManager.Instance.OverPowerPlayer();

    }


}
