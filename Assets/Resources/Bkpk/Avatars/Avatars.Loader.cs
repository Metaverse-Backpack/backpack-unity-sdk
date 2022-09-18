using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UniGLTF;
using UniHumanoid;
using UnityEngine;
using UnityEngine.Networking;
using VRMShaders;
using VRM;

namespace Bkpk
{
    public static partial class Avatars
    {
        public static RuntimeAnimatorController AvatarController =
            Resources.Load("Bkpk/AnimationFiles/GenericCharacter") as RuntimeAnimatorController;

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

        static BkpkAvatar GetModel(
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
            Avatar anim_avatar = HandleRigAvatar(avatarInfo);

            if (anim_avatar != null)
                avatar.Animator.avatar = anim_avatar;
            avatar.Animator.runtimeAnimatorController = AvatarController;

            return avatar;
        }

        static Avatar HandleRigAvatar(AvatarInfo avatarInfo)
        {
            Avatar avatar;

            switch (avatarInfo.metadata.bodyType)
            {
                case "humanoid-female":
                    avatar =
                        Resources.Load<Avatar>("Bkpk/AnimationFiles/FemaleAnimationTargetV2.fbx")
                        as Avatar;
                    break;
                default:
                    avatar =
                        Resources.Load<Avatar>("Bkpk/AnimationFiles/MaleAnimationTargetV2.fbx")
                        as Avatar;
                    ;
                    break;
            }

            return avatar;
        }

        static async Task<BkpkAvatar> LoadModelAsync(
            AvatarInfo avatarInfo,
            string uri,
            byte[] bytes,
            GameObject target
        )
        {
            var size = bytes != null ? bytes.Length : 0;
            switch (avatarInfo.metadata.fileFormat)
            {
                case "gltf":
                case "glb":
                case "zip":
                {
                    var instance = await GltfUtility.LoadAsync(
                        uri,
                        new ImmediateCaller(),
                        new GltfMaterialDescriptorGenerator()
                    );
                    return GetModel(avatarInfo, instance, target);
                    break;
                }

                case "vrm":
                {
                    VrmUtility.MaterialGeneratorCallback materialCallback = (
                        VRM.glTF_VRM_extensions vrm
                    ) => new VRM.VRMMaterialDescriptorGenerator(vrm);

                    var instance = await VrmUtility.LoadBytesAsync(
                        uri,
                        bytes,
                        new ImmediateCaller(),
                        materialCallback,
                        loadAnimation: true
                    );
                    return GetModel(avatarInfo, instance, target);
                    break;
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
