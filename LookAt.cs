using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MalbersAnimations;
using MalbersAnimations.Utilities;

[Serializable]
    public class BoneRotation
    {
        public Transform bone;                                         
        public Vector3 offset = new Vector3(0, -90, -90);              
        [Range(0, 1)]
        public float weight = 1;                                        
        internal Quaternion initialRotation;
    }

    

    public class LookAt : MonoBehaviour, IAnimatorListener      
    {
        public bool active = true;                      

        public bool UseCamera;                          
        public Transform Target;                        

        [Space]
        public float LimitAngle = 80f;                  
        public float Smoothness = 5f;                   
        public Vector3 UpVector = Vector3.up;


        float currentSmoothness;

        [Space]
        public BoneRotation[] Bones;                    

        private Transform cam;                         
        protected float angle;                            
        protected Vector3 direction;
        public bool debug = true;
        bool hasTarget;

        private RaycastHit aimHit;

        public Vector3 Direction
        {
            set { direction = value; }
            get { return direction; }
        }


        public bool IsAiming
        {
            get { return angle < LimitAngle && active && hasTarget; }
        }


        public RaycastHit AimHit
        {
            get { return aimHit; }
            set { aimHit = value; }
        }

        bool AnimatorOnAnimatePhysics;

        void Awake()
        {
            if (Camera.main != null) cam = Camera.main.transform;               

            var Anim = GetComponent<Animator>();

            AnimatorOnAnimatePhysics = (Anim && Anim.updateMode == AnimatorUpdateMode.AnimatePhysics); 

            if (AnimatorOnAnimatePhysics) return;

            foreach (var bone in Bones)                                        
            {
                bone.initialRotation = bone.bone.transform.localRotation;
            }
        }


        //private void Update()
        //{
        //    if (AnimatorOnAnimatePhysics) return;
        //    foreach (var bone in Bones)                                                 //Stores the Initial Rotation (For NON BONE LOOKAT)
        //    {
        //        bone.bone.transform.localRotation = bone.initialRotation;
        //    }
        //}


        void Update()
        {
            if (AnimatorOnAnimatePhysics) return;

            foreach (var bone in Bones)                                        
            {
            if (bone!=null)
                bone.initialRotation = bone.bone.transform.localRotation;
            }
        }

        void LateUpdate()
        {
            if (!AnimatorOnAnimatePhysics)
            {
                LookAtBoneSet();           
            }
            else
            {
                LookAtBoneSet_AnimatePhysics();
            }
        }

        public void EnableLookAt(bool value)
        {
            active = value;
        }

        void LookAtBoneSet()
        {
            hasTarget = false;
            if (UseCamera || Target) hasTarget = true;


            angle = Vector3.Angle(transform.forward, direction);                                                    //Find the angle for the current bone


            currentSmoothness = Mathf.Lerp(currentSmoothness, IsAiming ? 1 : 0, Time.deltaTime * Smoothness);

            if (currentSmoothness > 0.999f) currentSmoothness = 1;
            if (currentSmoothness < 0.001f) currentSmoothness = 0;

            


            foreach (var bone in Bones)
            {
                if (!bone.bone) continue;

                Vector3 dir = transform.forward;


                if (UseCamera)
                {
                    dir = cam.forward;

                    aimHit = MalbersTools.RayCastHitToCenter(bone.bone);

                    if (aimHit.collider)
                    {
                        dir = MalbersTools.DirectionTarget(bone.bone.position, aimHit.point);
                    }
                }

                if (Target) dir = MalbersTools.DirectionTarget(bone.bone, Target);

                direction = Vector3.Lerp(direction, dir, Time.deltaTime * Smoothness);

                if (currentSmoothness == 0) return;                                         


                if (debug) Debug.DrawRay(bone.bone.position, direction * 15, Color.green);


                var final = Quaternion.LookRotation(direction, UpVector) * Quaternion.Euler(bone.offset);
                var next = Quaternion.Lerp(bone.bone.rotation, final, bone.weight * currentSmoothness);
                bone.bone.rotation = next;
            }
        }

        void LookAtBoneSet_AnimatePhysicsv2()
        {
            hasTarget = false;
            if (UseCamera || Target) hasTarget = true;


            angle = Vector3.Angle(transform.forward, direction);


            foreach (var bone in Bones)
            {
                if (!bone.bone) continue;

                Vector3 dir = transform.forward;

                if (UseCamera)
                {
                    dir = cam.forward;

                    aimHit = MalbersTools.RayCastHitToCenter(bone.bone);

                    if (aimHit.collider)
                    {
                        dir = MalbersTools.DirectionTarget(bone.bone.position, aimHit.point);
                    }
                }
                if (Target) dir = (Target.position - bone.bone.position).normalized;

                direction = Vector3.Lerp(direction, dir, Time.deltaTime * Smoothness);



                angle = Vector3.Angle(transform.forward, direction); 

                if ((angle < LimitAngle && active && hasTarget))
                {
                    var final = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(bone.offset);
                    var next = Quaternion.Lerp(bone.initialRotation, final, bone.weight);
                    bone.initialRotation = Quaternion.Lerp(bone.initialRotation, next, Time.deltaTime * Smoothness * 2);
                    moreRestore = 1;
                    if (debug) Debug.DrawRay(bone.bone.position, direction * 5, Color.green);
                }
                else
                {
                    moreRestore += Time.deltaTime;
                    bone.initialRotation = Quaternion.Lerp(bone.initialRotation, bone.bone.rotation, Time.deltaTime * Smoothness * 2 * moreRestore);

                    moreRestore = Mathf.Clamp(moreRestore, 0, 1000); 
                }
                bone.bone.rotation = bone.initialRotation;
            }
        }
        public void SetTarget(Transform target)
    {
        Target = target;
    }

        void LookAtBoneSet_AnimatePhysics()
        {
            hasTarget = false;
            if (UseCamera || Target) hasTarget = true;


            angle = Vector3.Angle(transform.forward, direction);


            foreach (var bone in Bones)
            {
                if (!bone.bone) continue;

                Vector3 dir = transform.forward;

                if (UseCamera)
                {
                    dir = cam.forward;

                    aimHit = MalbersTools.RayCastHitToCenter(bone.bone);

                    if (aimHit.collider)
                    {
                        dir = MalbersTools.DirectionTarget(bone.bone.position, aimHit.point);
                    }
                }
                if (Target) dir = (Target.position+UpVector - bone.bone.position).normalized;

                direction = Vector3.Lerp(direction, dir, Time.deltaTime * Smoothness);



                angle = Vector3.Angle(transform.forward, direction); 

                if ((angle < LimitAngle && active && hasTarget))
                {
                    var final = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(bone.offset);
                    var next = Quaternion.Lerp(bone.initialRotation, final, Time.deltaTime* Smoothness);
                    bone.initialRotation = Quaternion.Lerp(bone.initialRotation, next, Time.deltaTime * Smoothness * 2);
                    moreRestore = 1;
                    if (debug) Debug.DrawRay(bone.bone.position, direction * 5, Color.green);
                }
                else
                {
                    moreRestore += Time.deltaTime;
                    bone.initialRotation = Quaternion.Lerp(bone.initialRotation, bone.bone.rotation, Time.deltaTime * Smoothness * 2 * moreRestore);

                    moreRestore = Mathf.Clamp(moreRestore, 0, 1000); 
                }
                bone.bone.rotation = bone.initialRotation;
            }
        }



        float moreRestore = 1;

        public virtual void NoTarget()
        {
            Target = null;
        }

        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }
    }
















