using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Cloak : Skill
{
    private float transitionTime = 0.3f;
    private float duration;

    public override void Equipped()
    {
        skillIcon = Resources.Load<Sprite>("cloak");
        skillName = "Cloak";
        duration = 4f;
        blocksMovement = false;
        blocksAttack = false;
        cooldowntimer = 6f;
        skillEnded = true;
    }

    public override IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction)
    {
        skillEnded = true;
        yield return null;
    }

    public override IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        skillEnded = false;
        movementEnded = false;
        skillBar.color = activeBar;
        skillBar.fillAmount = 1;
        Debug.Log(skillBar.color);
        for (float i = 0; i <= transitionTime + 0.01;i += 0.05f)
        {
            playerrb2.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, 1/(transitionTime/0.05f));
            Debug.Log(i);
            yield return new WaitForSeconds(0.05f);
        }
        playerrb2.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.03f);
        playerrb2.GetComponent<Player>().cloaked = true;
        yield return new WaitForSeconds(duration);
        playerrb2.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        playerrb2.GetComponent<Player>().cloaked = false;
        for (float i = 0; i <= transitionTime + 0.01; i += 0.05f)
        {
            playerrb2.GetComponentInChildren<SpriteRenderer>().color += new Color(0, 0, 0, 1 / (transitionTime / 0.05f));
            yield return new WaitForSeconds(0.05f);
        }
        skillBar.color = normal;

        movementEnded = true;
    }
}
