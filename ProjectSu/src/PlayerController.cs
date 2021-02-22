using BEPUphysics.Character;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.PhysicsSystem;
using ProjectSu.src.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src
{
    public class PlayerController : ClassBase
    {
        public PlayerEntity _playerEntity;

        private double MoveSpeed = 0.3f;
        private double sensitivity = 0.1f;

        const double characterHeight = 1;
        const double characterWidth = 0.5f;
        const double jumpSpeed = 10f;

        const double stepHeight = 5f;

        private Vector3d mouseRotationBuffer;

        private Vector2d _lastPos;
        private bool _firstMove = true;

        public double Yaw { get; private set; }
        public double Pitch { get; private set; }

        private bool DevSpeed = false;

        CharacterController _CharacterController;

        private Ambience ambience;

        #region Camera
        public Camera _PlayerCamera;
        #endregion

        public PlayerController(PlayerEntity playerEntity, float cameraHight)
        {
            _playerEntity = playerEntity;

            _PlayerCamera = new Camera(_playerEntity, cameraHight);

            _CharacterController = new CharacterController();

            _CharacterController.Body.Position = new BEPUutilities.Vector3(playerEntity.transform.Position.X, playerEntity.transform.Position.Y, playerEntity.transform.Position.Z);
            //_CharacterController.Body.BecomeKinematic();
            Physics.Add(_CharacterController);

           
        }

        public void UpdateController()
        {
            var moveVector = new Vector2d(0, 0);

            /*if (Physics.RayCast(_playerEntity.transform.Position, _playerEntity.transform.Position + new Vector3d(0, -1.05f, 0), out ClosestRayResultCallback hit))
            {
                
            }*/

            if (Input.GetKeyDown(Keys.F5, 0))
            {
                if (DevSpeed)
                {
                    DevSpeed = false;
                }
                else
                {
                    DevSpeed = true;
                }
            }
            ambience = Ambience.GetEnvironment(_playerEntity.SpaceName);

            if (ambience != null)
            {
                if (_PlayerCamera.position.Y + _playerEntity.transform.Position.Y < -1)
                {
                    ambience.Density = 0.4f;
                    ambience.Distance = 0.5f;
                    ambience.FogColor = Color4.DarkBlue;

                    ambience.SkyColor = Color4.DarkBlue;
                    ambience.SkyHorizonColor = Color4.DarkBlue;
                }
                else
                {
                    ambience.Density = 0.014f;
                    ambience.Distance = 3.5f;
                    ambience.FogColor = Color4.SkyBlue;

                    ambience.SkyColor = Color4.DeepSkyBlue;
                    ambience.SkyHorizonColor = Color4.SkyBlue;
                }
            }

            #region CalculateMouseInput
            if (_firstMove) // this bool variable is initially set to true
            {
                _lastPos = new Vector2d(Input.mouseState.Position.X, Input.mouseState.Position.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = Input.mouseState.Position.X - _lastPos.X;
                var deltaY = Input.mouseState.Position.Y - _lastPos.Y;
                _lastPos = new Vector2d(Input.mouseState.Position.X, Input.mouseState.Position.Y);


                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Yaw -= deltaX * sensitivity / 20f;
                Pitch -= deltaY * sensitivity / 20f; // reversed since y-coordinates range from bottom to top
            }
            #endregion

            if (Engine.MouseCursor.MouseLocked)
            {
                mouseRotationBuffer.X = Yaw;
                mouseRotationBuffer.Y = Pitch;

                if (Pitch < MathHelper.DegreesToRadians(-75.0f))
                {
                    Pitch = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.DegreesToRadians(-75.0f));
                }

                if (Pitch > MathHelper.DegreesToRadians(75.0f))
                {
                    Pitch = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.DegreesToRadians(75.0f));
                }

                _PlayerCamera.rotation = new Quaternion(-MathHelper.Clamp((float)mouseRotationBuffer.Y, MathHelper.DegreesToRadians(-75.0f), MathHelper.DegreesToRadians(75.0f)), 0, 0, 0);
                _playerEntity.transform.Rotation = new Quaterniond(0, WrapAngle(mouseRotationBuffer.X), 0, 0);

                #region PlayerMove

                if (Input.GetKey(Keys.W))
                {
                    moveVector += new Vector2d(0, 1) * Time._DeltaTime;
                }
                if (Input.GetKey(Keys.S))
                {
                    moveVector += new Vector2d(0, -1) * Time._DeltaTime;
                }
                if (Input.GetKey(Keys.A))
                {
                    moveVector += new Vector2d(-1, 0) * Time._DeltaTime;
                }
                if (Input.GetKey(Keys.D))
                {
                    moveVector += new Vector2d(1, 0) * Time._DeltaTime;
                }

                if (Input.GetKey(Keys.LeftShift))
                {
                    MoveSpeed = 5f;
                }
                else
                {
                    MoveSpeed = 3f;
                }
                if (DevSpeed)
                {
                    _CharacterController.StandingSpeed = MoveSpeed * 15;
                }
                else
                {
                    _CharacterController.StandingSpeed = MoveSpeed * 2;
                }

                _CharacterController.AirSpeed = _CharacterController.StandingSpeed;

                if (Input.GetKey(Keys.Z))
                {
                    _CharacterController.StanceManager.DesiredStance = Stance.Prone;
                }
                else if (Input.GetKey(Keys.LeftControl))
                {
                    _CharacterController.StanceManager.DesiredStance = Stance.Crouching;
                }
                else
                {
                    _CharacterController.StanceManager.DesiredStance = Stance.Standing;
                }

                if (Input.GetKeyDown(Keys.Space, 0))
                {
                    _CharacterController.Jump();
                }

                MovePlayer(moveVector);
                #endregion

            }

            //_playerEntity.transform.Position = kinematicCharacter.GhostObject.WorldTransform.ExtractTranslation();
            _playerEntity.transform.Position = new Vector3d(_CharacterController.Body.Position.X, _CharacterController.Body.Position.Y, _CharacterController.Body.Position.Z);

            //Game.GetWorld.PlayerPos = _playerEntity.transform.Position;

            _PlayerCamera.Tick();//Update the camera
        }

        protected override void OnDispose()
        {
            Physics.Remove(_CharacterController);

            _PlayerCamera.Dispose();
            _PlayerCamera = null;

            _playerEntity = null;

            _CharacterController = null;

            ambience = null;

            base.OnDispose();
        }

        public static double WrapAngle(double angle)
        {
            if ((angle > -MathHelper.Pi) && (angle <= MathHelper.Pi))
                return angle;
            angle %= MathHelper.TwoPi;
            if (angle <= -MathHelper.Pi)
                return angle + MathHelper.TwoPi;
            if (angle > MathHelper.Pi)
                return angle - MathHelper.TwoPi;
            return angle;
        }

        private void MovePlayer(Vector2d moveVector)
        {
            /*var camRotation = Matrix3.CreateRotationX(_playerEntity.transform.Rotation.X) * Matrix3.CreateRotationY(_playerEntity.transform.Rotation.Y) * Matrix3.CreateRotationZ(_playerEntity.transform.Rotation.Z);
            var rotatedVector = Vector3d.Transform(Vector3d.Zero, camRotation);
            
            kinematicCharacter.SetWalkDirection(rotatedVector * MoveSpeed);*/

            _CharacterController.ViewDirection = Forward(_playerEntity.transform.RotationMatrix);
            _CharacterController.HorizontalMotionConstraint.MovementDirection = BEPUutilities.Vector2.Normalize(new BEPUutilities.Vector2(moveVector.X, moveVector.Y));
        }

        public BEPUutilities.Vector3 Forward(Matrix4 matrix)
        {
            BEPUutilities.Vector3 vector = new BEPUutilities.Vector3();

            vector.X = matrix.M31;
            vector.Y = matrix.M32;
            vector.Z = matrix.M33;
            return vector;
        }
    }
}
