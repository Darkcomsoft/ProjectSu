using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using BEPUphysics;
using System;
using System.Collections.Generic;
using System.Text;
using BEPUphysics.Character;
using ProjectIND.darkcomsoft.src.engine.physics;
using OpenTK.Mathematics;

namespace ProjectIND.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {
        private double v_moveSpeed = 0.3f;
        private double v_sensitivity = 0.1f;

        private double Yaw { get; set; }
        private double Pitch { get; set; }

        private const double v_characterHeight = 1;
        private const double v_characterWidth = 0.5f;
        private const double v_jumpSpeed = 10f;
        private const double v_stepHeight = 5f;

        private bool v_playerControllerReady = false;

        private CharacterController v_charController;
        private Vector2d v_lastPos;
        private Vector2 v_moveVector = new Vector2(0,0);
        private Vector3d v_mouseRotationBuffer;
        private Camera v_playerCamera;

        protected override void OnStart()
        {
            SetUpController();
            v_playerControllerReady = true;
            base.OnStart();
        }

        protected override void OnDeath()
        {
            v_playerControllerReady = false;
            
            Game.KillPlayer(this, true);
            base.OnDeath();
        }

        protected override void OnDispose()
        {
            Physics.Remove(v_charController);
            v_charController = null;

            v_playerCamera.Dispose();
            v_playerCamera = null;
            base.OnDispose();
        }

        protected override void OnTick()
        {
            if (v_playerControllerReady)
            {
                TickController();
            }
            v_playerCamera?.Tick();
            base.OnTick();
        }

        #region CharController
        private void TickController()
        {
            v_moveVector.X = 0;
            v_moveVector.Y = 0;

            #region CalculateMouseInput
            // Calculate the offset of the mouse position
            var deltaX = Input.getMouse.Position.X - v_lastPos.X;
            var deltaY = Input.getMouse.Position.Y - v_lastPos.Y;
            v_lastPos = new Vector2d(Input.getMouse.Position.X, Input.getMouse.Position.Y);


            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            Yaw -= deltaX * v_sensitivity / 20f;
            Pitch -= deltaY * v_sensitivity / 20f; // reversed since y-coordinates range from bottom to top
            #endregion

            #region CameraRotation
            v_mouseRotationBuffer.X = Yaw;
            v_mouseRotationBuffer.Y = Pitch;

            if (Pitch < MathHelper.DegreesToRadians(-75.0f))
            {
                Pitch = v_mouseRotationBuffer.Y - (v_mouseRotationBuffer.Y - MathHelper.DegreesToRadians(-75.0f));
            }

            if (Pitch > MathHelper.DegreesToRadians(75.0f))
            {
                Pitch = v_mouseRotationBuffer.Y - (v_mouseRotationBuffer.Y - MathHelper.DegreesToRadians(75.0f));
            }

            v_playerCamera.rotation = new Quaternion(-MathHelper.Clamp((float)v_mouseRotationBuffer.Y, MathHelper.DegreesToRadians(-75.0f), MathHelper.DegreesToRadians(75.0f)), 0, 0, 0);
            transform.v_Rotation = new Quaterniond(0, WrapAngle(v_mouseRotationBuffer.X), 0, 0);
            #endregion

            #region Movement
            if (Input.GetKey(GameSettings.MOVEFRONT_KEY))
            {
                v_moveVector.X += 0;
                v_moveVector.Y += 1 * (float)Time._DeltaTime;
            }
            if (Input.GetKey(GameSettings.MOVEBACK_KEY))
            {
                v_moveVector.X += 0;
                v_moveVector.Y += -1 * (float)Time._DeltaTime;
            }
            if (Input.GetKey(GameSettings.MOVELEFT_KEY))
            {
                v_moveVector.X += -1 * (float)Time._DeltaTime;
                v_moveVector.Y += 0;
            }
            if (Input.GetKey(GameSettings.MOVERIGHT_KEY))
            {
                v_moveVector.X += 1 * (float)Time._DeltaTime;
                v_moveVector.Y += 0;
            }

            transform.v_Position = new Vector3d(v_charController.Body.Position.X, v_charController.Body.Position.Y, v_charController.Body.Position.Z);

            transform.v_Position.X = v_charController.Body.Position.X;
            transform.v_Position.Y = v_charController.Body.Position.Y;
            transform.v_Position.Z = v_charController.Body.Position.Z;
            #endregion
        }

        private void SetUpController()
        {
            v_playerCamera = new Camera(transform, (float)v_characterHeight);

            v_charController = new CharacterController();
            v_charController.Body.Position = new BEPUutilities.Vector3((float)transform.v_Position.X, (float)transform.v_Position.Y, (float)transform.v_Position.Z);
            v_charController.Body.BecomeKinematic();
            Physics.Add(v_charController);
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

            //v_charController.ViewDirection = Forward(transform.RotationMatrix);
            v_charController.ViewDirection = new BEPUutilities.Vector3((float)transform.Forward.X, (float)transform.Forward.Y, (float)transform.Forward.Z);
            v_charController.HorizontalMotionConstraint.MovementDirection = BEPUutilities.Vector2.Normalize(new BEPUutilities.Vector2((float)moveVector.X, (float)moveVector.Y));
        }

        public BEPUutilities.Vector3 Forward(Matrix4 matrix)
        {
            BEPUutilities.Vector3 vector = new BEPUutilities.Vector3();

            vector.X = matrix.M31;
            vector.Y = matrix.M32;
            vector.Z = matrix.M33;
            return vector;
        }
        #endregion
    }
}
