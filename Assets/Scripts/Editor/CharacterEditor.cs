using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterConfig))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(20);

        CharacterConfig config = (CharacterConfig)target;

        GUILayout.BeginVertical();
            PrintProp("Jump Height",config.JumpHeight);
            PrintProp("Sliding Jump Height",config.SlideJumpHeight);
            PrintProp("Max Velocity",config.MaxVelocity);

            float AirTime = Mathf.Sqrt(Mathf.Abs(8*config.JumpHeight/Physics2D.gravity.y));
            GUILayout.Label("Air Time : " + AirTime*1000 + "ms");

            float MaxRange = config.MaxVelocity * AirTime;
            PrintProp("Max Jump Range",MaxRange);
            PrintProp("Max Sliding Jump Range",MaxRange*config.SlideJumpSpeedBoost);
        GUILayout.EndHorizontal();
    }
    void PrintProp(string Label,float Value)
    {
        GUILayout.BeginHorizontal();
            GUILayout.Label(Label);
            GUILayout.Label(Value + "units");
            GUILayout.Label(Value*2 + "blocks");
        GUILayout.EndHorizontal();
    }
}
