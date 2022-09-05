using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VRM;
using UniVRM10;
using UniGLTF;

namespace Bkpk
{
    // Parent class for all partner modules
    public class AvatarLoaderModule
    {
        // Metadata for the module
        public string ModuleName;
        public bool AxisInverted;
        public FilterMode TextureFilterMode = FilterMode.Bilinear;
        public static RuntimeAnimatorController AvatarController =
            Resources.Load("Bkpk/AnimationFiles/GenericCharacter") as RuntimeAnimatorController;

        public async Task<BkpkAvatar> RequestAvatar(
            AvatarInfo avatarInfo,
            GameObject avatarObject = null
        )
        {
            var result = await Download(avatarInfo.source);

            BkpkAvatar avatar = SpawnAvatar(avatarInfo, (byte[])result);

            if (avatarObject != null)
                avatar.AvatarObject.transform.SetParent(avatarObject.transform);
            avatar.AvatarObject.transform.localPosition = Vector3.zero;
            avatar.AvatarObject.transform.localRotation = Quaternion.identity;

            avatar.AvatarObject.AddComponent<BkpkUpdateFetch>().Avatar = avatar;

            return avatar;
        }

        // Spawns the avatar into the input gameobject
        public virtual BkpkAvatar SpawnAvatar(AvatarInfo avatarInfo, byte[] avatarData)
        {
            return null;
        }

        public Avatar HandleRigAvatar(AvatarInfo avatarInfo)
        {
            Avatar avatar;

            switch (avatarInfo.bodyType)
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

        // GLB parsing
        public GltfData HandleGLB(byte[] avatarData)
        {
            GltfData glb_data = new GlbBinaryParser(avatarData, ModuleName + "_character").Parse();

            return glb_data;
        }

        // Force all textures to use the same filter mode
        public void HandleTextures(IReadOnlyList<Texture> textures)
        {
            foreach (Texture texture in textures)
            {
                if (texture == null)
                    continue;

                texture.filterMode = TextureFilterMode;
            }
        }

        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return true;
            return false;
        }

        // Request downloads, returns the downloadHandler.
        public async Task<object> Download(string url)
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
    }
}
