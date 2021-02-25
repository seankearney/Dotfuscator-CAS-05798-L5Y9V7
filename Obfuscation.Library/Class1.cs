using System;

namespace Obfuscation.Library
{
    public class Class1
    {
        public string TestProperty { get; set; }

        public void Method()
        {
            // Silly code just to ensure contents of method is obfuscated
            TestProperty = Guid.NewGuid().ToString("N");
        }

        internal void InternalMethod()
        {
            // This method should be renamed with obfuscation
            TestProperty = Guid.NewGuid().ToString("N");
        }
    }
}
