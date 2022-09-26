using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UniGLTF;
using UniHumanoid;
using UnityEngine;
using UnityEngine.UI;
using VRMShaders;
using VRM;

namespace Bkpk
{
    public class BkpkAvatar : IDisposable
    {
        public Animator Animator;
        public GameObject gameObject;
        RuntimeGltfInstance _instance;
        HumanPoseTransfer _pose;
        VRMBlendShapeProxy m_proxy;

        public BkpkAvatar(RuntimeGltfInstance instance, GameObject target = null)
        {
            _instance = instance;
            gameObject = instance.gameObject;

            var lookAt = instance.GetComponent<VRMLookAtHead>();
            if (lookAt != null)
            {
                // vrm
                _pose = gameObject.AddComponent<HumanPoseTransfer>();
                _pose.Source = GameObject.FindObjectOfType<HumanPoseTransfer>();
                _pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;

                lookAt.Target = target != null ? target.transform : null;
                lookAt.UpdateType = UpdateType.LateUpdate; // after HumanPoseTransfer's setPose

                m_proxy = instance.GetComponent<VRMBlendShapeProxy>();
            }

            // not vrm
            var animation = instance.GetComponent<Animation>();
            if (animation && animation.clip != null)
            {
                animation.Play(animation.clip.name);
            }
        }

        public BkpkAvatar(GameObject avatar, GameObject target = null)
        {
            gameObject = avatar;

            // not vrm
            var animation = avatar.GetComponent<Animation>();
            if (animation && animation.clip != null)
            {
                animation.Play(animation.clip.name);
            }
        }

        public void Dispose()
        {
            // Destroy game object. not RuntimeGltfInstance
            GameObject.Destroy(gameObject);
        }

        public void EnableTPose(HumanPoseClip pose)
        {
            if (_pose != null)
            {
                _pose.PoseClip = pose;
                _pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
            }
        }

        public void OnResetClicked()
        {
            if (_pose != null)
            {
                foreach (var spring in _pose.GetComponentsInChildren<VRMSpringBone>())
                {
                    spring.Setup();
                }
            }
        }

        public void Update()
        {
            if (m_proxy != null)
            {
                m_proxy.Apply();
            }
        }
    }
}
