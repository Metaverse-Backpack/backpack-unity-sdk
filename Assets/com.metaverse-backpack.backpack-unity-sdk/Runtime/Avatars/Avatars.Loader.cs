using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UniGLTF;
using Siccity.GLTFUtility;
using UniHumanoid;
using UnityEngine;
using UnityEngine.Networking;
using VRMShaders;
using VRM;

namespace Bkpk
{
    public static partial class Avatars
    {
        private const string BONE_HIPS = "Hips";
        private const string BONE_ARMATURE = "Armature";

        public static RuntimeAnimatorController GenericController =
            Resources.Load<RuntimeAnimatorController>(
                "MetaverseBackpack/Controllers/GenericCharacter"
            );

        public static Avatar MaleAnimationTarget = Resources.Load<Avatar>(
            "MetaverseBackpack/AnimationTargets/MasculineAnimationAvatar"
        );

        public static Avatar FemaleAnimationTarget = Resources.Load<Avatar>(
            "MetaverseBackpack/AnimationTargets/FeminineAnimationAvatar"
        );

        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return true;
            return false;
        }

        // Request downloads, returns the downloadHandler.
        public static async Task<object> Download(string url)
        {
            if (!IsValidUrl(url))
                throw new BkpkException(BkpkErrors.INVALID_URL);

            // Construct the request
            UnityWebRequest request = UnityWebRequest.Get(url);

            // Send the request
            request.SendWebRequest();

            // Wait for the request to complete
            while (!request.isDone)
            {
                await Task.Yield();
            }

            // Check for errors
            if (
                request.result != UnityWebRequest.Result.Success
                || request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError
                || request.result == UnityWebRequest.Result.DataProcessingError
            )
            {
                throw new BkpkException(BkpkErrors.DOWNLOAD_FAILED);
            }

            return request.downloadHandler.data;
        }

        static BkpkAvatar GetGlbModel(AvatarInfo avatarInfo, GameObject avatarGo, GameObject target)
        {
            if (!avatarGo.transform.Find(BONE_ARMATURE))
            {
                var armature = new GameObject();
                armature.name = BONE_ARMATURE;
                armature.transform.parent = avatarGo.transform;

                Transform hips = avatarGo.transform.Find(BONE_HIPS);
                hips.parent = armature.transform;
            }

            BkpkAvatar avatar = new BkpkAvatar(avatarGo, target);

            avatar.Animator = avatarGo.GetComponent<Animator>();
            if (avatar.Animator == null)
            {
                avatar.Animator = avatarGo.AddComponent<Animator>();
            }
            Avatar anim_avatar = GetAvatarRig(avatarInfo);

            if (anim_avatar != null)
                avatar.Animator.avatar = anim_avatar;

            avatar.Animator.runtimeAnimatorController = GenericController;
            avatar.Animator.applyRootMotion = true;

            return avatar;
        }

        static BkpkAvatar GetVrmModel(
            AvatarInfo avatarInfo,
            RuntimeGltfInstance instance,
            GameObject target
        )
        {
            var _ = FastSpringBoneReplacer.ReplaceAsync(instance.Root);
            instance.EnableUpdateWhenOffscreen();
            instance.ShowMeshes();

            GameObject avatarGo = instance.gameObject;

            BkpkAvatar avatar = new BkpkAvatar(instance, target);

            avatar.Animator = avatarGo.GetComponent<Animator>();
            if (avatar.Animator == null)
            {
                avatar.Animator = avatarGo.AddComponent<Animator>();
            }
            Avatar anim_avatar = GetAvatarRig(avatarInfo);

            if (anim_avatar != null)
                avatar.Animator.avatar = anim_avatar;

            avatar.Animator.runtimeAnimatorController = GenericController;
            avatar.Animator.applyRootMotion = true;

            return avatar;
        }

        static Avatar GetAvatarRig(AvatarInfo avatarInfo)
        {
            if (!avatarInfo.metadata.fileFormat.Equals("glb"))
                return null;
            switch (avatarInfo.metadata.bodyType)
            {
                case "humanoid-female":
                    return FemaleAnimationTarget;
                default:
                    return MaleAnimationTarget;
            }
        }

        static async Task<BkpkAvatar> LoadModelAsync(
            AvatarInfo avatarInfo,
            string uri,
            byte[] bytes,
            GameObject target
        )
        {
            RuntimeGltfInstance instance = null;
            switch (avatarInfo.metadata.fileFormat)
            {
                case "gltf":
                case "glb":
                case "zip":
                {
                    var avatar = Importer.LoadFromBytes(bytes, new ImportSettings());
                    return GetGlbModel(avatarInfo, avatar, target);
                }

                case "vrm":
                {
                    VrmUtility.MaterialGeneratorCallback materialCallback = (
                        VRM.glTF_VRM_extensions vrm
                    ) => new VRM.VRMMaterialDescriptorGenerator(vrm);

                    instance = await VrmUtility.LoadBytesAsync(
                        uri,
                        bytes,
                        new ImmediateCaller(),
                        materialCallback,
                        loadAnimation: true
                    );
                    return GetVrmModel(avatarInfo, instance, target);
                }
            }

            throw new BkpkException(BkpkErrors.INVALID_FILE_FORMAT);
        }

        public static async Task<BkpkAvatar> LoadAvatar(
            AvatarInfo avatarInfo,
            GameObject target = null
        )
        {
            string uri = Config.IpfsGateway + "/ipfs/" + avatarInfo.content;

            byte[] result = (byte[])(await Download(uri));

            BkpkAvatar avatar = await LoadModelAsync(avatarInfo, uri, result, target);

            return avatar;
        }
    }
}
