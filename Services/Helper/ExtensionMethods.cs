namespace Services.Helper
{
    public static class ExtensionMethods
    {

        public class ModelShowAttribute : System.Attribute
        {
            public bool IsLast { get; set; }
            public string EventName { get; set; }

            public ModelShowAttribute(bool isLast, string eventName)
            {
                this.IsLast = isLast;
                this.EventName = eventName;
            }
        }
          public class InputGroupAttribute : System.Attribute
        {
            public string Name { get; set; }

            public InputGroupAttribute(string Name)
            {
                this.Name = Name;
            }
        }

    }
}
