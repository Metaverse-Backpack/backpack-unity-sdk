using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VRM;
using UniVRM10;
using UniGLTF;

namespace Bkpk
{
    // Parent class for all partner modules
    public class VrmLoaderModule : AvatarLoaderModule
    {
        public VrmLoaderModule()
        {
            ModuleName = "vrm";
        }

        // Spawns the avatar into the input gameobject
        public override BkpkAvatar SpawnAvatar(AvatarInfo avatarInfo, byte[] avatarData)
        {
            BkpkAvatar avatar = new BkpkAvatar();

            // GLB data
            GltfData vrm_glb_data = HandleGLB(avatarData);
            if (vrm_glb_data == null)
                return null;

            // VRM data
            VRMData vrm_data = new VRMData(vrm_glb_data);

            // Import VRM
            using (VRMImporterContext context = new VRMImporterContext(vrm_data))
            {
                context.InvertAxis = Axes.Z;

                RuntimeGltfInstance instance = context.Load();
                instance.EnableUpdateWhenOffscreen();
                instance.ShowMeshes();
                HandleTextures(instance.Textures);
                avatar.AvatarObject = instance.Root;

                avatar.Animator = instance.GetComponent<Animator>();
                Avatar anim_avatar = HandleRigAvatar(avatarInfo);
                if (anim_avatar != null)
                    avatar.Animator.avatar = anim_avatar;
                avatar.Animator.runtimeAnimatorController = AvatarController;
            }

            vrm_glb_data.Dispose();

            return avatar;
        }
    }
}
