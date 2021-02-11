using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerClass
{
    public enum PlayerClassEnum
    {
        Rogue,
        Mage,
        Warrior
    }

    public static Sprite getClassTexture(PlayerClassEnum cl) {
        switch (cl)
        {
            case PlayerClassEnum.Rogue:
                return Resources.Load<Sprite>("rogue_sprite");
            case PlayerClassEnum.Mage:
                return Resources.Load<Sprite>("mage_sprite");
            case PlayerClassEnum.Warrior:
                return Resources.Load<Sprite>("archer_sprite");
            default:
                break;
        }
        return null;
    }
    public static RuntimeAnimatorController getClassAnimator(PlayerClassEnum cl)
    {
        switch (cl)
        {
            case PlayerClassEnum.Rogue:
                var r1= Resources.Load<RuntimeAnimatorController>("Rogue_anim");
                return r1;
            case PlayerClassEnum.Mage:
                var r2 = Resources.Load<RuntimeAnimatorController>("Mage_anim");
                return r2;
            case PlayerClassEnum.Warrior:
                var r3 = Resources.Load<RuntimeAnimatorController>("Archer_anim");
                return r3;
            default:
                break;
        }
        return null;
    }

    public static Type getClassSkill(PlayerClassEnum cl)
    {
        switch (cl)
        {
            case PlayerClassEnum.Rogue:
                return typeof(Cloak);
            case PlayerClassEnum.Mage:
                return typeof(Sleep);
            case PlayerClassEnum.Warrior:
                return typeof(Dash);
            default:
                break;
        }
         return null; 
    }

    public static string getClassSkillDescription(PlayerClassEnum cl)
    {
        switch (cl)
        {
            case PlayerClassEnum.Rogue:
                return "Originally wielded by all kinds of thiefs, nowadays can be used by people who want to hide their presence from enemies or to stay invisible in both meanings.";
            case PlayerClassEnum.Mage:
                return "Magical sleep that can pacify wild beasts apart from ones with the strongest mind and will.";
            case PlayerClassEnum.Warrior:
                return "Good old trick which can be used to attack or flee.";
            default:
                break;
        }
        return null;
    }

    public static string getClassDescription(PlayerClassEnum cl)
    {
        switch (cl)
        {
            case PlayerClassEnum.Rogue:
                return "If you ask people who is this man, no one will tell you for sure, but it's well known that if you see him in the crowd, you better watch your wallet.";
            case PlayerClassEnum.Mage:
                return "Powerful yet wise, every mage apprentice wished to become more like him. Rumors say, he what lies beyond the Dwarven Halls.";
            case PlayerClassEnum.Warrior:
                return "Everyone in the countryside knew this boy as a local jester, but he changed drastically after he returned from king's army service.";
            default:
                break;
        }
        return null;
    }
}
