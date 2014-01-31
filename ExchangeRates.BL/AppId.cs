namespace ExchangeRates.BL
{
    /// <summary>
    /// Token with guid for accessing http://openexchangerates.org/
    /// </summary>
    public class AppId
    {
        public AppId(string appId)
        {
            Value = appId;
        }

        public string Value { get; private set; }
    }
}