using GiaiKhatNgocMai.Infrastructure.Utils;
using System;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class ItemPhotoModel
    {
        public ItemPhotoModel() {}
        public ItemPhotoModel(ItemPhoto photo):this()
        {
            ModelObjectHelper.CopyObject(photo, this);
        }
        public int Id { get; set; }
        public string PhotoDescription { get; set; }
        public string PhotoFileName { get; set; }
        public Nullable<int> ItemId { get; set; }

        public ItemPhoto ToEntity()
        {
            var photo = new ItemPhoto();
            ModelObjectHelper.CopyObject(this, photo);
            return photo;
        }
    }
}