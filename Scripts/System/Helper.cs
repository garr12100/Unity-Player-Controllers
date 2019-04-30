using UnityEngine;
using System.Collections;

namespace Helper
{
    #region Reference Cache

    public class PlayerInput
    {
        public static string Horizontal = "Horizontal";
        public static string Vertical = "Vertical";
        public static string Jump = "Jump"; 
        public static string LookX = "LookX";
        public static string LookY = "LookY";
        public static string Attack1 = "Attack1";
        public static string Attack2 = "Attack2";
        public static string Target = "Target";
        public static string Sprint = "Sprint";


    }

    public class GameTag
    {
        //System Tags
        public static string Untagged = "Untagged";
        public static string Respawn = "Resawn";
        public static string Finish = "Finish";
        public static string EditorOnly = "EditorOnly";
        public static string MainCamera = "MainCamera";
        public static string Player = "Player";
        public static string GameController = "GameController";

        public static string PlayerCamera = "PlayerCamera";
        public static string Enemy = "Enemy";
    }

    public class Resource
    {
        public static string AnimatorControllerPlayer = "System/PlayerAnimator";
    }

    public static class AnimatorConditionsPlayer
    {
        public static string speed = "Speed";
        public static string AbsSpeed = "AbsSpeed";

        public static string direction = "Direction";
        public static string AbsDirection = "AbsDirection";

        public static string grounded = "Grounded";
        public static string airSpeed = "AirSpeed";

        public static string Running = "Running";

        public static string Attack1 = "Attack1";
        public static string Attack2 = "Attack2";
        public static string Hit = "Hit";
        public static string Dead = "Dead";



    }

    public static class GameLayers
    {
        public static int Player = 8;
    }


    #endregion

    #region FSM Enumerations

    public enum CameraState
    {
        Normal,
        Target,
        Sprint
    }

    public enum SpeedState
    {
        Walk,
        Sprint, 
        Target,
        Crouch
    }

    #endregion


    #region Object Structions

    public struct CameraTargetObject
    {
        private Vector3 position;
        private Transform xForm;

        public Vector3 Position
        {
            get { return Position; }
            set { position = value; } 
        } 

        public Transform XForm
        {
            get { return xForm; }
            set { xForm = value; }
        }

        public void Init(string camName, Vector3 pos, Transform transform, Transform parent)
        {
            position = pos;
            xForm = transform;
            xForm.name = camName;
            xForm.parent = parent;
            xForm.localPosition = Vector3.zero;
            xForm.localPosition = position;
        }
    }

    public struct CameraMountPoint
    {
        private Vector3 position;
        private Transform xForm;

        public Vector3 Position
        {
            get { return Position; }
            set { position = value; }
        }

        public Transform XForm
        {
            get { return xForm; }
            set { xForm = value; }
        }

        public void Init(string camName, Vector3 pos, Transform transform, Transform parent)
        {
            position = pos;
            xForm = transform;
            xForm.name = camName;
            xForm.parent = parent;
            xForm.localPosition = Vector3.zero;
            xForm.localPosition = position;
        }
    }

    #endregion


}
