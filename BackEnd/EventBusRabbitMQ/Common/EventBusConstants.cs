namespace EventBusRabbitMQ.Common
{
    public static class EventBusConstants
    {
        public const string FileCheckoutQueue = "FileCheckoutQueue";
        public const string FileCheckDeleteQueue = "FileCheckDeleteQueue";
        public const string BasketCheckoutQueue = "basketCheckoutQueue";
        public const string UsersCheckoutQueue = "usersCheckoutQueue";
        public const string UserIssuedCheckoutQueue = "userIssuedCheckoutQueue";

        /// <summary>
        /// اضافه کردن اطلاعات ثبت فروشگاه 
        /// </summary>
        public const string StoreCheckoutQueue = "StoreCheckoutQueue";
        public const string DeleteCustomerprocuteCheckoutQueue = "DeleteCustomerprocuteCheckoutQueue";
        public const string UpdataCustomerprocuteCheckoutQueue = "UpdataCustomerprocuteCheckoutQueue";
        public const string UpdataCustomerprocuteCountCheckoutQueue = "UpdataCustomerprocuteCountCheckoutQueue";
        public const string StoreExpireDate = "StoreExpireDate"; ////برای به روز رسانی تاریخ انقضا
        public const string StoreDeleteCheckoutQueue = "StoreDeleteCheckoutQueue";
        public const string StoreCopyCheckoutQueue = "StoreCopyCheckoutQueue";
        public const string CustomerProdcutDeleteCheckoutQueue = "CustomerProdcutDeleteCheckoutQueue";
        public const string CustomerProdcutChangeProdcuteIdCheckoutQueue = "CustomerProdcutChangeProdcuteIdCheckoutQueue"; ////در صورت اذعام محصولات 
        public const string CustomerProdcutChangeBrandIdCheckoutQueue = "CustomerProdcutChangeBrandIdCheckoutQueue"; ////در صورت اذعام برند 



        /// <summary>
        /// به روز رسانی کاربر در identity
        /// </summary>
        public const string UserIdentityCheckoutQueue = "UserIdentityCheckoutQueue";
        public const string UserUpdateCheckoutQueue = "UserUpdateCheckoutQueue";


        /// <summary>
        /// confrim Factor 
        /// </summary>
        public const string FactorModuleCheckoutQueue = "FactorModuleCheckoutQueue";
        public const string FactorProdcutCheckoutQueue = "FactorProdcutCheckoutQueue";
        public const string FactorProdcutByWalletCheckoutQueue = "FactorProdcutByWalletCheckoutQueue";
        public const string FactorProdcutInstallmentCheckoutQueue = "FactorProdcutInstallmentCheckoutQueue";
        // public const string FactorCheckoutQueue = "FactorCheckoutQueue";

        /// <summary>
        /// define
        /// </summary>
        public const string DefineCacheCheckoutQueue = "DefineCacheCheckoutQueue";
        public const string DefineUpdateStoreheckoutQueue = "DefineUpdateStoreheckoutQueue";

        public const string CrmtCheckoutQueue = "CrmtCheckoutQueue";


    }
}
