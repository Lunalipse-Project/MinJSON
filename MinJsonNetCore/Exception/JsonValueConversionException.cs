namespace MinJSON.Exception
{
    public class JsonValueConversionException : System.Exception
    {
        public JsonValueConversionException() { }
        public JsonValueConversionException(string message) : base(message) { }
        public JsonValueConversionException(string message, System.Exception inner) : base(message, inner) { }
    }
}
