namespace AspNetCoreEcommerce.PaymentChannel
{
    public class ErcasPay
    {
        
        private readonly string _ercasPayApiKey = Environment.GetEnvironmentVariable("ERCASPAY_SECRET_KEY");
    }
}
