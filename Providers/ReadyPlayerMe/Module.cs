using UnityEngine;

namespace Bkpk
{
    public class ReadyPlayerMeModule : GlbLoaderModule
    {
        ReadyPlayerMeMetadata AvatarMetadata;

        public ReadyPlayerMeModule()
        {
            ModuleName = "ready-player-me";
            AvatarMetadata = new ReadyPlayerMeMetadata();
            AxisInverted = true;
        }

        public override void ProcessMetadata(string metadata)
        {
            AvatarMetadata = JsonUtility.FromJson<ReadyPlayerMeMetadata>(metadata);
        }

        public override Avatar HandleRigAvatar()
        {
            if (AvatarMetadata == null)
            {
                return null;
            }

            Avatar avatar;

            switch (AvatarMetadata.bodyType)
            {
                case "masculine":
                    avatar = Resources.Load<Avatar>("RPM/MaleAnimationTargetV2");
                    break;
                case "feminine":
                    avatar = Resources.Load<Avatar>("RPM/FemaleAnimationTargetV2");
                    break;
                default:
                    avatar = Resources.Load<Avatar>("RPM/MaleAnimationTargetV2");
                    break;
            }

            if (avatar == null)
            {
                Debug.LogError("Could not load avatar");
            }
            return avatar;
        }
    }

    [System.Serializable]
    public class ReadyPlayerMeMetadata
    {
        public string bodyType;
        public string outfitGender;
        public string outfitVersion;
    }
}
