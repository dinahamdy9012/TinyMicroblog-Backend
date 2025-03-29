using TinyMicroblog.SharedKernel.Enums;

namespace TinyMicroblog.SharedKernel.Constants
{
    public static class ImageResizeSizesConstant
    {
        public static Dictionary<string, int> IMAGE_RESIZE_SIZES = new Dictionary<string, int>
        {
            { nameof(ImageTypeEnum.Small), 320 },
            { nameof(ImageTypeEnum.Medium), 640 },
            { nameof(ImageTypeEnum.Large), 1280 }
        };

    }
}
