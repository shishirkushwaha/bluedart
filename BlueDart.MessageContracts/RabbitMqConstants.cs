namespace BlueDart.Messaging
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "YourRebbitMqURI";
        public const string UserName = "YourUserName";
        public const string Password = "YourPassword";
        public const string RegisterOrderServiceQueue = "registerorder.service";
        public const string RegisterOrderRollbackServiceQueue = "registerorderrollback.service";
        public const string NotificationServiceQueue = "notification.service";
        public const string PaymentServiceQueue = "payment.service";
        public const string ShippingServiceQueue = "shipping.service";
        public const string SagaQueue = "saga.service";
    }
}
