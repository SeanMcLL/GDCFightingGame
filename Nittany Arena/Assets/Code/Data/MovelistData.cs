using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Data", menuName = "Fighting Game/Movelist Data")]

public class MovelistData : ScriptableObject
{
    [Header("Regular Attacks")]
    //Jab
    public AttackData NeutralRegular;
    //Forward Tilt
    public AttackData ForwardRegular;
    //Up Tilt
    public AttackData UpRegular;
    //Down Tilt
    public AttackData DownRegular;

    [Header("Aerial Attacks")]
    //Neutral Air
    public AttackData NeutralAir;
    //Forward Air
    public AttackData ForwardAir;
    //Back Air
    public AttackData BackAir;
    //Up Air
    public AttackData UpAir;
    //Down Air
    public AttackData DownAir;

    [Header("Special Attacks")]
    //Neutral B
    public AttackData NeutralSpecial;
    //Forward B
    public AttackData ForwardSpecial;
    //Up B
    public AttackData UpSpecial;
    //Down B
    public AttackData DownSpecial;
}
