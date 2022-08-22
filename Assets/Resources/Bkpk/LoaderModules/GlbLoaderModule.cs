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
    public class GlbLoaderModule : AvatarLoaderModule
    {
        public GlbLoaderModule()
        {
            ModuleName = "glb";
        }

        // Spawns the avatar into the input gameobject
        public override BkpkAvatar SpawnAvatar(byte[] avatarData)
        {
            BkpkAvatar avatar = new BkpkAvatar();

            // GLB data
            GltfData glb_data = HandleGLB(avatarData);

            if (glb_data == null)
                return null;

            using (ImporterContext context = new ImporterContext(glb_data))
            {
                context.InvertAxis = AxisInverted ? Axes.X : Axes.Z;

                RuntimeGltfInstance instance = context.Load();
                instance.EnableUpdateWhenOffscreen();
                instance.ShowMeshes();
                HandleTextures(instance.Textures);
                avatar.AvatarObject = instance.Root;

                avatar.Animator = avatar.AvatarObject.AddComponent<Animator>();
                Avatar anim_avatar = HandleRigAvatar();
                if (anim_avatar != null)
                    avatar.Animator.avatar = anim_avatar;
                avatar.Animator.runtimeAnimatorController = AvatarController;
            }

            glb_data.Dispose();

            return avatar;
        }
    }
}
