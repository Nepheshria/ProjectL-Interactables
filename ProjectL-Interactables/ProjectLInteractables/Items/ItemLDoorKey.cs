
// External
using System.Runtime.Serialization;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace ProjectLInteractables.Items {

    public enum DoorColor {
        [EnumMember(Value = "white")]
        White = 1,

        [EnumMember(Value = "black")]
        Black = 2,

        [EnumMember(Value = "red")]
        Red = 3,

        [EnumMember(Value = "dark_red")]
        DarkRed = 4,

        [EnumMember(Value = "orange")]
        Orange = 5,

        [EnumMember(Value = "yellow")]
        Yellow = 6,

        [EnumMember(Value = "lime")]
        Lime = 7,

        [EnumMember(Value = "green")]
        Green = 8,

        [EnumMember(Value = "dark_green")]
        DarkGreen = 9,

        [EnumMember(Value = "mint")]
        Mint = 10,

        [EnumMember(Value = "cyan")]
        Cyan = 11,

        [EnumMember(Value = "teal")]
        Teal = 12,

        [EnumMember(Value = "sky_blue")]
        SkyBlue = 13,

        [EnumMember(Value = "blue")]
        Blue = 14,

        [EnumMember(Value = "navy")]
        Navy = 15,

        [EnumMember(Value = "purple")]
        Purple = 16,

        [EnumMember(Value = "violet")]
        Violet = 17,

        [EnumMember(Value = "magenta")]
        Magenta = 18,

        [EnumMember(Value = "pink")]
        Pink = 19,

        [EnumMember(Value = "brown")]
        Brown = 20,

        [EnumMember(Value = "tan")]
        Tan = 21,

        [EnumMember(Value = "beige")]
        Beige = 22,

        [EnumMember(Value = "gray")]
        Gray = 23,

        [EnumMember(Value = "light_gray")]
        LightGray = 24,

        [EnumMember(Value = "dark_gray")]
        DarkGray = 25,

        [EnumMember(Value = "silver")]
        Silver = 26,

        [EnumMember(Value = "gold")]
        Gold = 27,

        [EnumMember(Value = "bronze")]
        Bronze = 28,

        [EnumMember(Value = "olive")]
        Olive = 29,

        [EnumMember(Value = "maroon")]
        Maroon = 30
    }

    internal class ItemLDoorKey : Item {

    }
}
