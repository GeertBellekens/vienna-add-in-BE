using System;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.Utils
{
    public class PropertyEqualityAssertion<T> where T : class
    {
        private readonly T proxy;

        public PropertyEqualityAssertion(T expected, T actual, string message)
        {
            proxy =
                (T)
                new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(typeof (T), expected,
                                                                             new MethodInterceptor(expected, actual,
                                                                                                   message));
        }

        public PropertyEqualityAssertion(T expected, T actual) : this(expected, actual, string.Empty)
        {
        }

        public void AreEqual(Func<T, object> invokeProperty)
        {
            invokeProperty(proxy);
        }

        #region Nested type: MethodInterceptor

        private class MethodInterceptor : IInterceptor
        {
            private readonly T actual;
            private readonly T expected;
            private readonly string message;

            public MethodInterceptor(T expected, T actual, string message)
            {
                this.expected = expected;
                this.actual = actual;
                this.message = message;
            }

            #region IInterceptor Members

            public void Intercept(IInvocation invocation)
            {
                object expectedValue = invocation.Method.Invoke(expected, invocation.Arguments);
                object actualValue = invocation.Method.Invoke(actual, invocation.Arguments);
                invocation.ReturnValue = expectedValue;
                Assert.AreEqual(expectedValue, actualValue,
                                "Properties are not equal: " + typeof (T).Name + "." +
                                invocation.Method.Name.Substring(4) +
                                ": " + message);
            }

            #endregion
        }

        #endregion
    }
}