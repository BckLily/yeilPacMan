using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : ItemBase
{

    protected override void GetItem()
    {
        // ���� ���� �� �ְ� �ٲ��
        // ���� ������ ���� �ϰ�
        // ��Ÿ ����� �� �� �ְ� ������ �Ѵ�.
        GameManager.Instance.OverPowerPlayer();

    }


}
